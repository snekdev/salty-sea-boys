using UnityEngine;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]
public class BotControlScript : MonoBehaviour
{

	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
	public bool useCurves;						// a setting for teaching purposes to show use of curves

	
	private Animator anim;							// a reference to the animator on the character
	

	void Start ()
	{
		// initialising reference variables
		anim = GetComponent<Animator>();					  

	}
	
	
	void FixedUpdate ()
	{
		float h = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
		float v = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
		anim.SetFloat("Speed", v);							// set our animator's float parameter 'Speed' equal to the vertical input axis				
		anim.SetFloat("Direction", h); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
		anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'

		
	}
}
