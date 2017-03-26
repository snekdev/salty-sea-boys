using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawner: NetworkBehaviour {

    public GameObject prefabA;
    public GameObject prefabB;
    static int count = 0;
    static float lastTime = 0;
    public int showCount = 0;


	void OnGUI()
	{

		//GUILayout.Label("Spawns up to 200 characters");
		//GUILayout.Label("Press fire button to spawn Teddy!");		
	}

    void Start()
    {
        lastTime = Time.time;
    }

    void Update()
    {
        if(count < 50 && SoftRare.Net.uMMO.get.isServer)
        {
            bool alt = Random.Range(1f, 100f) > 80f ? true : false;
            //bool alt = false;

            if (Time.time - lastTime > 0.1f)
            {
                GameObject newNPC = null;
                if (prefabA != null && !alt) {
                    newNPC = (GameObject)Instantiate(prefabA, new Vector3(0,0,0), Quaternion.Euler(0, 0, 0));
                }

                if (prefabB != null && alt) {
                    newNPC = (GameObject)Instantiate(prefabB, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                }
                NetworkServer.Spawn(newNPC);
                lastTime = Time.time;
                count++;
                showCount = count;
            }
        }
    }
}
