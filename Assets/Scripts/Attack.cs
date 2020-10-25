using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.UIElements;
using UnityEngine;

public class Attack : StateMachineBehaviour
{
    GameObject enemy, me;
    float timer = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = GameObject.FindGameObjectWithTag("Player");
        me = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        float t = Vector3.Distance(enemy.transform.position, me.transform.position)/2.5f;
        if (Time.time - timer > 5.0f)
        {
            enemy.GetComponent<RobotController>().TakeDamage(t);
            timer = Time.time;
        }

    }

}
