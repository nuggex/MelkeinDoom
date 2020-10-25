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
    float ti = 3f;
    public float rotatedAmount;
    public float curRot;

    Quaternion qInitial;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
        rotatedAmount = 0;
        me = animator.gameObject;
        me.GetComponent<R2D2Patrol>().pauseMesh();
        rotatedAmount = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        float rotAmount = (ang * Time.deltaTime) / ti;
        rotatedAmount += rotAmount;
        if (rotatedAmount >= 360)
        {
            me.GetComponent<R2AI>().setLookFalse();
        }
        else
        {
            me.transform.RotateAround(me.transform.position, Vector3.up,rotAmount);
        }

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        me.GetComponent<R2D2Patrol>().resumeMesh();
    }

}
