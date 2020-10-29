using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.UIElements;
using UnityEngine;

public class Attack : StateMachineBehaviour
{

    // Gameobject for animator self and target player // 
    GameObject enemy, me;

    // Atack timer // 
    float timer = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get Game Objects // 
        enemy = GameObject.FindGameObjectWithTag("Player");
        me = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        // Check distance and modulate damge done based on distance // 
        float t = Vector3.Distance(enemy.transform.position, me.transform.position);
        if (Time.time - timer > 1.0f)
        {
            enemy.GetComponent<RobotController>().TakeDamage(500.0f/t);
            timer = Time.time;
        }

    }

}
