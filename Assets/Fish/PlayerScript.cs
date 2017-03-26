using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public Animator ani;
    public Rigidbody rig;

    // Use this for initialization
    void Start () {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ani.SetInteger("SwimState", 1);
            rig.AddForce(Vector3.forward * 100);

        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    ani.SetInteger("SwimState", 2);
        //    rig.AddForce(Vector3.forward * 100);
        //}

    }

    void __uMMO_localPlayer_init()
    {
        Debug.Log("init here");
        Camera.main.GetComponent<ThirdPersonCamera.CameraController>().target = this.transform;
    }
}
