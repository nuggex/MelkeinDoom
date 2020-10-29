using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using UnityEditor.UIElements;
using UnityEngine;

public class Scan : StateMachineBehaviour
{

    // Variables and objects //
    GameObject me;
    float ang = 360.0f;
    float ti = 3f;
    public float rotatedAmount;

    Quaternion qInitial;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set rotated to 0, get animator, pause the navmesh.
        me = animator.gameObject;
        me.GetComponent<R2D2Patrol>().pauseMesh();
        rotatedAmount = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // create rotation and transform around self untill >360 // 
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
        // Resume nav mesh
        me.GetComponent<R2D2Patrol>().resumeMesh();
    }

}
