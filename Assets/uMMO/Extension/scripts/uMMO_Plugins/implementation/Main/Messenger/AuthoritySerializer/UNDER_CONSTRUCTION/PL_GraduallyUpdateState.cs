using UnityEngine;
using System;
using System.Collections;

// For the client which owns the player, the local transform contains 
// the current client _predicted_ position/rotation. The buffered state
// contains the _server_ position/rotation, which is the correct state. The
// predicted and server states are interpolated together so the predicted
// state will always converge with the server state.
//
// For the server the transform contains the current 100% legal position and
// rotation of the player in question. He sends this over the network
// to all clients.
//
// For other clients, which don't own this player, the buffered state contains 
// the correct values. This is played back with a 100 ms delay to elminate
// choppyness. The delay needs to be higher if the ping time between the server
// and client is larger than 100 ms.
namespace SoftRare.Net.Plugin {
    public class PL_GraduallyUpdateState {/*: PL_AuthoritySerializer {

    internal struct State {
        internal double timestamp;
        internal Vector3 pos;
        internal Quaternion rot;
    }
    // How far behind should server data be played back during remote player
    // interpolation. The poorer the connection the higher this value needs to
    // be. Fast connections should do fine with 0.1. The server latency
    // affects this the most.
    public double m_InterpolationBackTime = 0.1;

    public bool noErrorCorrection; // doesn't correct errors, so m_PredictionThreshold and m_TimeThreshold have no effect

    // The position vector distance to start error correction. The higher the latency the higher this
    // value should be or it constantly tries to correct errors in prediction, of course this depends
    // on the game too.
    [SerializeField]
    public float m_PredictionThreshold = 0.5f;

    // Time difference in milliseconds where we check for error in position. If the server time value
    // of a state is too different from the local state time then the error correction comparison is
    // highly unreliable and you might try to correct more errors than there really are.
    [SerializeField]
    public float m_TimeThreshold = 0.05f;

    // We store twenty states with "playback" information
    State[] m_BufferedState = new State[20];
    State[] m_LocalBufState = new State[20];
    // Keep track of what slots are used
    int m_TimestampCount;
    int m_LocalStateCount;

    [HideInInspector]
    public bool m_IsMine = false;

    // Stat variables for latency, msg rate
    [HideInInspector]
    [SerializeField]
    public float m_Timer;
    [HideInInspector]
    [SerializeField]
    public int m_MsgCounter = 0;
    [HideInInspector]
    [SerializeField]
    public double m_MsgLatencyTotal = 0;

    // Stat variabels for prediction stuff
    [HideInInspector]
    [SerializeField]
    public float m_TimeAccuracy = 0;
    [HideInInspector]
    [SerializeField]
    public float m_PredictionAccuracy = 0;
    bool m_FixError = false;
    Vector3 m_NewPosition;

    void Awake() {
        m_Timer = Time.time + 1;
    }

    // We need to grab a reference to the isMoving variable in the javascript ThirdPersonController script
    void Start() {
        //targetController = GetComponent("ThirdPersonController");
        //isMovingFieldInfo=targetController.GetType().GetField("isMoving");

    }

    // Convert field info from character controller script to a local bool variable
    bool targetIsMoving
    {
        get
        {
            return myNetObject.isLocalClientGeneratingInput;
        }
    }

    IEnumerator MonitorLocalMovement() {
        while (myNetObject.pluginShouldSyncPosition || myNetObject.pluginShouldSyncRotation) {
            yield return new WaitForSeconds(1 / 15);

            // Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
            for (int i = m_LocalBufState.Length - 1; i >= 1; i--) {
                m_LocalBufState[i] = m_LocalBufState[i - 1];
            }

            // Save currect received state as 0 in the buffer, safe to overwrite after shifting
            State state;
            state.timestamp = Utils.Library.NetworkTime;
            state.pos = myNetObject.transform.position;
            state.rot = myNetObject.transform.rotation;
            m_LocalBufState[0] = state;

            // Increment state count but never exceed buffer size
            m_LocalStateCount = Mathf.Min(m_LocalStateCount + 1, m_LocalBufState.Length);

            //
            // Check if the client side prediction has an error
            //

            // Find the local buffered state which is closest to network state in time
            int j = 0;
            bool match = false;
            for (j = 0; j < m_LocalStateCount - 1; j++) {
                if (m_BufferedState[0].timestamp <= m_LocalBufState[j].timestamp && m_LocalBufState[j].timestamp - m_BufferedState[0].timestamp <= m_TimeThreshold) {
                    //Debug.Log("Comparing state " + j + "localtime: " + m_LocalBufState[j].timestamp  + " networktime: " + m_BufferedState[0].timestamp);
                    //Debug.Log("Local: " + m_LocalBufState[j].pos + " Network: " + m_BufferedState[0].pos);
                    m_TimeAccuracy = Mathf.Abs((float)m_LocalBufState[j].timestamp - (float)m_BufferedState[0].timestamp);
                    m_PredictionAccuracy = (Vector3.Distance(m_LocalBufState[j].pos, m_BufferedState[0].pos));
                    match = true;
                    break;
                }
            }
            if (!match) {
                //Debug.Log("No match!");
            }
            // If prediction is off, diverge current location by the amount of the offset
            else if (m_PredictionAccuracy > m_PredictionThreshold) {
                //Debug.Log("Error in prediction("+m_PredictionAccuracy+"), local is " + m_LocalBufState[j].pos + " network is " + m_BufferedState[0].pos);
                //Debug.Log("Local time: " + m_LocalBufState[j].timestamp + " Network time: " + m_BufferedState[0].timestamp);

                // Find how far we travelled since the prediction failed
                Vector3 localMovement = m_LocalBufState[j].pos - m_LocalBufState[0].pos;

                // "Erase" old values in the local buffer
                m_LocalStateCount = 1;

                // New position which we need to converge to in the update loop				
                m_NewPosition = m_BufferedState[0].pos + localMovement;

                // Trigger the new position convergence routine				
                m_FixError = true;

            } else {
                m_FixError = false;
            }
        }
    }

    // The network sync routine makes sure m_BufferedState always contains the last 20 updates
    // The latest update is in slot 0, oldest in slot 19
    public override void OnAuthoritySerialize() {
        if (myNetObject.pluginShouldSyncPosition || myNetObject.pluginShouldSyncRotation) {
            // Always send transform (depending on reliability of the network view)
            Vector3 pos = myNetObject.transform.position;
            Quaternion rot = myNetObject.transform.rotation;
            if (myNetObject.pluginShouldSyncPosition)
                stream.Serialize(ref pos);
            if (myNetObject.pluginShouldSyncRotation)
                stream.Serialize(ref rot);
        }

    }

    public override void OnNonAuthorityDeserialize() {
        if (myNetObject.pluginShouldSyncPosition || myNetObject.pluginShouldSyncRotation) {
            // When receiving, buffer the information
            m_MsgCounter++;
            m_MsgLatencyTotal += (Utils.Library.NetworkTime - latestTimestamp);

            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = myNetObject.transform.rotation;//Quaternion.identity;
            if (myNetObject.pluginShouldSyncPosition)
                stream.Serialize(ref pos);
            if (myNetObject.pluginShouldSyncRotation)
                stream.Serialize(ref rot);

            // Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
            for (int i = m_BufferedState.Length - 1; i >= 1; i--) {
                m_BufferedState[i] = m_BufferedState[i - 1];
            }

            // Save currect received state as 0 in the buffer, safe to overwrite after shifting
            State state;
            state.timestamp = latestTimestamp;
            state.pos = pos;
            state.rot = rot;
            m_BufferedState[0] = state;

            // Increment state count but never exceed buffer size
            m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);

            // Check integrity, lowest numbered state in the buffer is newest and so on
            for (int i = 0; i < m_TimestampCount - 1; i++) {
                if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
                    Debug.Log("State inconsistent");
            }

            //Debug.Log("stamp: " + info.timestamp + "my time: " + Network.time + "delta: " + (Network.time - info.timestamp));
        }

    }

    void __uMMO_localPlayer_init() {
        Debug.Log("Setting ownership for local player");
        m_IsMine = true;
        StartCoroutine(MonitorLocalMovement());
    }

    // This should run on all clients if server setup is authoritative
    [System.Reflection.Obfuscation]
    void Update() {

        if (myNetObject.pluginShouldSyncPosition || myNetObject.pluginShouldSyncRotation) {

            double currentTime = Utils.Library.NetworkTime;
            double interpolationTime = currentTime - m_InterpolationBackTime;
            // We have a window of interpolationBackTime where we basically play 
            // By having interpolationBackTime the average ping, you will usually use interpolation.
            // And only if no more data arrives we will use extrapolation

            // If this is my player interpolate server position with the position set by me
            if (m_IsMine && m_FixError && !noErrorCorrection) {
                Vector3 difference = m_NewPosition - myNetObject.transform.position;
                    // This is a cheap method for interpolating server and client positions. The higher
                    // the difference the closer to the client state we will go. This is to minimize big jumps
                    // in the movment. This can be done differenctly, like for example just doing 50/50 server
                    // and client position.
                    myNetObject.transform.position = Vector3.Lerp(m_NewPosition, myNetObject.transform.position, difference.magnitude);
                // If we are not moving converge to the 100% accurate server position
            } else if (m_IsMine && !targetIsMoving && !noErrorCorrection) {
                    myNetObject.transform.position = Vector3.Lerp(m_BufferedState[0].pos, myNetObject.transform.position, 0.95F);
                // Use interpolation for other remote clients
            } else if (uMMO.get.isClient && (!m_IsMine || noErrorCorrection)) {

                // Use interpolation
                // Check if latest state exceeds interpolation time, if this is the case then
                // it is too old and extrapolation should be used
                if (m_BufferedState[0].timestamp > interpolationTime) {
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
                                    myNetObject.transform.position = Vector3.Lerp(lhs.pos, rhs.pos, t);
                            if (myNetObject.pluginShouldSyncRotation)
                                    myNetObject.transform.rotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
                            //m_InterpolationTime = (Network.time - m_BufferedState[i].timestamp)*1000;
                            return;
                        }
                    }
                }
                // Use extrapolation. Here we do something really simple and just repeat the last
                // received state. You can do clever stuff with predicting what should happen.
                else {
                    State latest = m_BufferedState[0];

                    if (myNetObject.pluginShouldSyncPosition)
                            myNetObject.transform.localPosition = latest.pos;
                    if (myNetObject.pluginShouldSyncRotation)
                            myNetObject.transform.localRotation = latest.rot;
                    //Debug.Log("Extrapolating " + latest.pos);
                }
            }
        }*/
    }
}
