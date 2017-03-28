using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DidPlayerPickUp : NetworkBehaviour {

    float respawnTimer = 0;
    float respawnCap = 5;
    public GameObject Collectables;

    [SyncVar]
    public bool isCollectableThere;
    private void Update()
    {
        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnCap)
        {
            isCollectableThere = true;
            //Collectables.SetActive(true);
        }
        Collectables.SetActive(isCollectableThere);
    }
    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.gameObject.tag == "Player")
        {
            //Owner = other.transform;
            if (Collectables.activeSelf == true)
            {
                other.gameObject.GetComponent<PlayerScript>().gestation += 5;
                //Collectables.SetActive(false);
                isCollectableThere = false;
                respawnTimer = 0;
            }
            
        }
    }
}
