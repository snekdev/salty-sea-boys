using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DidPlayerPickUp : NetworkBehaviour {
    public float gestation =25;
    float respawnTimer = 0;
    float respawnCap = 5;
    public GameObject parent;

    public Rigidbody rig;
    public float gravity = 0.3f;

    Vector3 initialPosition;
    private void Start()
    {
        initialPosition = transform.position;
      //  rig = GetComponent<Rigidbody>();
    }

    [SyncVar]
    public bool isCollectableThere;
    private void Update()
    {
        //Debug.Log("I REACHEDIT");
        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnCap)
        {
            isCollectableThere = true;
            //Collectables.SetActive(true);
        }
        //Collectables.SetActive(isCollectableThere);

       // parent.transform.position -= Vector3.down * gravity * Time.deltaTime;
       rig.AddForce(Vector3.down * gravity);
        
       // else
        {
       //     transform.position = new Vector3(Random.RandomRange(100f, 900f), 150, Random.RandomRange(100f, 900f)); //new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.gameObject.tag == "Player")
        {
            //Owner = other.transform;
            //if (Collectables.activeSelf == true)
            //{

           // Debug.Log("HITTTTT");
            other.gameObject.GetComponent<PlayerScript>().gestation += gestation;
            //GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishSpawnerNetwork>().ProduceChild = true;
            //Cmdspawnfish();
            //Collectables.SetActive(false);
            isCollectableThere = false;
            respawnTimer = 0;
            parent.transform.position = new Vector3(Random.RandomRange(350f, 700f), Random.RandomRange(-20f, 100f), Random.RandomRange(350f, 700f));// new Vector3(initialPosition.x, initialPosition.y + 50, initialPosition.z);
                                                                                                                         //}

        }
    }
    //[Command]
    //void Cmdspawnfish()
    //{
    //    //other.GetComponent<PlayerScript>().gestation += 100;
    //    //GameObject tempHolder = Instantiate(Collectables, Vector3.zero, Quaternion.identity) as GameObject;
    //    Debug.Log("I REACHEDIT");

    //    //NetworkServer.Spawn(tempHolder);
    //}
}
