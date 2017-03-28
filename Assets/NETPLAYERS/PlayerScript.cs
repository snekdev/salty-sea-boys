using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public Animator ani;
    public Rigidbody rig;

    float Timer = 0;
    float MaxTimer = 1;
    bool isMoving = false;

    public SkinnedMeshRenderer sk;

    GameObject waterTransform;

    public int PLAYERHEALTH;
    public float gravity = 20;
    public float moveForce = 15;

    GameObject myTextMesh;


    // Use this for initialization
    void Start () {
        PLAYERHEALTH = 1000;
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        if (GameObject.FindGameObjectsWithTag("Water").Length > 0)
            waterTransform = GameObject.FindGameObjectsWithTag("Water")[0];
        if (GameObject.FindGameObjectsWithTag("HUDText").Length > 0)
            myTextMesh = GameObject.FindGameObjectsWithTag("HUDText")[0];

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
            Vector3 direction = Camera.main.transform.forward;

            targetRotation = Quaternion.LookRotation(direction);
            if (waterTransform == null ||(  waterTransform != null && transform.position.y < waterTransform.transform.position.y))
            {
                rig.AddForce(direction * moveForce);
            }
            
           
        }
        if (Timer < MaxTimer)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        ani.SetBool("isMoving", isMoving);

        if (isMoving)
            rig.MoveRotation(Quaternion.RotateTowards(rig.rotation, targetRotation, speed * Time.deltaTime));
        else
            rig.MoveRotation(Quaternion.RotateTowards(rig.rotation, Quaternion.Euler(new Vector3(-67,0,0)), .3f * speed * Time.deltaTime));


        if (waterTransform != null && transform.position.y > waterTransform.transform.position.y)
        {
            rig.AddForce(Vector3.down * gravity);
        }

        if (myTextMesh != null)
            myTextMesh.GetComponent<TextMesh>().text = PLAYERHEALTH.ToString();

        if (sk != null)
        {
            sk.SetBlendShapeWeight(0, 50 + 50 * Mathf.Sin(Time.time));
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

        Camera.main.GetComponent<ThirdPersonCamera.CameraController>().Target = this.transform;
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
            Camera.main.GetComponent<ThirdPersonCamera.CameraController>().Target = this.transform;
        }
    }
}
