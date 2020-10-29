using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class RobotMonoS : MonoBehaviour
{

    public float Health = 100;
    Rigidbody rb;
    float m_Speed;
    public float points = 0;
    bool grounded;
    Vector3 startingPosition;
    GameObject[] robot;
    public float timer = 0.0f;
    public float yRot = 0.0f;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        m_Speed = 10.0f;
        grounded = true;
    }

    public void TakeDamage(float x)
    {
        Health -= x;

        if (Health < 0)
        {
            GameManager.instance.ResetGame();
        }
    }

    public void FixedUpdate()
    {

        // Reset game with R
        if (Input.GetKeyDown(KeyCode.R)) GameManager.instance.ResetGame();


        // Movement including Strafing 
        if (Input.GetKey(KeyCode.W)) transform.position += transform.forward * Time.deltaTime * m_Speed;

        if (Input.GetKey(KeyCode.S)) transform.position -= transform.forward * Time.deltaTime * m_Speed;

        if (Input.GetKey(KeyCode.A)) transform.position -= transform.right * Time.deltaTime * m_Speed;

        if (Input.GetKey(KeyCode.D)) transform.position += transform.right * Time.deltaTime * m_Speed;


        // Jump ( Not Currently used, constrained by navmesh rules to reduce risk of total Yeet while training)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
            }
        }

        // Rotation
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Rotate(-Vector3.up * 360.0f * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow)) transform.Rotate(Vector3.up * 360.0f * Time.deltaTime);


        // Attacking
        if (Input.GetKey(KeyCode.LeftControl))
        {
            robot = GameObject.FindGameObjectsWithTag("GameController");
            foreach (GameObject x in robot)
            {
                if (Vector3.Distance(rb.transform.position, x.transform.position) < 15.0f)
                {
                    x.GetComponent<R2AI>().gotAttacked(100.0f);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        //detect if player on ground // 
        if (collision.collider.tag == "ground")
        {
            grounded = false;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "ground")
        {
            grounded = true;
        };
    }
    public void AddScore(Rewards x)
    {
        points += (float)x;
    }
    public void TimeKeeper()
    {

        if (Time.time - timer > 90)
        {
            timer = Time.time;
        }
    }

}