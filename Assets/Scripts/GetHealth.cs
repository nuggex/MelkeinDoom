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
    int length;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        health = GameObject.FindGameObjectsWithTag("health").ToList();

        me = animator.gameObject;
        length = health.Count;
        
        for(int i = 0; i < length; i++)
        {
            float dist = Vector3.Distance(health[i].transform.position, me.transform.position);
            distance.Add(dist);
            
            if (distance.Count > 1)
            {
                if (distance[i] < distance[i - 1])
                {
                    destination = health[i];
                }
            }
            else
            {
                destination = health[i];
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        me.GetComponent<R2D2Patrol>().setHealthWaypoint(destination);

        if(Vector3.Distance(destination.transform.position, me.transform.position) < 5f)
        {
            me.GetComponent<R2AI>().gotHealth();
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        me.GetComponent<R2D2Patrol>().clearHealthWaypoint();
    }
}
