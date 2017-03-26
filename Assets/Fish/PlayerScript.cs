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
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 direction = Camera.main.transform.forward * 15;

            //rig.MoveRotation(Quaternion.LookRotation(direction));
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //Quaternion.RotateTowards()

            rig.MoveRotation(targetRotation);
            //rig.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, 15 * Time.deltaTime));
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 0.1f);
            //ani.SetInteger("SwimState", 1);
            rig.AddForce(direction);

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
