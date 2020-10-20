using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using UnityEditor.UIElements;
using UnityEngine;

public class Scan : StateMachineBehaviour
{
    GameObject me;
    float ang = 360.0f;
    float ti = 1.0f;
    bool rotated = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        me = animator.gameObject;
        me.GetComponent<R2D2Patrol>().setRotating();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        //me.transform.rotation = Quaternion.Slerp(me.transform.rotation, target, Time.deltaTime * 5.0f);
        
        me.transform.RotateAround(me.transform.position, Vector3.up, ang * Time.deltaTime / ti);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        me.GetComponent<R2AI>().setRotated();
        me.GetComponent<R2D2Patrol>().cancelRotating();


    }

}
