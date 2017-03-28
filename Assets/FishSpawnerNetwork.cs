using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FishSpawnerNetwork : NetworkBehaviour {
    public GameObject fishObject;
	// Use this for initialization
	void Start () {
		
	}
    float timer = 0;
    float maxTimer = 3;
    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        //if (timer >= maxTimer)
        //{
            //timer = 0;
            //DoFire();
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    CmdDoFire();
        //}
    }
    public void DoFire()
    {
        Debug.Log("TEST");
        GameObject tempHolder = Instantiate(fishObject, Vector3.zero, Quaternion.identity) as GameObject;


        NetworkServer.Spawn(tempHolder);
    }

    [Command]
    void CmdDoFire()
    {
        Debug.Log("TEST");
        GameObject tempHolder = Instantiate(fishObject, Vector3.zero,Quaternion.identity) as GameObject;


        NetworkServer.Spawn(tempHolder);
    }
}
