using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour {

    Grid grid;
    //public GameObject seeker, target;
    SeekerController manager;

    void Awake()
    {
        // grid = GetComponent<Grid>();
        //seeker = GameObject.FindGameObjectWithTag("Seeker");
        //target = GameObject.FindGameObjectWithTag("Target");
        //print(seeker.transform.position);
        //print(target.transform.position);
        manager = GetComponent<SeekerController>();
    }

    void Start()
    {
        grid = GetComponent<Grid>();
        //FindPath(seeker.transform.position, target.transform.position);

    }

    void Update()
    {
        //FindPath(seeker.transform.position, target.transform.position);
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        //print(startNode.worldPos);
        //print(targetNode.worldPos);
        // create a open set and close set
        Heap<Node> openSet = new Heap<Node>(grid.Maxsize);
        HashSet<Node> closeSet = new HashSet<Node>();

        // add the start node to OPEN
        openSet.Add(startNode);

        // loop
        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            // add current to CLOSE
            closeSet.Add(currentNode);

            //if current is the target node: find the path!
            if (currentNode == targetNode)
            {
                pathSuccess = true;
                break;
            }

            // for each neighbour of the current node
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                // if neighbour is not traversable or neighbour is in CLOSE
                // skip to the next neighbour
                if(!neighbour.walkable || closeSet.Contains(neighbour))
                {
                    continue;
                }

                //if new path to neighbour is shorter OR neighbour is not in OPEN
                //set fcost of neighbour
                //set parent of neighbout to current
                int newMovementCostToNeighbour = currentNode.gcost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour<neighbour.gcost || !openSet.Contains(neighbour))
                {
                    neighbour.gcost = newMovementCostToNeighbour;
                    neighbour.hcost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    //if neighbour is not in OPEN
                    //add neighbout to OPEN
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        manager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode!=startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints=SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
        //print(path);
        //grid.path = path;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        // heuristics: Euclidean Distance
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return Mathf.RoundToInt(Mathf.Sqrt(distX * distX + distY * distY));
    }
}
