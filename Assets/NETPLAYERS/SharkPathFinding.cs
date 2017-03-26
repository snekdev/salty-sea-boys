using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkPathFinding : MonoBehaviour {
    List<List<SharkPathNode>> myListOfPossiblePaths = new List<List<SharkPathNode>>();
    List<SharkPathNode> myBestPath = new List<SharkPathNode>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public Transform FindNewOptimalPath(Vector3 startLocation, Vector3 endLocation)
    {

        return findStartingNode(startLocation);
    }
    Transform findStartingNode(Vector3 startLocation)
    {
        //SharkPathNode closestChild = null;
        //float lowestDistance = float.MaxValue;
        //foreach (Transform child in transform)
        //{
        //    float distance = Mathf.Abs(Vector3.Distance(startLocation, child.position));

        //    if (distance < lowestDistance)
        //    {
        //        closestChild = child.GetComponent<SharkPathNode>();
        //    }
        //    // do whatever you want with child transform object here
        //}
        //return closestChild;




        //SharkPathNode tMin = null;
        //float minDist = Mathf.Infinity;
        //Vector3 currentPos = transform.position;
        //foreach (Transform t in transform)
        //{
        //    float dist = Vector3.Distance(t.position, currentPos);
        //    if (dist < minDist)
        //    {
        //        tMin = t.GetComponent< SharkPathNode>();
        //        minDist = dist;
        //    }
        //}
        //return tMin;


        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = startLocation;
        foreach (Transform potentialTarget in transform)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
