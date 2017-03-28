using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogColorChange : MonoBehaviour {

    public Color deepColor;
    public Color surfaceColor;
    public float deepY;
    public float surfaceY;

    public float deepStrength;
    public float surfaceStrength;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RenderSettings.fogColor = Color.Lerp(deepColor, surfaceColor, (transform.position.y - deepY) / (surfaceY - deepY));
        RenderSettings.fogDensity = Mathf.Lerp(deepStrength, surfaceStrength, (transform.position.y - deepY) / (surfaceY - deepY));
    }
}
