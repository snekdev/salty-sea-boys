using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class testSpawnOnTouch : NetworkBehaviour
{
    GameObject FishSpawner;
    GameObject ParentPlayer;
    PlayerScript myPlayerScript;

    // Use this for initialization
    void Start () {
        FishSpawner = GameObject.FindGameObjectWithTag("FishManager");
        ParentPlayer = transform.parent.gameObject;
        myPlayerScript = ParentPlayer.GetComponent<PlayerScript>();
            }
	
	// Update is called once per frame
	void Update () {
        if (myPlayerScript.gestation >= 5)
        {
            //myPlayerScript.gestation = 0;
            FishSpawner.GetComponent<FishSpawnerNetwork>().ProduceChild = true;
            FishSpawner.GetComponent<FishSpawnerNetwork>().VaginaLocation = this.transform.position;

        }
    }
    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);

    }
}
