using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public Animator ani;
    public Rigidbody rig;

    float Timer = 0;
    float MaxTimer = 1;
    bool isMoving = false;

    GameObject waterTransform;



    // Use this for initialization
    void Start () {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        waterTransform = GameObject.FindGameObjectsWithTag("Water")[0];

    }
    public float speed = 50;
    Quaternion targetRotation;
    // Update is called once per frame
    void Update () {
        Timer += Time.deltaTime;
        if (Input.GetMouseButton(1))
        {
            Timer = 0;
            isMoving = true;
            //ani.SetBool("IsMoving", true);
            Vector3 direction = Camera.main.transform.forward * 15;

            targetRotation = Quaternion.LookRotation(direction.normalized);
           // targetRotation *= Quaternion.Euler(90, 0, 0);
            rig.AddForce(direction);
        }
        if (Timer < MaxTimer)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Timer = 0;
        ////isMoving = true;
        ////ani.SetBool("IsMoving", true);
        //Vector3 direction = Camera.main.transform.forward;

        //targetRotation = Quaternion.LookRotation(direction);
        //targetRotation *= Quaternion.Euler(90, 0, 0);
        //rig.AddForce(direction);

      //  isMoving = true;

        ani.SetBool("isMoving", isMoving);
        
        rig.MoveRotation(transform.rotation = Quaternion.RotateTowards(rig.rotation, targetRotation, speed * Time.deltaTime));

        if (this.transform.position.y > waterTransform.transform.position.y)
        {
            rig.AddForce(Vector3.down * 20);
        }
    }

    void __uMMO_localPlayer_init()
    {
        Debug.Log("init here");

     
           GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (spawnPoints.Length > 0)
        {
          transform.position = spawnPoints[0].transform.position;
        }
        else
        {
          
        }

        Camera.main.GetComponent<ThirdPersonCamera.CameraController>().target = this.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.gameObject.tag == "Fish")
        {
            //Owner = other.transform;

            var main = other.GetComponent<ParticleSystem>().emission;
            //Set the particle size.
            var isemitting = main.enabled;
            isemitting = true;
            main.enabled = isemitting;
            Camera.main.GetComponent<ThirdPersonCamera.CameraController>().target = this.transform;
        }
    }
}
