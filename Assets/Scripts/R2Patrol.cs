using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class R2Patrol : MonoBehaviour
{

    public List<GameObject> waypoints;
    int wpIndex = 0;
    NavMeshAgent nav;


    // Start is called before the first frame update
    void Start()
    {

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.SetDestination(waypoints[wpIndex].transform.position);
    }

    public void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, waypoints[wpIndex].transform.position) <= 3.4f)
        {
            wpIndex++;

            if (wpIndex >= waypoints.Count)
            {
                wpIndex = 0;
            }
            nav.SetDestination(waypoints[wpIndex].transform.position);
        }
    }
}
