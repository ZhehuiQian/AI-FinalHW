using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMovement : MonoBehaviour {

    public Transform target;
    float speed = 5;
    Vector3[] path;
    int targetIndex;
    public Terrain terrain;
    void Start()
    {
        //terrain = FindObjectOfType<Terrain>();
        transform.position = new Vector3(0, terrain.SampleHeight(new Vector3(0, 0, 0)), 0);
        SeekerController.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            currentWaypoint.y = terrain.SampleHeight(new Vector3(currentWaypoint.x, 0, currentWaypoint.z))+1.0f;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    //public void OnDrawGizmos()
    //{
    //    if (path != null)
    //    {
    //        for (int i = targetIndex; i < path.Length; i++)
    //        {
    //            Gizmos.color = Color.black;
    //            Gizmos.DrawCube(path[i], Vector3.one);

    //            if (i == targetIndex)
    //            {
    //                Gizmos.DrawLine(transform.position, path[i]);
    //            }
    //            else
    //            {
    //                Gizmos.DrawLine(path[i - 1], path[i]);
    //            }
    //        }
    //    }
    //}
}
