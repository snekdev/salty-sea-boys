using UnityEngine;
using System.Collections;

namespace SoftRare.Net.Plugin {
    public class PL_SimplyLerpTransform : PL_AuthoritySerializer {

        public float positionLerpTimeFraction = 0.95f;
        public float rotationLerpTimeFraction = 0.95f;

        public override void OnNonAuthorityDeserialize(double latestRemoteTimestampOfChange) {
            if (myNetObject.pluginShouldSyncPosition) {
                Vector3 pos = Vector3.zero;
                Sync("p", ref pos);

                myNetObject.transform.position = Vector3.Lerp(transform.position, pos, positionLerpTimeFraction);
            }
            if (myNetObject.pluginShouldSyncRotation) {
                Quaternion rot = Quaternion.identity;
                Sync("r", ref rot);
                myNetObject.transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationLerpTimeFraction);
            }

        }

        public override void OnAuthoritySerialize() {

            if (myNetObject.pluginShouldSyncPosition) {
                Vector3 pos = myNetObject.transform.position;
                Sync("p", ref pos);
            }
            if (myNetObject.pluginShouldSyncRotation) {
                Quaternion rot = myNetObject.transform.rotation;
                Sync("r", ref rot);
            }
        }

    }
}
