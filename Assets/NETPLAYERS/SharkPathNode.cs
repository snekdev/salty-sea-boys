using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkPathNode : MonoBehaviour {
    public List<SharkPathNode> ThisNodesOptions = new List<SharkPathNode>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        foreach (var item in ThisNodesOptions)
        {
            Debug.DrawLine(this.transform.position, item.transform.position, Color.red);
        }
#endif
    }

    void OnDrawGizmosSelected()
    {
        foreach (var item in ThisNodesOptions)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, item.transform.position);
        }
    }
}
