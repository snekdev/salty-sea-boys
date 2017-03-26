using UnityEngine;
using System.Collections;

using SoftRare.Serialization;
namespace SoftRare.Net.Plugin {
    public class PL_Chat : PL_Messenger {

        public override void __awake(GameObject owner) {

            StartCoroutine(saySomething("c1","hello"));
        }

        public IEnumerator saySomething(string channel, string text) {
            yield return new WaitForSeconds(2f);

            if (myNetObject.isLocalPlayer) {
                
                sendMsgToServer(channel, new StringValue(text), true);
            }
        }

        public override void __receiveValue(string channel, AValue value, double latestRemoteTimestampOfChange) {
            print("chat arrived: channel: " + channel + ", value: " + value);
        }
    }
}
