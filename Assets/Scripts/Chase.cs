using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chase : StateMachineBehaviour
{
    // GameObjects // 
    GameObject enemy, me;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find game object and self with animator // 
        enemy = GameObject.FindGameObjectWithTag("Player");
        me = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Beräknar vektorn från robotens position till följande vägpunkt
        Vector3 direction = enemy.transform.position - me.transform.position;

        // Turn towards player // 
        me.transform.rotation = Quaternion.Slerp(me.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

        // Move towards player // 
        me.transform.Translate(0, 0, 0.05f);
    }
}
