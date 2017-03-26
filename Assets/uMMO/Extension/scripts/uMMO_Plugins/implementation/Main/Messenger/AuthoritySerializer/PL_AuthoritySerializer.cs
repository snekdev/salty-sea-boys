using UnityEngine;
using System.Collections;

using SoftRare.Serialization;

namespace SoftRare.Net.Plugin {
    public abstract class PL_AuthoritySerializer : PL_Messenger {

        public float sendInterval = 0.4f;
        public float readInterval = 0.01f;
        public int decimalPlaces = 3;
        public AuthoritySerializerReadingMethod readingMethod = AuthoritySerializerReadingMethod.ReadPeriodically;
        //public float timeAccumulator = 0f;

        private AValue Sync(string key, AValue value, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {

            AValue tmpValue = getValue(key, InputReadingMethod.CanReadSameValueMultipleTimes);
            if (hasAuthority()) {
                bool isDifferent = false;
                if (value.CompareTo(tmpValue) != 0) {
                    isDifferent = true;
                }
                
                saveValue(key, value);

                if (!saveOnlyNoSend) {
                    if (isDifferent || sendWithoutDiffCheck) {
                        sendUpdate(key, value);
                    }
                }
                    
            } else {
                value = tmpValue;
            }
            
            return value;
        }

        protected void Sync(string key, ref Vector3 value, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new Vector3Value(value, "F"+decimalPlaces), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<Vector3>(res);
            }
        }

        protected void Sync(string key, ref Vector3 value, string format, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new Vector3Value(value, format), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<Vector3>(res);
            }
        }

        protected void Sync(string key, ref Quaternion value, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new QuaternionValue(value, "F"+decimalPlaces), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<Quaternion>(res);
            }
        }

        protected void Sync(string key, ref Quaternion value, string format, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new QuaternionValue(value, format), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<Quaternion>(res);
            }
        }

        protected void Sync(string key, ref bool value, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new BoolValue(value), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<bool>(res);
            }
        }

        protected void Sync(string key, ref double value, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new DoubleValue(value, "F"+decimalPlaces), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<double>(res);
            }
        }

        protected void Sync(string key, ref double value, string format, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new DoubleValue(value, format), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<double>(res);
            }
        }

        protected void Sync(string key, ref float value, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new FloatValue(value, "F"+decimalPlaces), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<float>(res);
            }
        }

        protected void Sync(string key, ref float value, string format, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new FloatValue(value, format), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<float>(res);
            }
        }

        protected void Sync(string key, ref int value, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new IntValue(value), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<int>(res);
            }
        }

        protected void Sync(string key, ref string value, bool saveOnlyNoSend = false, bool sendWithoutDiffCheck = false) {
            AValue res = Sync(key, new StringValue(value), saveOnlyNoSend, sendWithoutDiffCheck);
            if (res != null) {
                value = AValue.to<string>(res);
            }
        }

        //sending on authority side
        public void sendUpdate(string key, AValue value) {

            if (myNetObject.isServer) {
                sendMsgToClients(key, value);
            } else if (myNetObject.isClient) {
                sendMsgToServer(key, value, true);
            }

        }

        public override void __receiveValue(string key, AValue value, double latestRemoteTimestampOfChange) {
            base.__receiveValue(key, value, latestRemoteTimestampOfChange); //saves the value
            if (readingMethod == AuthoritySerializerReadingMethod.ReadOnPeriodicallyAndOnDemand || readingMethod == AuthoritySerializerReadingMethod.ReadOnDemand) {
                OnNonAuthorityDeserialize(latestRemoteTimestampOfChange);
            }
        }

        public abstract void OnAuthoritySerialize();

        public abstract void OnNonAuthorityDeserialize(double latestRemoteTimestampOfChange);

        protected void write() {
            if (initialized && (uMMO.get.isServer || uMMO.get.isClient)) {
                OnAuthoritySerialize();
            }
        }

        protected void read() {
            if (initialized && (uMMO.get.isServer || uMMO.get.isClient)) {
                OnNonAuthorityDeserialize(latestRemoteTimestampOfChange);
            }
        }

        public override void __start() {
            base.__start();
            if (hasAuthority()) {
                InvokeRepeating("write", 0f, sendInterval);
            } else if (readingMethod == AuthoritySerializerReadingMethod.ReadOnPeriodicallyAndOnDemand || readingMethod == AuthoritySerializerReadingMethod.ReadPeriodically) {
                InvokeRepeating("read", 0f, readInterval);
            }
        }

        /*public override void __awake(GameObject pluginOwner) {
            base.__init(pluginOwner);

        }*/
    }
}
