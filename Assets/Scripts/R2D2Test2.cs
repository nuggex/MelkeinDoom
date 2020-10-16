﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class R2D2Test2 : MonoBehaviour
{
    public List<GameObject> waypoints;
    int wpIndex = 0;
    NavMeshAgent nav;


    // Start is called before the first frame update
    void Start()
    {
        //waypoints = GameObject.FindGameObjectsWithTag("wp1").ToList();

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.SetDestination(waypoints[wpIndex].transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, waypoints[wpIndex].transform.position) <= 5.4f)
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
