using UnityEngine;

using UnityEngine.Networking;

using UnityEngine.SceneManagement;

namespace SoftRare.Net.Plugin {
    //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class PL_Connector_SimpleDemo : PL_Connector {


        [SerializeField]
        public int offsetX;
        [SerializeField]
        public int offsetY;


        void Update() {
            if (!showGUI)
                return;

            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.S)) {
                    StartServer();

                }
                /*if (Input.GetKeyDown(KeyCode.H)) {
                    manager.StartHost();
                }*/
                if (UnityEngine.Input.GetKeyDown(KeyCode.C)) {
                    StartClient();
                }
            }
            if (NetworkServer.active && NetworkClient.active) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.X)) {
                    CancelConnection();
                }
            }
        }

        protected void OnGUI() {
            if (!showGUI)
                return;

            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            int spacing = 24;

            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null) {
                /*if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
                {
                    manager.StartHost();
                }
                ypos += spacing;*/

                if (GUI.Button(new Rect(xpos, ypos, 105, 20), "Client (C)")) {
                    StartClient();
                }
                manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Dedicated Server (S)")) {
                    StartServer();
                    SceneManager.LoadScene(0);
                }
                ypos += spacing;
            } else {
                if (NetworkServer.active) {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
                    ypos += spacing;
                }
                if (NetworkClient.active) {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }
            }

            if (NetworkClient.active && !ClientScene.ready) {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready")) {
                    ClientScene.Ready(manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0) {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (NetworkServer.active || NetworkClient.active) {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)")) {
                    CancelConnection();
                }
                ypos += spacing;
            }

        }
    }
}
