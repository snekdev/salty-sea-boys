using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;

namespace SoftRare.Net.Plugin {
    public class PL_NetworkManager_Default : PL_NetworkManager {

        protected void OnValidate() {
            if (!Application.isPlaying) {
                if (uMMO.get != null) {
                    uMMO.get.dontDestroyOnLoad = dontDestroyOnLoad;
                    uMMO.get.dynPreprocessorDefines = !scriptCRCCheck;

                }
            }
        }
        

        public override void OnStartServer() {
            base.OnStartServer();

            uMMO.get.architectureToCompile = CompilationArchitecture.Server;

            StartCoroutine(spawnObjects());
        }

        public IEnumerator spawnObjects() {
            yield return new WaitForSeconds(1f);

            //spawn NPCs and NPOs already existing in the scene:
            NetworkServer.SpawnObjects();
        }

        public override void OnStartClient(NetworkClient client) {
            base.OnStartClient(client);

            uMMO.get.architectureToCompile = CompilationArchitecture.Client;

        }

        public override void OnServerConnect(NetworkConnection conn) {
            base.OnServerConnect(conn);

            uMMO.get.connections2NetObjects.Add(conn,null);

        }

        public override void OnServerDisconnect(NetworkConnection conn) {
            base.OnServerDisconnect(conn);

            uMMO.get.connections2NetObjects.Remove(conn);

        }


    }
}
