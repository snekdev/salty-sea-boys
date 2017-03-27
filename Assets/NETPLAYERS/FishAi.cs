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

    Transform Owner;

    GameObject waterTransform;

    ParticleSystem particleSystem;

    // Use this for initialization
    void Start () {
        thisAnimator = GetComponent<Animator>();
        thisRigid = GetComponent<Rigidbody>();

        Direction = new Vector3(Random.value, Random.value, Random.value);

        targetRotation = Quaternion.LookRotation(Direction.normalized);
        targetRotation *= Quaternion.Euler(0, -90, 0);
        thisRigid.AddForce(Direction);

        waterTransform = GameObject.FindGameObjectsWithTag("Water")[0];

        particleSystem = GetComponent<ParticleSystem>();

    }
    float speed = 50;
    // Update is called once per frame
    void Update () {

        if (Owner == null)
        {
            particleSystem.Stop();
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
        else
        {
            particleSystem.Play();
            Direction = Owner.position - this.transform.position;

            targetRotation = Quaternion.LookRotation(Direction.normalized);
            targetRotation *= Quaternion.Euler(0, -90, 0);
            thisRigid.AddForce(Direction);
        }

        if (this.transform.position.y > waterTransform.transform.position.y)
        {
            thisRigid.AddForce(Vector3.down * 20);
        }
    }
    void __uMMO_serverNPO_init()
    {
        Owner = null;
        //thisAnimator.SetBool("IsMoving", true);
        Debug.Log("init here");
        //Camera.main.GetComponent<ThirdPersonCamera.CameraController>().target = this.transform;
    }
    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.gameObject.tag == "Player")
        {
            Owner = other.transform;
        }
    }
}
