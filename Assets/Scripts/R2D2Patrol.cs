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
    public string tagname = "";

    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag(tagname).ToList();

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.SetDestination(waypoints[wpIndex].transform.position);
    }
    // Update is called once per frame
    void Update()
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
    public List<GameObject> returnWaypoints()
    {
        return waypoints;
    }
}
