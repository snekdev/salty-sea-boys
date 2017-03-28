using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommanTest : NetworkBehaviour {

    public GameObject bulletPrefab;

    [Command]
    void CmdDoFire(float lifeTime)
    {
        GameObject bullet = (GameObject)Instantiate(
            bulletPrefab,
            transform.position + transform.right,
            Quaternion.identity);

        var bullet2D = bullet.GetComponent<Rigidbody2D>();
        bullet2D.velocity = transform.right * 0.1f;
        Destroy(bullet, lifeTime);

        NetworkServer.Spawn(bullet);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
