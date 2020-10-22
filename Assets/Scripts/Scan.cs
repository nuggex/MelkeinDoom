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
    float ti = 3.0f;
    public float curRot;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        curRot = 0f;
        me = animator.gameObject;
        me.GetComponent<R2D2Patrol>().pauseMesh();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Save current rotation // 
        curRot += System.Math.Abs(me.transform.localRotation.y);


        if (curRot > 300.0f)
        {
             
            me.GetComponent<R2AI>().setLookFalse();
        }
        else
        {
            me.transform.RotateAround(me.transform.position, Vector3.up, ang * Time.deltaTime / ti);

        }


    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        me.GetComponent<R2D2Patrol>().resumeMesh();
    }

}
