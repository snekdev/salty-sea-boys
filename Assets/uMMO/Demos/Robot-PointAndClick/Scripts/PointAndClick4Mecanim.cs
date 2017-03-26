using UnityEngine;
using System.Collections;

using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class PointAndClick4Mecanim : SoftRare.Net.PlayerAction {

    private Vector3 destinationPosition = Vector3.zero;        // The destination Point
    private float destinationDistance;          // The distance between transform and destinationPosition
    public float Speed;                         // Speed at which the character moves
    public float animSpeed;
    public float Direction;                    // The Speed the character will move
    public Animator anim;                     // Animator to Anim converter

    void Start()
    {

        Physics.gravity = new Vector3(0, -200f, 0); // used In conjunction with RigidBody for better Gravity (Freeze Rotation X,Y,Z), set mass and use Gravity)
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        destinationPosition = transform.position;
    }

    void Update()
    {
        // keep track of the distance between this gameObject and destinationPosition 
        if (hasAuthority) {
            destinationDistance = Vector3.Distance(destinationPosition, transform.position);

            // Set's speed in reference to distance

            if (destinationDistance < .5f) {
                Speed = 0;
            } else if (destinationDistance > .5f) {
                Speed = 2;

                Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destinationPosition - transform.position), 3f);
                transform.rotation = targetRotation;
                //Below sends Floats to Mecanim, Raycast set's speed to X until destination is reached animation is played until speed drops
            }

            if (Speed > .5f) {
                anim.SetFloat("Speed", 0.3f);
            } else if (Speed < .5f) {
                anim.SetFloat("Speed", 0.0f);
            }
        }


        // Moves the Player if the Left Mouse Button was clicked
        if (UnityEngine.Input.GetMouseButtonDown(0)) {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist)) {
                if (isLocalPlayer) {
                    Vector3 target = ray.GetPoint(hitdist);
                    if (!hasAuthority) {
                        CmdSetDestination(target);
                    } else {
                        destinationPosition = target;
                    }
                }
            }
        }

        // To prevent code from running if not needed
        if (destinationDistance > .5f) {
            transform.Translate(Vector3.forward * 0.05f);
        }
        
    }

    [Command]
    public void CmdSetDestination(Vector3 vec) {
        destinationPosition = vec;
    }
}
