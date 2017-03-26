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

        SharkPathNode startPos = findStartingNode(startLocation);
        SharkPathNode endPos = findStartingNode(endLocation);

        //for each option
        //foreach (SharkPathNode item in startPos.ThisNodesOptions)
        //{
        //    //add to list and go one layer deeper
        //    List<SharkPathNode> itemOptionListnew = new List<SharkPathNode>();
        //    itemOptionListnew.Add(item);
        //    //myListOfPossiblePaths.Add()
        //}
        bool hasItBeenSolved = false;
        while (hasItBeenSolved == false)
        {

        }


        return endPos.transform;

    }
    private void populateNextList(SharkPathNode thisNode)
    {
        List<List<SharkPathNode>> newLists = new List<List<SharkPathNode>>();
    }
    private bool checktoseeifthislistarrives(SharkPathNode thisNode)
    {

        return false;
    }
    SharkPathNode findStartingNode(Vector3 startLocation)
    {
        SharkPathNode bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = startLocation;
        foreach (Transform potentialTarget in transform)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.GetComponent<SharkPathNode>(); ;
            }
        }

        return bestTarget;
    }

    SharkPathNode findEndingNode(Vector3 endLocation)
    {
        SharkPathNode bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = endLocation;
        foreach (Transform potentialTarget in transform)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.GetComponent<SharkPathNode>(); ;
            }
        }

        return bestTarget;
    }
}
