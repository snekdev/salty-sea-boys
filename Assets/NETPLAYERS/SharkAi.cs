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

    GameObject waterTransform;

    // Use this for initialization
    void Start () {
        thisRigid = GetComponent<Rigidbody>();
        //targetPosition.position = new Vector3(10, 0, 0);


        waterTransform = GameObject.FindGameObjectsWithTag("Water")[0];
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

            float distanceToTargetLastKnownPosition = Vector3.Distance(transform.position, targetPosition.position);
            if (distanceToTargetLastKnownPosition < 5)
            {
                targetRotation.eulerAngles = new Vector3(this.transform.eulerAngles.x + (Mathf.Sin(Time.deltaTime) * 50), this.transform.eulerAngles.y, this.transform.eulerAngles.z + (Mathf.Sin(Time.deltaTime) * 50));

                //thisRigid.AddForce(Direction * moveSpeed * 1.5f);
                thisRigid.MoveRotation(transform.rotation = Quaternion.RotateTowards(thisRigid.rotation, targetRotation, speed * Time.deltaTime));
            }
            else
            {
                //move to player or last know position
                //should probably raycast to look for them more accurately once identified
                Direction = targetPosition.position - transform.position;



                targetRotation = Quaternion.LookRotation(Direction.normalized);
                targetRotation *= Quaternion.Euler(0, -90, 0);

                thisRigid.AddForce(Direction * moveSpeed * 1.5f);
                thisRigid.MoveRotation(transform.rotation = Quaternion.RotateTowards(thisRigid.rotation, targetRotation, speed * Time.deltaTime));
            }



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


        if (this.transform.position.y > waterTransform.transform.position.y)
        {
            thisRigid.AddForce(Vector3.down * 20);
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
