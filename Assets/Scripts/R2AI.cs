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
        enemy = GameManager.instance.Player;
        fsm = GetComponent<Animator>();
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        GameManager.instance.Enemies.Add(this.gameObject);
    }

    public void FixedUpdate()
    {
        // Get distance to player character // 
        float distance = Vector3.Distance(transform.position, enemy.transform.position);

        // Get direction to player Character // 
        Vector3 direction = enemy.transform.position - transform.position;

        // Get angle to player character // 
        float angle = Vector3.Angle(direction, transform.forward);

        // if robothealth < 25 get healing 
        if (robotHealth > 25)
        {
            // always reset getHealth to false if health above limit //
            fsm.SetBool("getHealth", false);

            // If distance and angle to enemy is within reach attack 
            if (distance < 15 && angle < 100)
            {
                // Keep enemy in sight // 
                fsm.SetBool("enemyInSight", true);
            }
            else
            {
                // Enemy not in sight // 
                fsm.SetBool("enemyInSight", false);
                fsm.SetBool("canAttack", false);
            }
            if (distance < 10 && angle < 80)
            {
                // Go to attack state // 
                fsm.SetBool("canAttack", true);
            }
            else
            {
                // If player goes out of limits set to False // 
                fsm.SetBool("canAttack", false);
            }
        }
        else
        {
            // Getting health // 
            fsm.SetBool("getHealth", true);
        }

        // If robot health goes below 1 add score and reward and destroy current robot instance //
        if (this.robotHealth < 1.0f)
        {
            enemy.GetComponent<RobotController>().AddScore(score);
            GameManager.instance.SetDeathTime(Time.time);
            GameManager.instance.Enemies.Remove(this.gameObject);
            Destroy(this.gameObject);

        }


    }

    // Take Damage when attacked // 
    public void gotAttacked(float a)
    {
        // Reduce health of enemy with incoming value //
        this.robotHealth -= a;

 
    }


    // Get health if health below 25 and above 0 heal untill 50 // 
    public void gotHealth()
    {
        if (robotHealth > 0f)
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

    // Set bool fsm lookaround to true // 
    public void lookAround()
    {
        fsm.SetBool("look", true);
    }
    // Set bool fsm lookaround to false // 
    public void setLookFalse()
    {
        fsm.SetBool("look", false);
    }
}
