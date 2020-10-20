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
    float robotHealth = 50; 

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
        if (distance < 15 && angle < 70)
        {
            fsm.SetBool("enemyInSight", true);
            if(distance < 10 && angle < 50)
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
}
