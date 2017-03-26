using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoftRare.Net.Plugin {
    public class PL_NetworkInterpolatedTransform : PL_AuthoritySerializer {
        public double interpolationBackTime = 0.1;

        //public bool useGlobalInterpolationBackTime; //uses interpolationBackTime from uMMO_NetObject_NetworkViewSerializer_GraduallyUpdateState, requires uMMO_NetObject_globalInterpolationBackTime module

        internal class State {
            internal double timestamp;
            internal Vector3 pos;
            internal Quaternion rot;
            internal bool visible;
        }

        bool writing = false;
        bool reading = false;

        // We store X states with "playback" information
        State[] m_BufferedState = new State[10];
        // Keep track of what slots are used
        int m_TimestampCount;

        public override void OnAuthoritySerialize() {
            writing = true;

            Vector3 pos = myNetObject.transform.localPosition;
            Quaternion rot = myNetObject.transform.localRotation;
            
            
            if (myNetObject.pluginShouldSyncPosition) {
                Sync("p", ref pos, "F10");
            }
            
            if (myNetObject.pluginShouldSyncRotation) {
                Sync("r", ref rot, "F10");
            }
        }

        // When receiving, buffer the information
        public override void OnNonAuthorityDeserialize(double latestRemoteTimestampOfChange) {
            // Receive latest state information
            reading = true;
            if (myNetObject.pluginShouldSyncPosition || myNetObject.pluginShouldSyncRotation) {

                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;

                if (myNetObject.pluginShouldSyncPosition) {
                    Sync("p", ref pos);
                }

                if (myNetObject.pluginShouldSyncRotation) {
                    Sync("r", ref rot);
                }

                // Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
                for (int i = m_BufferedState.Length - 1; i >= 1; i--) {
                    m_BufferedState[i] = m_BufferedState[i - 1];
                }

                // Save currect received state as 0 in the buffer, safe to overwrite after shifting
                State state = new State();
                state.timestamp = latestRemoteTimestampOfChange;
                state.pos = pos;
                state.rot = rot;
                state.visible = false;
                m_BufferedState[0] = state;

                // Increment state count but never exceed buffer size
                m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);

                // Check integrity, lowest numbered state in the buffer is newest and so on
                for (int i = 0; i < m_TimestampCount - 1; i++) {
                    if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
                        Debug.Log("State inconsistent");
                }

            }
            //Debug.Log("stamp: " + info.timestamp + "my time: " + Network.time + "delta: " + (Network.time - info.timestamp));

        }

        // This only runs on remote peers (server/clients)
        [System.Reflection.Obfuscation]
        void Update() {

            if (myNetObject.pluginShouldSyncPosition || myNetObject.pluginShouldSyncRotation) {
                if (reading) {

                    double currentTime = Utils.Library.NetworkTime;

                    /*if (useGlobalInterpolationBackTime)
                        interpolationBackTime = uMMO_StaticLibrary.global_InterpolationBackTime;*/

                    double interpolationTime = currentTime - interpolationBackTime;
                    // We have a window of interpolationBackTime where we basically play 
                    // By having interpolationBackTime the average ping, you will usually use interpolation.
                    // And only if no more data arrives we will use extrapolation

                    // Use interpolation
                    // Check if latest state exceeds interpolation time, if this is the case then
                    // it is too old and extrapolation should be used
                    if (m_BufferedState[0].timestamp > interpolationTime && uMMO.get.isClient) {
                        //print("int");
                        for (int i = 0; i < m_TimestampCount; i++) {
                            // Find the state which matches the interpolation time (time+0.1) or use last state
                            if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount - 1) {
                                // The state one slot newer (<100ms) than the best playback state
                                State rhs = m_BufferedState[Mathf.Max(i - 1, 0)];
                                // The best playback state (closest to 100 ms old (default time))
                                State lhs = m_BufferedState[i];

                                // Use the time between the two slots to determine if interpolation is necessary
                                double length = rhs.timestamp - lhs.timestamp;
                                float t = 0.0F;
                                // As the time difference gets closer to 100 ms t gets closer to 1 in 
                                // which case rhs is only used
                                if (length > 0.0001)
                                    t = (float)((interpolationTime - lhs.timestamp) / length);

                                // if t=0 => lhs is used directly
                                if (myNetObject.pluginShouldSyncPosition)
                                    myNetObject.transform.localPosition = Vector3.Lerp(lhs.pos, rhs.pos, t);
                                if (myNetObject.pluginShouldSyncRotation)
                                    myNetObject.transform.localRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
                                return;
                            }
                        }
                    }
                    // Use extrapolation. Here we do something really simple and just repeat the last
                    // received state. You can do clever stuff with predicting what should happen.
                    else if(uMMO.get.isClient){
                        //on the client we lerp to that latest position
                        //print("ext");
                        State latest = m_BufferedState[0];
                        if (myNetObject.pluginShouldSyncPosition) {
                            myNetObject.transform.position = Vector3.Lerp(myNetObject.transform.position, latest.pos, Time.deltaTime);
                        }
                        if (myNetObject.pluginShouldSyncRotation) {
                            myNetObject.transform.rotation = Quaternion.Lerp(myNetObject.transform.rotation, latest.rot, Time.deltaTime);
                        }
                    } else if (uMMO.get.isServer) {

                        //no interpolation/lerp needed on the server
                        //print("ext");
                        State latest = m_BufferedState[0];
                        if (myNetObject.pluginShouldSyncPosition) {
                            myNetObject.transform.localPosition = latest.pos;

                        }
                        if (myNetObject.pluginShouldSyncRotation) {
                            myNetObject.transform.localRotation = latest.rot;

                        }
                    }
                }
            }
        }

        void OnDrawGizmosSelected() {
            
            for(int i=0; i< m_BufferedState.Length; i++) {
                State state = m_BufferedState[i];

                if (state == null ) break;

                double currentTime = Utils.Library.NetworkTime;
                double interpolationTime = currentTime - interpolationBackTime;
                string status = "interpolating";
                if (state.timestamp > interpolationTime) {
                    Gizmos.color = Color.yellow;
                } else {
                    Gizmos.color = Color.blue;
                    status = "old";
                }
                state.visible = true;

                Gizmos.DrawWireCube(state.pos + (Vector3.up * 1.2f), new Vector3(0.8f, 1.8f, 0.8f));
#if UNITY_EDITOR
                if (i<5)
                Handles.Label(state.pos, "object: position update\nstatus: "+ status + "\ntime received: " + state.timestamp);
#endif          
            }
        }

    }
}