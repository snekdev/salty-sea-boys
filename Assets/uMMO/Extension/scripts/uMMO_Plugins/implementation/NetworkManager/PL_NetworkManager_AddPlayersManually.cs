using UnityEngine;
using System.Collections;
using SoftRare.Net;
using UnityEngine.Networking;

public class PL_NetworkManager_AddPlayersManually : SoftRare.Net.Plugin.PL_NetworkManager {
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

    public override void OnServerReady(NetworkConnection conn) {
        base.OnServerReady(conn);

        NetworkServer.SetClientReady(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnClientSceneChanged(NetworkConnection conn) {
        base.OnClientSceneChanged(conn);

        //ClientScene.Ready(conn);
        ClientScene.AddPlayer(conn, 0);
    }

    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);

    }

    public override void OnServerConnect(NetworkConnection conn) {
        base.OnServerConnect(conn);

        uMMO.get.connections2NetObjects.Add(conn, null);

    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);

        uMMO.get.connections2NetObjects.Remove(conn);

    }
}
