using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FishSpawnerNetwork : NetworkBehaviour {
    public GameObject babyObject;

    [SyncVar]
    public bool ProduceChild;
    [SyncVar]
    public Vector3 VaginaLocation;

    bool isServer = false;
    // Use this for initialization
    public override void OnStartServer()
    {
        // create and spawn
        isServer = true;
    }


float timer = 0;
    float maxTimer = 3;
    // Update is called once per frame
    void Update()
    {
        //if (!Network.isServer)
        //return;
        if (isServer == false)
            return;

        //PooOutBaby(VaginaLocation);
        if (ProduceChild == true)
        {
            PooOutBaby(VaginaLocation);
        }
    }
    void PooOutBaby(Vector3 Vagina)
    {
        GameObject babyHolder = Instantiate(babyObject, Vector3.zero, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(babyHolder);

        ProduceChild = false;
    }

    //[Command]
    //void CmdDoFire()
    //{
    //    Debug.Log("TEST");
    //    GameObject tempHolder = Instantiate(babyObject, Vector3.zero,Quaternion.identity) as GameObject;


    //    NetworkServer.Spawn(tempHolder);
    //}
}
