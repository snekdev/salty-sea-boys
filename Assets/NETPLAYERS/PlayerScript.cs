using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{

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

    GameObject PlayerManager;

    public GameObject prefabFish;

    GameObject FishSpawner;

    [SyncVar]
    public float gestation = 0;

    const float MaxHealth = 100;
    [SyncVar]
    public float currentHealth = MaxHealth;

    class SpawnFishyMessage : MessageBase
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    public const short RegisterHotsMsgId = 888;
    // Use this for initialization
    //void Start () {


    //}
    public float speed = 50;
    Quaternion targetRotation;
   // public float pregoTimer = 0;
    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
            return;
        //if (!isLocalPlayer)
        //if(!GetComponent<NetworkIdentity>().isLocalPlayer)
        //    return;
        Timer += Time.deltaTime;
        //pregoTimer += Time.deltaTime * 10;
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
            rig.MoveRotation(Quaternion.RotateTowards(rig.rotation, Quaternion.Euler(new Vector3(-67, rig.rotation.eulerAngles.y, rig.rotation.eulerAngles.z)), .3f * speed * Time.deltaTime));


        if (waterTransform != null && transform.position.y > waterTransform.transform.position.y)
        {
            rig.AddForce(Vector3.down * gravity);
        }

        if (myTextMesh != null)
            myTextMesh.GetComponent<TextMesh>().text = PLAYERHEALTH.ToString();

        /// if (sk != null)
        //{

        //sk.SetBlendShapeWeight(0, 0);// 100f* ((Time.time % 15f) / 15));
        //}
        //ani.SetLayerWeight(0, 100);
        //ani.SetFloat("Bloat", pregoTimer);
        //sk.SetBlendShapeWeight(0, ani.GetFloat("Bloat"));
        sk.SetBlendShapeWeight(0, gestation);

        if (gestation > 99)
        {
            PlayerWaistManager wasitHolder = PlayerManager.GetComponent<PlayerWaistManager>();
            wasitHolder.TIMETOPOOP(transform.position);
            gestation = 0;
        }

        //if (gestation >= 5)
        //{
        //    FishSpawner.GetComponent<FishSpawnerNetwork>().ProduceChild = true;
        //    FishSpawner.GetComponent<FishSpawnerNetwork>().VaginaLocation = this.transform.position;
        //    gestation = 0;
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //CmdDoFire(this.transform.position, this.transform.rotation);
        //}
    }

    void __uMMO_localPlayer_init()
    {
        PLAYERHEALTH = 1000;
        ani = GetComponent<Animator>();

        rig = GetComponent<Rigidbody>();
        PlayerManager = GameObject.FindGameObjectWithTag("PlayerManager");

        if (GameObject.FindGameObjectsWithTag("Water").Length > 0)
            waterTransform = GameObject.FindGameObjectsWithTag("Water")[0];
        if (GameObject.FindGameObjectsWithTag("HUDText").Length > 0)
            myTextMesh = GameObject.FindGameObjectsWithTag("HUDText")[0];

        Debug.Log("init here");

        FishSpawner = GameObject.FindGameObjectWithTag("FishManager");
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
    [Command]
    void CmdDoFire(Vector3 positionz, Quaternion rotationz)
    {
        //GameObject tempHolder = Instantiate(Resources.Load("FISH", typeof(GameObject)), positionz, rotationz) as GameObject;

        //NetworkServer.Spawn(tempHolder);

        //GameObject tempHolder = Instantiate(Resources.Load("Fishy2", typeof(GameObject)), positionz + new Vector3(0, -0.2f, 0), rotationz) as GameObject;
        Debug.Log("TEST");
        
        //GameObject tempHolder = Instantiate(prefabFish, positionz + new Vector3(0, -0.2f, 0), rotationz) as GameObject;


        //NetworkServer.Spawn(tempHolder);
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
