using GPUTools.Hair.Scripts.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHairGravity : MonoBehaviour {
    HairSettings myHair;
	// Use this for initialization
	void Start () {
        myHair = GetComponent<HairSettings>();
    

    }
	
	// Update is called once per frame
	void Update () {
        myHair.PhysicsSettings.Gravity = Vector3.zero;

    }
}
