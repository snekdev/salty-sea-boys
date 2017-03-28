using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SoftRare.Net.Plugin;

public class PlayerWaistManager : NetworkBehaviour
{
    public TextMesh myMesh;
    public PL_NetworkManager_Default NetMngr;
    public GameObject myPrefab;
    int connectedPlayers = 0;

    public const float maxHealth = 100;
    [SyncVar]
    public float currentHealth = maxHealth;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isServer)
        {
            ServerStuff();
            
        }
        myMesh.text = currentHealth.ToString();
    }
    List<PlayerController> myPlayer = new List<PlayerController>();
    void ServerStuff()
    {
        if (NetMngr.numPlayers != connectedPlayers)
        {
            if (NetMngr.numPlayers > connectedPlayers)
            {
                GameObject tempHolder = Instantiate(myPrefab, Vector3.zero,Quaternion.identity) as GameObject;
                
                NetworkServer.Spawn(tempHolder);
            }
            else
            {

            }
            connectedPlayers = NetMngr.numPlayers;
        }

        currentHealth -= Time.deltaTime * 0.01f;

        GameObject[] myBlendShapes = GameObject.FindGameObjectsWithTag("BlendShapeTag");
        foreach (var item in myBlendShapes)
        {
            item.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, currentHealth);
        }


    }

    public void TIMETOPOOP()
    {
        GameObject tempHolder = Instantiate(myPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        NetworkServer.Spawn(tempHolder);
    }
}
