using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : StateMachineBehaviour
{
    GameObject enemy, me;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = GameObject.Find("Player");
        me = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Beräknar vektorn från robotens position till följande vägpunkt
        Vector3 direction = enemy.transform.position - me.transform.position;

        //vända roboten "sakta mak" mot nästa vägpunkt

        me.transform.rotation = Quaternion.Slerp(me.transform.rotation, Quaternion.LookRotation(direction), 0.04f);


        me.transform.Translate(0, 0, 0.06f);
    }
}
