using UnityEngine;
using System.Collections;

using UnityEngine.Networking;

public class Done_DestroyByContact : NetworkBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private Done_GameController gameController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter (Collider other)
	{

        if (/*(*/ SoftRare.Net.uMMO.get.isServer /*) || (hasAuthority)*/) {
            if (other.tag == "Boundary" || other.tag == "Enemy") {
                return;
            }

            if (explosion != null) {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            if (other.tag == "Player") {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
            }

            gameController.AddScore(scoreValue);
            if (other.gameObject.GetComponent<Done_DestroyByContact>() != null) {
                other.gameObject.GetComponent<Done_DestroyByContact>().CmdDestroy();
            }

            DestroyIfExists(other.gameObject);
            if (SoftRare.Net.uMMO.get.isClient)
                CmdDestroy();
            DestroyIfExists(this.gameObject);

        }
	}

    void DestroyIfExists(GameObject go) {
        if (go != null) {
            Destroy(go);

            if (SoftRare.Net.uMMO.get.isServer && go.GetComponent<NetworkIdentity>() != null) {
                NetworkServer.Destroy(go);
            }
        }
    }

    [Command]
    void CmdDestroy() {
        //NetworkServer.Destroy(this.gameObject);
        DestroyIfExists(this.gameObject);
    }
}