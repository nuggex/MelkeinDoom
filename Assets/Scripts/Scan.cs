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
    Quaternion startRotation;
    public float curRot;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        curRot = 0f;
        me = animator.gameObject;
        me.GetComponent<R2D2Patrol>().pauseMesh();
        startRotation = me.transform.rotation;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //me.transform.rotation = Quaternion.Slerp(me.transform.rotation, target, Time.deltaTime * 5.0f);

        //vända roboten "sakta mak" mot nästa vägpunkt

        //me.transform.rotation = Quaternion.Slerp(me.transform.rotation, Quaternion.LookRotation(direction), 0.1f);




        curRot += System.Math.Abs(me.transform.localRotation.y);

        Debug.Log(curRot);

        if (curRot > 300.0f)
        {
            Debug.Log("We have rotated atleast once");

            /*me.GetComponent<R2AI>().resetTimer();*/

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
        me.GetComponent<R2AI>().setRotatedOff();
    }

}
