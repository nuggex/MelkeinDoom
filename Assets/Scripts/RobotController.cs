using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Quaternion = UnityEngine.Quaternion;
using System.Net.Sockets;
using UnityEngine.AI;
using UnityEngine.UI;

public class RobotController : Agent
{

    public float Health = 100;
    Rigidbody rb;
    float m_Speed;
    public float points = 0;
    bool grounded;
    Vector3 startingPosition;
    Quaternion startRotation;
    GameObject[] robot;
    GameObject PlayerSpawner;
    public float timer = 0.0f;
    public float yRot = 0.0f;
    GameObject HealthField;
    GameObject PointsField;
    GameObject TimeField;
    Text HealthText;
    Text PointText;
    Text TimeText;
    float attackTimer = 0f;
    public override void Initialize()
    {

        TimeField = GameObject.Find("Time");
        TimeText = TimeField.GetComponent<Text>();
        HealthField = GameObject.Find("Health");
        HealthText = HealthField.GetComponent<Text>();
        PointsField = GameObject.Find("Points");
        PointText = PointsField.GetComponent<Text>();
        PlayerSpawner = GameObject.Find("Level/PlayerSpawner");
        rb = gameObject.GetComponent<Rigidbody>();
        startingPosition = rb.transform.position;
        startRotation = rb.transform.rotation;
        m_Speed = 10.0f;
        grounded = true;
        timer = Time.time;
    }

 
    public void TakeDamage(float x)
    {
        Health -= x;
        AddReward((float)Rewards.takeDamage);
        if (Health < 0)
        {
            EndEpisode();
        }
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        //AddReward(-0.0025f);

        transform.RotateAround(transform.position, Vector3.up, 360.0f * Time.deltaTime * vectorAction[0]);
        transform.position += transform.forward * Time.deltaTime * m_Speed * vectorAction[1] * 100;

        /*if (vectorAction[2] > 0)
        {
            if (grounded)
            {
                // rb.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
            }
        }*/

        if (vectorAction[2] > 0)
        {
            foreach (GameObject x in robot)
            {
                if (Vector3.Distance(rb.transform.position, x.transform.position) < 5)
                {
                    if (Time.time - attackTimer >= 2f)
                    {
                        AddReward(0.0005f);
                        x.GetComponent<R2AI>().gotAttacked(2.0f);
                        attackTimer = Time.time;
                    }
                }
            }
        }

        

    }
    
    public override void OnEpisodeBegin()
    {
        GameManager.instance.ResetGame();
    }

    public override void Heuristic(float[] actionsOut)
    {

        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * m_Speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * m_Speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * m_Speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * m_Speed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 12, 0), ForceMode.Impulse);
            }
        }

        robot = GameObject.FindGameObjectsWithTag("GameController");

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(-Vector3.up * 360.0f * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * 360.0f * Time.deltaTime);
        }


        if (Input.GetKey(KeyCode.LeftControl))
        {
           // robot = GameObject.FindGameObjectsWithTag("GameController");

            foreach (GameObject x in robot)
            {
                if (Vector3.Distance(rb.transform.position, x.transform.position) < 15)
                {
                    x.GetComponent<R2AI>().gotAttacked(10.0f);
                }
            }
        }
    }

    public void FixedUpdate()
    {
        HealthText.text = "HP: " + Mathf.Floor(Health);
        PointText.text = "P: " + Mathf.Floor(points);
        TimeText.text = "T:" + Mathf.Floor(Time.time - timer);
        if (rb.position.y < -60f)
        {
            EndEpisode();
        }
        TimeKeeper();
        RequestDecision();
        

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.rotation.y);
        sensor.AddObservation(rb.transform.position);
        sensor.AddObservation(rb.GetComponent<Rigidbody>().velocity);
        sensor.AddObservation(Health);
        robot = GameObject.FindGameObjectsWithTag("GameController");

        foreach (GameObject x in robot)
        {
            sensor.AddObservation(Vector3.Distance(rb.transform.position, x.transform.position)); ;
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "wall")
        {
            AddReward(-0.1f);
        }
    }



    private void OnCollisionExit(Collision collision)
    {
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
        if (collision.collider.tag == "wall")
        {
            AddReward(-0.1f);
        }
    }


    public void Reset()
    {
        // getting player character back to spawner using navmesh and reset rotation // 
        gameObject.GetComponent<NavMeshAgent>().Warp(PlayerSpawner.transform.position);
        rb.rotation = startRotation;
        // Set health and points to 0 on reset // 
        Health = 100.0f;
        points = 0.0f;
        timer = Time.time;
        //GameManager.instance.ResetGame();
    }
    public void AddScore(Rewards x)
    {
        points += (float)x;
        AddReward((float)x);
        if (x == Rewards.killReward)
        {
            GameManager.instance.SetDeathTime(Time.time);
        }
        if((float)x == (float)Rewards.hotdogReward)
        {
            EndEpisode();
        }

    }
    public void TimeKeeper()
    {

        if (Time.time - timer > 90)
        {
            AddReward((float)Rewards.timePenalty);
            timer = Time.time;
            EndEpisode();
            //GameManager.instance.ResetGame();
        }
    }

    
}

