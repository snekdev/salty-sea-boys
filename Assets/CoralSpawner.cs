using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralSpawner : MonoBehaviour {


//    public GameObject coralPrefab;
    public Material coralMaterial;
    public GameObject temp;

    public List<GameObject> coralPrefabs;
    public int numberPerSpecies;
	// Use this for initialization
	void Start () {
     //   AddCoral();

    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            AddCoral();
        }
	}
    

    public void AddCoral()
    {
        foreach(GameObject coralPrefab in coralPrefabs)
        {

            if (temp == null)
                temp = new GameObject("temp");

            Mesh currentMesh = transform.GetComponent<MeshFilter>().mesh;
            Mesh coralMesh = coralPrefab.GetComponent<MeshFilter>().sharedMesh;
            Material material = coralPrefab.GetComponent<MeshRenderer>().sharedMaterial;

            GameObject coralHolder = new GameObject("coral " + coralPrefab.name);
            coralHolder.AddComponent<MeshFilter>();
            coralHolder.AddComponent<MeshRenderer>();
            coralHolder.GetComponent<MeshRenderer>().material = material;

            CombineInstance[] combine = new CombineInstance[numberPerSpecies];

            for (int i = 0; i < numberPerSpecies; ++i)
            {
                {
                    int index = Random.Range(0, currentMesh.triangles.Length / 3);

                    int vertex1 = currentMesh.triangles[index];
                    int vertex2 = currentMesh.triangles[index + 1];
                    int vertex3 = currentMesh.triangles[index + 2];

                    Vector3 p1 = currentMesh.vertices[vertex1];
                    Vector3 p2 = currentMesh.vertices[vertex2];
                    Vector3 p3 = currentMesh.vertices[vertex3];

                    float r1 = Random.Range(0, 1f);
                    float r2 = Random.Range(0, 1f);
                    //float tmp = Mathf.Sqrt(u);
                    //float x = 1 - tmp;
                    //float y = v * tmp;


                    Vector3 point = (1 - Mathf.Sqrt(r1)) * p1 + (Mathf.Sqrt(r1) * (1 - r2)) * p2 + (Mathf.Sqrt(r1) * r2) * p3;
                    temp.transform.position = transform.TransformPoint(point);
                }
                temp.transform.rotation = Quaternion.Euler(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
                combine[i].mesh = coralMesh;
                combine[i].transform = temp.transform.localToWorldMatrix;
            }

            coralHolder.GetComponent<MeshFilter>().mesh = new Mesh();
            coralHolder.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        }
    }
}
