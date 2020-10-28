using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class R2AI : MonoBehaviour
{
    GameObject enemy;
    Animator fsm;
    Vector3 position;
    Vector3 rotation;
    public float robotHealth = 50;
    public float starttime = 0;
    public string spawnTag = "";
    public Rewards score;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Player");

        fsm = GetComponent<Animator>();
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
    }

    public void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        Vector3 direction = enemy.transform.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        
        // if robothealth < 25 get healing 
        if (robotHealth > 25)
        {
            fsm.SetBool("getHealth", false);
            // If distance and angle to enemy is within reach attack 
            if (distance < 15 && angle < 100 && distance > 5)
            {
                fsm.SetBool("enemyInSight", true);
                if (distance < 12 && angle < 90)
                {
                    fsm.SetBool("canAttack", true);
                }
                else
                {
                    fsm.SetBool("canAttack", false);
                }

            }
            else
            {
                fsm.SetBool("enemyInSight", false);
                fsm.SetBool("canAttack", false);
            }
        }
        else
        {
            fsm.SetBool("getHealth", true);
        }

    }

    public void gotAttacked(float a)
    {
        this.robotHealth -= a;

        if (this.robotHealth < 1.0f)
        {
            enemy.GetComponent<RobotController>().AddScore(score);
            Destroy(this.gameObject);
        }
    }



    public void gotHealth()
    {
        if (robotHealth > 1.0f)
        {

            if (robotHealth < 50)
            {
                robotHealth += 25;
            }
            if (robotHealth > 50)
            {
                robotHealth = 50;
            }

        }
    }
    public void setRotatedOff()
    {
        fsm.SetBool("hasRotated", false);
    }

    public void lookAround()
    {
        fsm.SetBool("look", true);
    }

    public void setLookFalse()
    {
        fsm.SetBool("look", false);
    }
}
