using UnityEngine;
using System.Collections;
using System;

using UnityEngine.Networking;

namespace SoftRare.Net.Plugin {
    public abstract class PL_Connector : PL_Main {

        protected NetworkManager manager;

        public bool showGUI = true;

        public bool connectAutomatically = false;

        public float connectAfterSeconds = 1f;

        void Awake() {
            manager = uMMO.get.networkManager;
        }

        public virtual IEnumerator connectDelayed() {
            yield return new WaitForSeconds(connectAfterSeconds);

            if (uMMO.get.architectureToCompile == CompilationArchitecture.Server) {
                StartServer();
            } else if (uMMO.get.architectureToCompile == CompilationArchitecture.Client) {
                StartClient();
            } else {
                showGUI = true;
            }
        }

        public virtual void StartServer() {
            manager.StartServer();
        }

        public virtual void StartClient() {
            manager.StartClient();
        }

        public virtual void CancelConnection() {
            manager.StopHost();
        }
    }
}
