using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAi : MonoBehaviour {
    Animator thisAnimator;
    Rigidbody thisRigid;
    float Timer = 0;
    float MaxTimer = 1;
    Vector3 Direction = Vector3.zero;
    float Speed = 250;
    bool isMoving = false;
    Quaternion targetRotation;
    // Use this for initialization
    void Start () {
        thisAnimator = GetComponent<Animator>();
        thisRigid = GetComponent<Rigidbody>();

        Direction = new Vector3(Random.value, Random.value, Random.value);

        targetRotation = Quaternion.LookRotation(Direction.normalized);
        targetRotation *= Quaternion.Euler(0, -90, 0);
        thisRigid.AddForce(Direction);

    }
    float speed = 50;
    // Update is called once per frame
    void Update () {
        Timer += Time.deltaTime;
        //thisAnimator.SetBool("IsMoving", true);
        if (Timer >= MaxTimer)
        {
            Timer = 0;


            isMoving = System.Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
            //thisAnimator.SetBool("IsMoving", isMoving);
            thisAnimator.SetBool("IsMoving", isMoving);
            if (isMoving == true)
            {
                //Direction = new Vector3(Random.value * 2 - Random.value, Random.value * 2 - Random.value, Random.value * 2 - Random.value);
                Direction = Random.onUnitSphere;

                targetRotation = Quaternion.LookRotation(Direction.normalized);
                targetRotation *= Quaternion.Euler(0, -90, 0);
                thisRigid.AddForce(Direction);
            }
        }
        if (isMoving == true)
        {
            thisRigid.AddForce(Direction);
        }
            thisRigid.MoveRotation(transform.rotation = Quaternion.RotateTowards(thisRigid.rotation, targetRotation, speed * Time.deltaTime));

    }
    void __uMMO_serverNPO_init()
    {
        //thisAnimator.SetBool("IsMoving", true);
        Debug.Log("init here");
        Camera.main.GetComponent<ThirdPersonCamera.CameraController>().target = this.transform;
    }
}
