using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class R2D2Patrol : MonoBehaviour
{
    public List<GameObject> waypoints;
    int wpIndex = 0;
    NavMeshAgent nav;
    public bool rotating = false;
    public GameObject healthPickup;

    // Start is called before the first frame update
    public void initPatrol(string tagName)
    {
        waypoints = GameObject.FindGameObjectsWithTag(tagName).ToList();

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.SetDestination(waypoints[wpIndex].transform.position);
    }
    void Start()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotating == false)
        {
            if (healthPickup != null)
            {
                nav.speed = 20;
                nav.angularSpeed = 1080;
                nav.acceleration = 100;
                nav.SetDestination(healthPickup.transform.position);
            }
            else
            {
                if (Vector3.Distance(transform.position, waypoints[wpIndex].transform.position) <= 3.4f)
                {
                    nav.GetComponent<R2AI>().lookAround();

                    wpIndex++;
                    if (wpIndex >= waypoints.Count)
                    {
                        wpIndex = 0;
                    }
                    nav.SetDestination(waypoints[wpIndex].transform.position);
                }
            }
        }
    }
    public List<GameObject> returnWaypoints()
    {
        return waypoints;
    }
    public GameObject setHealthWaypoint(GameObject p) => healthPickup = p;

    public void clearHealthWaypoint()
    {
        healthPickup = null;
        nav.speed = 10;
        nav.angularSpeed = 120;
        nav.SetDestination(waypoints[wpIndex].transform.position);
    }
    public void pauseMesh()
    {
        nav.isStopped = true;
    }
    public void resumeMesh()
    {
        nav.isStopped = false;
    }
}
