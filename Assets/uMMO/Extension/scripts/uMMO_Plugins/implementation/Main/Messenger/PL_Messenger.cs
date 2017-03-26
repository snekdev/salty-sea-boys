using UnityEngine;
using System.Collections;

using SoftRare.Serialization;

namespace SoftRare.Net.Plugin {
    public abstract class PL_Messenger : PL_Main {
        protected NetObject myNetObject;
        protected string UID;
        protected double latestRemoteTimestampOfChange;
        public AuthoritySpecification authoritySpecification = AuthoritySpecification.Use_AuthoritySpecificationOfNetObject;
        public int QoS_ChannelID;

        public string getUID() {
            return UID;
        }

        public override void __awake(GameObject pluginOwner) {
            base.__awake(pluginOwner);

            myNetObject = pluginOwner.GetComponent<NetObject>();
            UID = Utils.Library.GetHashString(this.ToString()); //TODO: more efficient hash function?
        }

        public bool hasAuthority() {            //v.2.0.8+: new way to determine where authority lies on every Messenger plugin. To give plugins possibility to have authority - altough the UNet prefab does not have it
            bool ret = false;

            if (authoritySpecification == AuthoritySpecification.Use_AuthoritySpecificationOfNetObject) {
                if (myNetObject.hasAuthority) {
                    ret = true;
                }
            } else if (authoritySpecification == AuthoritySpecification.Override_LocalClientAuthority) { 
                if (myNetObject.isLocalPlayer) {
                    ret = true;
                }
            } else if (authoritySpecification == AuthoritySpecification.Override_ServerAuthority) {
                if (uMMO.get.isServer) {
                    ret = true;
                }
            }

            return ret;
        }

        public virtual void __receiveValue(string key, AValue value, double latestRemoteTimestampOfChange) {
            this.latestRemoteTimestampOfChange = latestRemoteTimestampOfChange;
            saveValue(key, value);
        }

        protected AValue getValue(string key, InputReadingMethod readingMethod) {
            return myNetObject.getPluginValue(UID, key, readingMethod);
        }

        protected void saveValue(string key, AValue value) {
            myNetObject.savePluginValue(UID, key, value);
        }

        protected void sendMsgToServer(string key, AValue value, bool serverRelayToClients) {
            double latestTimestamp = Utils.Library.NetworkTime;
            myNetObject.SENDCmdPluginRecvValue(UID, key, value, latestTimestamp, serverRelayToClients, QoS_ChannelID);
        }

        protected void sendMsgToClients(string key, AValue value) {
            double latestTimestamp = Utils.Library.NetworkTime;
            myNetObject.SENDRpcPluginRecvValue(UID, key, value, latestTimestamp, QoS_ChannelID);
        }
        
    }
}
