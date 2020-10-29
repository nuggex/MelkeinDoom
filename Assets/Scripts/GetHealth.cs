using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GetHealth : StateMachineBehaviour
{
    public List<GameObject> health;

    GameObject me;
    public List<float> distance;
    GameObject destination;
    int index = 0;
    int HealthPickupCount;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        index = 0;
        destination = null;
        health = null;
        // Get all Health objects 
        health = GameObject.FindGameObjectsWithTag("health").ToList();
        // Set me to animator object // 
        me = animator.gameObject;

        // loop through health game objects // 
        HealthPickupCount = health.Count;
        for (int i = 0; i < HealthPickupCount; i++)
        {
            distance.Add(Vector3.Distance(health[i].transform.position, me.transform.position));
        }

        float minVal = distance.Min();
        index = distance.IndexOf(minVal);
        destination = health[index];
        distance.Clear();
        HealthPickupCount = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Set navmesh destination to health point // 
        me.GetComponent<R2D2Patrol>().setHealthWaypoint(destination);

        // If distance to health < 5 get health // 
        if(Vector3.Distance(destination.transform.position, me.transform.position) < 5f)
        {
            me.GetComponent<R2AI>().gotHealth();
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Clear health waypoint and resum to normal navigation // 
        me.GetComponent<R2D2Patrol>().clearHealthWaypoint();
    }
}
