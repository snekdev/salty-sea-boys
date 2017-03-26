using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour {

    public List<GameObject> prefabs;
    public List<Material> materials;
    public float distanceBetween = 1;

	// Use this for initialization
	void Start () {

        if (prefabs.Count > 0 && materials.Count > 0)
            for (int i = 0; i < prefabs.Count; ++i)
            {
                for (int j = 0; j < materials.Count; ++j)
                {
                    GameObject g = Instantiate(prefabs[i]);
                    g.transform.position = new Vector3(i * distanceBetween, 0, j * distanceBetween);
                    g.GetComponent<MeshRenderer>().material= materials[j];

                }
            }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
