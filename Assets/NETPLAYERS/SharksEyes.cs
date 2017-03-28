using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharksEyes : MonoBehaviour {

    // Use this for initialization
    public string PlayerName;
    public Vector3 LastSeenAt = Vector3.zero;
    public bool isVisible = false;
	void Start () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.gameObject.tag == "Player")
        {
            isVisible = true;
            LastSeenAt = other.transform.position;

            PlayerName = other.gameObject.name;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
