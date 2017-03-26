using UnityEngine;
using System.Collections;

using UnityEngine.Networking;

public class Done_WeaponController : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float delay;

	void Start ()
	{
		InvokeRepeating ("Fire", delay, fireRate);
	}

	void Fire ()
	{
		GameObject go = (GameObject)Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        NetworkServer.Spawn(go);

        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().Play();
	}
}
