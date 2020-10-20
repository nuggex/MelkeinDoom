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

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Player");
        fsm = GetComponent<Animator>();
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);




        if (distance < 5)
        {
            fsm.SetBool("enemyInSight", true);
        }
        else
        {
            fsm.SetBool("enemyInSight", false);
        }
        Debug.Log(distance);
    }
}
