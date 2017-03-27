using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAi : MonoBehaviour {
    //List<List<string>> myList = new List<List<string>>();
    public SharkPathFinding myPathFinding;
    Rigidbody thisRigid;

    bool isHunting = false;

    public Transform targetPosition;
    public SharksEyes sharkEyes;

    float timeSinceLastSeen = 0;

    Quaternion targetRotation;
    Vector3 Direction = Vector3.zero;
    float speed = 50;

    // Use this for initialization
    void Start () {
        thisRigid = GetComponent<Rigidbody>();
        //targetPosition.position = new Vector3(10, 0, 0);


    }
    float moveSpeed = 0.1f;
	// Update is called once per frame
	void Update () {

        if (sharkEyes.isVisible == true)
        {
            timeSinceLastSeen = 30;
        }
        if (timeSinceLastSeen > 0)
        {
            timeSinceLastSeen -= Time.deltaTime;
            isHunting = true;
        }
        else
        {
            timeSinceLastSeen = 0;
            isHunting = false;
        }

        if (isHunting == true)
        {
            targetPosition.position = sharkEyes.LastSeenAt;

            Direction = targetPosition.position - transform.position;

            targetRotation = Quaternion.LookRotation(Direction.normalized);
            targetRotation *= Quaternion.Euler(0, -90, 0);

            thisRigid.AddForce(Direction * moveSpeed * 1.5f);
            thisRigid.MoveRotation(transform.rotation = Quaternion.RotateTowards(thisRigid.rotation, targetRotation, speed * Time.deltaTime));
        }
        else
        {
            Direction = targetPosition.position - transform.position;

            targetRotation = Quaternion.LookRotation(Direction.normalized);
            targetRotation *= Quaternion.Euler(0, -90, 0);

            //PathToNextNode();

            float distanceToTargetNode = Vector3.Distance(transform.position, targetPosition.position);
            if (distanceToTargetNode < 5)
            {
                PathToNextNode();
            }
            else
            {
                thisRigid.AddForce(Direction * moveSpeed);
                thisRigid.MoveRotation(transform.rotation = Quaternion.RotateTowards(thisRigid.rotation, targetRotation, speed * Time.deltaTime));
            }
        }
       


	}
    

    void PathToNextNode()
    {
        //myPathFinding.FindNewOptimalPath(transform.position);
        targetPosition.position = myPathFinding.FindNewOptimalPath(transform.position).position;
    }
    void __uMMO_serverNPO_init()
    {
        //thisAnimator.SetBool("IsMoving", true);
        Debug.Log("init here");
        Camera.main.GetComponent<ThirdPersonCamera.CameraController>().target = this.transform;

        PathToNextNode();
    }
}
