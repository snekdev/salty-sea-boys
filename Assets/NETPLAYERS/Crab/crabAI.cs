using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabAI : MonoBehaviour {
    Animator thisAnimator;
    Rigidbody thisRigid;

    Quaternion targetRotation;

    Vector3 Direction = Vector3.zero;

    float Timer = 0;
    float MaxTimer = 1;

    // Use this for initialization
    void Start () {
        thisAnimator = GetComponent<Animator>();
        thisRigid = GetComponent<Rigidbody>();

        Direction = new Vector3(Random.value, Random.value, Random.value);

        targetRotation = Quaternion.LookRotation(Direction.normalized);
    }
	
	// Update is called once per frame
	void Update () {

        Timer += Time.deltaTime;
        //thisAnimator.SetBool("IsMoving", true);
        if (Timer >= MaxTimer)
        {
            thisAnimator.SetBool("CrabState", true);
            Timer = 0;
            Direction = Random.onUnitSphere;
            Direction = new Vector3(Direction.x, 0, Direction.z);

            targetRotation = Quaternion.LookRotation(Direction.normalized);
            //targetRotation *= Quaternion.Euler(0, -90, 0);
            thisRigid.AddForce(Direction * 300);
        }
        thisRigid.AddForce(Vector3.down * 20);
    }
    void __uMMO_serverNPO_init()
    {

        //var main = particleSystemz.emission;
        ////Set the particle size.
        //var isemitting = main.enabled;
        //isemitting = false;
        //main.enabled = isemitting;
        //Camera.main.GetComponent<ThirdPersonCamera.CameraController>().target = this.transform;
    }
}
