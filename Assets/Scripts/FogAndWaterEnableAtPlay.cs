using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogAndWaterEnableAtPlay : MonoBehaviour {

    public GameObject water;

	// Use this for initialization
	void Start () {
        water.SetActive(true);
        RenderSettings.fog = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
