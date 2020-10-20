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
    GameObject[] robots;
    Animator fsm;
    Vector3 position;
    Vector3 rotation;
    public float robotHealth = 50;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Player");

        fsm = GetComponent<Animator>();
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        Vector3 direction = enemy.transform.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (robotHealth > 25)
        {
            fsm.SetBool("getHealth", false);
            if (distance < 15 && angle < 70 && distance > 5)
            {
                fsm.SetFloat("timedScan", 0f);
                fsm.SetBool("enemyInSight", true);
                if (distance < 10 && angle < 50)
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
                fsm.SetBool("hasRotated", false);
                fsm.SetBool("enemyInSight", false);
                // Debug.Log(Time.time);
                //fsm.SetFloat("timedScan", Time.time - fsm.GetFloat("timedScan"));

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
        Debug.Log(this.robotHealth);
        if (this.gameObject && this.robotHealth < 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void resetTimer()
    {
        fsm.SetFloat("timedScan", 0f);
    }

    public void setRotated()
    {
        fsm.SetBool("hasRotated", true);
    }
    public bool getHealhtStatus()
    {
        return fsm.GetBool("getHealth");
    }
    public void gotHealth()
    {
        if (robotHealth < 50)
        {
            robotHealth += 25;
        }
    }

}
