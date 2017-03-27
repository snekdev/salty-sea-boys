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
    SharkPathNode currentNode;
    SharkPathNode lastNode;

    public Transform FindNewOptimalPath(Vector3 startLocation)
    {

        SharkPathNode startPos = findStartingNode(startLocation);
        //SharkPathNode endPos = findStartingNode(endLocation);

        currentNode = startPos;

        //for each option
        //foreach (SharkPathNode item in startPos.ThisNodesOptions)
        //{
        //    //add to list and go one layer deeper
        //    List<SharkPathNode> itemOptionListnew = new List<SharkPathNode>();
        //    itemOptionListnew.Add(item);
        //    //myListOfPossiblePaths.Add()
        //}
        //bool hasItBeenSolved = false;
        //while (hasItBeenSolved == false)
        //{
        //    //populate list
        //    foreach (var item in currentNode.ThisNodesOptions)
        //    {
        //        //myListOfPossiblePaths
        //    }
        //}

        //roll the dice 10 times to try and minimise the odds of back tracking without making it impossible
        //for (int i = 0; i < 10; i++)
        //{
        //    int randomDirection = Random.Range(0, currentNode.ThisNodesOptions.Count);
        //    SharkPathNode testNode = startPos.ThisNodesOptions[randomDirection];
        //    //if (currentNode != testNode)
        //    //{
        //    //    currentNode = testNode;
        //    //    break;
        //    //}

        //    Vector3 directionToTarget = testNode.transform.position - currentNode.transform.position;
        //    float dSqrToTarget = directionToTarget.sqrMagnitude;
        //    float closestDistanceSqr = Mathf.Infinity;
        //    if (dSqrToTarget < closestDistanceSqr)
        //    {
        //        currentNode = testNode;
        //        break;
        //    }


        //}




        //find out which one to avoid next time
        //int randomDirection = Random.Range(0, currentNode.ThisNodesOptions.Count);
        //int closestNumber = 0;
        //float closestDistanceSqr = Mathf.Infinity;
        //for (int i = 0; i < currentNode.ThisNodesOptions.Count; i++)
        //{
        //    Vector3 directionToTarget = currentNode.ThisNodesOptions[i].transform.position - currentNode.transform.position;
        //    float dSqrToTarget = directionToTarget.sqrMagnitude;
        //    if (dSqrToTarget < closestDistanceSqr)
        //    {
        //        closestDistanceSqr = dSqrToTarget;
        //        closestNumber = i;
        //    }
        //}

        //lastNode = findLastNode(currentNode.transform.position);


        int randomDirection = Random.Range(0, currentNode.ThisNodesOptions.Count);
        return currentNode.ThisNodesOptions[randomDirection].transform;
        //return currentNode.transform;

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

    SharkPathNode findLastNode(Vector3 currentLocation)
    {
        SharkPathNode bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = currentLocation;
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
