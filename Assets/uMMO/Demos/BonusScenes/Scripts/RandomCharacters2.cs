using UnityEngine;
using System.Collections;

using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class RandomCharacters2 : SoftRare.Net.PlayerAction {

    private Vector3 destinationPosition = Vector3.zero;        // The destination Point
    private float destinationDistance;          // The distance between transform and destinationPosition
    public float Speed;                         // Speed at which the character moves
    public float animSpeed;
    public float Direction;                    // The Speed the character will move
    public Animator anim;                     // Animator to Anim converter

    void Start() {

        Physics.gravity = new Vector3(0, -200f, 0); // used In conjunction with RigidBody for better Gravity (Freeze Rotation X,Y,Z), set mass and use Gravity)
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        destinationPosition = transform.position;
    }

    void Update() {
        // keep track of the distance between this gameObject and destinationPosition 

            destinationDistance = Vector3.Distance(destinationPosition, transform.position);

            // Set's speed in reference to distance

            if (destinationDistance < .5f) {
                Speed = 0;
            } else if (destinationDistance > .5f) {
                Speed = 1;

                Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destinationPosition - transform.position), 3f);
                transform.rotation = targetRotation;
                //Below sends Floats to Mecanim, Raycast set's speed to X until destination is reached animation is played until speed drops
            }

            if (Speed > .5f) {
                anim.SetFloat("Speed", 0.2f);
            } else if (Speed < .5f) {
                anim.SetFloat("Speed", 0.0f);
            }
        

        float xd = destinationPosition.x;
        float zd = destinationPosition.z;
        float randomX = 0f;
        float randomZ = 0f;
        float rangeToWalk = 20;
        float rangeToStayCloseTo = 2;
        float directionBias = 2;

        if (xd < -rangeToStayCloseTo) {
            randomX = Random.Range(-xd/directionBias, rangeToWalk);
        } else if (xd >= -rangeToStayCloseTo && xd <= rangeToStayCloseTo) {
            randomX = Random.Range(-rangeToWalk, rangeToWalk);
        } else {
            randomX = Random.Range(-rangeToWalk, xd/directionBias);
        }
        if (zd < -rangeToStayCloseTo) {
            randomZ = Random.Range(-zd/directionBias, rangeToWalk);
        } else if (zd >= -rangeToStayCloseTo && zd <= rangeToStayCloseTo) {
            randomZ = Random.Range(-rangeToWalk, rangeToWalk);
        } else {
            randomZ = Random.Range(-rangeToWalk, zd/directionBias);
        }

        int rand = Random.Range(0, 150);
        if(rand == 20) {
            destinationPosition = transform.TransformDirection(new Vector3(xd + randomX, 0, zd + randomZ));
        }
        //Debug.DrawLine(transform.position, destinationPosition);

        // To prevent code from running if not needed
        if (destinationDistance > .5f) {
            transform.Translate(Vector3.forward * 0.015f);
        }

    }

    [Command]
    public void CmdSetDestination(Vector3 vec) {
        destinationPosition = vec;
    }
}
