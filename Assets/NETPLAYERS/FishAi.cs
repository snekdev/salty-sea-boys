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
    Transform falseFlag;

    GameObject waterTransform;

    public ParticleSystem particleSystemz;

    // Use this for initialization
    void Start () {
        thisAnimator = GetComponent<Animator>();
        thisRigid = GetComponent<Rigidbody>();

        Direction = new Vector3(Random.value, Random.value, Random.value);

        targetRotation = Quaternion.LookRotation(Direction.normalized);
        targetRotation *= Quaternion.Euler(0, -90, 0);
        thisRigid.AddForce(Direction);

        waterTransform = GameObject.FindGameObjectsWithTag("Water")[0];

        particleSystemz = GetComponent<ParticleSystem>();

    }
    float speed = 50;
    // Update is called once per frame
    void Update () {
        //ParticleSystem.EmissionModule em = particleSystem.emission;
        //em.enabled = false;

        //var main = particleSystemz.main;
        ////Set the particle size.
        //var startSize = main.startSize;
        //startSize = 0.0f;
        //main.startSize = startSize;



        if (Owner == null)
        {


            //particleSystem.transform.gameObject.SetActive(false);
            //particleSystem.time = 0;

            //particleSystem.emissionRate = 0;
            //particleSystem.emission.rateOverTime = 0;

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

            //particleSystem.emission.enabled = true;
            //ParticleSystem.EmissionModule em = particleSystem.emission;
            //em.enabled = true;
            //particleSystem.emission = em;

            //particleSystem.emission.enabled = true;

            //particleSystem.transform.gameObject.SetActive(true);

            float distance = Vector3.Distance(Owner.position, this.transform.position);
            if (distance < 50)
            {
                Direction = Owner.position - this.transform.position;

                targetRotation = Quaternion.LookRotation(Direction.normalized);
                targetRotation *= Quaternion.Euler(0, -90, 0);
                thisRigid.AddForce(Direction);


                //var main = particleSystemz.emission;
                ////Set the particle size.
                //var isemitting = main.enabled;
                //isemitting = true;
                //main.enabled = isemitting;
            }
            else
            {
                Owner = falseFlag;
            }

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

        var main = particleSystemz.emission;
        //Set the particle size.
        var isemitting = main.enabled;
        isemitting = false;
        main.enabled = isemitting;
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
