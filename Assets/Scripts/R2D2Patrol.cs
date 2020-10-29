using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class R2D2Patrol : MonoBehaviour
{
    // Variables and GameObjects //
    public List<GameObject> waypoints;
    public GameObject healthPickup;
    NavMeshAgent nav;
    //public bool rotating = false;
    int wpIndex = 0;

    // Init patrolling when GameObject is spawned // 
    public void initPatrol(string tagName)
    {
        // Find waypoints for gameobject // 
        waypoints = GameObject.FindGameObjectsWithTag(tagName).ToList();

        // Get navmesh Agent from gameObject // 
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Set Destination to waypoint index 1 // 
        nav.SetDestination(waypoints[wpIndex].transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (healthPickup != null)
        {
            // increase speed when getting Health // 
            nav.speed = 20;
            nav.angularSpeed = 1080;
            nav.acceleration = 100;
            nav.SetDestination(healthPickup.transform.position);
        }
        else
        {
            // If waypoint reached set destination as next in waypoints list // 
            if (Vector3.Distance(transform.position, waypoints[wpIndex].transform.position) <= 3.4f)
            {
                // Look around when waypoint reached // 
                nav.GetComponent<R2AI>().lookAround();

                wpIndex++;
                // Reset waypoint index when last waypoint reached // 
                if (wpIndex >= waypoints.Count)
                {
                    wpIndex = 0;
                }

                // Set destination to next waypoint // 
                nav.SetDestination(waypoints[wpIndex].transform.position);
            }
        }
    }


    // Return waypoints // 
    public List<GameObject> returnWaypoints()
    {
        return waypoints;
    }

    // Set health waypoint // 
    public GameObject setHealthWaypoint(GameObject p) => healthPickup = p;


    // Clear health waypoints and reset nav speed // 
    public void clearHealthWaypoint()
    {
        healthPickup = null;
        nav.speed = 10;
        nav.angularSpeed = 120;
        nav.SetDestination(waypoints[wpIndex].transform.position);
    }

    // Pause the navmesh // 
    public void pauseMesh()
    {
        nav.isStopped = true;
    }

    // Resume the navmesh // 
    public void resumeMesh()
    {
        nav.isStopped = false;
    }
}
