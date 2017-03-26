using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;
using SoftRare.Net;

public class Done_GameController : NetworkBehaviour
{
	public GameObject[] hazards;
    public List<NetObject> hazardsSpawned = new List<NetObject>();

    public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;

    //Not used yet in uMMO demo:
    /*private bool gameOver;
    private bool restart;
    private int score;*/

    public bool stop = false;

    void Start ()
	{
		/*gameOver = false;
		restart = false;
        score = 0;*/
        restartText.text = "";
		gameOverText.text = "";
		
		UpdateScore ();

        stop = false;
        if (uMMO.get.isServer) {
            StartCoroutine(SpawnWaves());
        }

    }
	
	void Update ()
	{
		/*if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}*/
	}

    [Server]
	public IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
            while (uMMO.get.connections2NetObjects.Count > 0) {
                for (int i = 0; i < hazardCount; i++) {

                    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    hazard = (GameObject)Instantiate(hazard, spawnPosition, spawnRotation);
                    NetworkServer.Spawn(hazard);
                    hazardsSpawned.Add(hazard.GetComponent<NetObject>());
                    hazard.transform.position = spawnPosition;
                    hazard.transform.rotation = spawnRotation;

                    yield return new WaitForSeconds(spawnWait);
                }
                yield return new WaitForSeconds(waveWait);

            }
            yield return null;
        }
	}
	
	public void AddScore (int newScoreValue)
	{
		//score += newScoreValue;
		//UpdateScore ();
	}
	
	void UpdateScore ()
	{
		//scoreText.text = "Score: " + score;
	}
	
	public void GameOver ()
	{
		//gameOverText.text = "Game Over!";
		//gameOver = true;
	}
}