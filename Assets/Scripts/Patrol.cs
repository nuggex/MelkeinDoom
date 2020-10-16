using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateMachineBehaviour
{
    GameObject[] waypoints;
    GameObject me;
    int wpIndex = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Hämtar alla spelobjekt som är taggade som "waypoint" och sparar dem i arrayen
        waypoints = GameObject.FindGameObjectsWithTag("wp1");
        me = animator.gameObject;
        wpIndex = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Beräknar vektorn från robotens position till följande vägpunkt
        Vector3 direction = waypoints[wpIndex].transform.position - me.transform.position;
        //vända roboten "sakta mak" mot nästa vägpunkt
        me.transform.rotation = Quaternion.Slerp(me.transform.rotation, Quaternion.LookRotation(direction), 0.04f);
        
        //OM vi inte ännu nått vägpunkten, förflytta roboten mot vägpunkten
        if (Vector3.Distance(me.transform.position, waypoints[wpIndex].transform.position) > 1.0f)
            me.transform.Translate(0, 0, 0.03f);

        //OM vi nått vägpunkten, byt index för vägpunkt (=byt vägpunkt)
        else
        {
            if (wpIndex < (waypoints.Length - 1))
                wpIndex++;
            else
                wpIndex = 0;
        }
    }

}
