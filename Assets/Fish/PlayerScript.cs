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
    float speed = 150;
    Quaternion targetRotation;
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 direction = Camera.main.transform.forward * 15;

            targetRotation = Quaternion.LookRotation(direction.normalized);
            rig.AddForce(direction);
        }

        rig.MoveRotation(transform.rotation = Quaternion.RotateTowards(rig.rotation, targetRotation, speed * Time.deltaTime));
    }

    void __uMMO_localPlayer_init()
    {
        Debug.Log("init here");
        Camera.main.GetComponent<ThirdPersonCamera.CameraController>().target = this.transform;
    }
}
