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
using Unity.Mathematics;
using System.Linq;
using UnityEditor;

public class RobotController : Agent
{
    // floats and bools
    float m_Speed;
    public float Health = 100f;
    public float points = 0f;
    public float timer = 0.0f;
    public float yRot = 0.0f;
    float attackTimer = 0f;

    // Bools
    bool grounded;

    // Vectors & Quaternions
    Vector3 startingPosition;
    Quaternion startRotation;

    //GameObjects 
    //GameObject[] robot;
    GameObject PlayerSpawner;
    GameObject HealthField;
    GameObject PointsField;
    GameObject TimeField;
    GameObject hotDog;
    // Rigidbodys
    Rigidbody rb;

    // Transforms
    Transform wall;
    Transform[] walls;
    //Texts 
    Text HealthText;
    Text PointText;
    Text TimeText;

    public override void Initialize()
    {
        hotDog = GameObject.Find("Level/Collectibles/Hotdog");
        // Find all wall objects for calculating distance to each // 
        wall = GameObject.Find("Level/map/walls").transform;
        walls = wall.GetComponentsInChildren<Transform>();

        // Get on screen showing fields // 
        TimeField = GameObject.Find("Time");
        TimeText = TimeField.GetComponent<Text>();
        HealthField = GameObject.Find("Health");
        HealthText = HealthField.GetComponent<Text>();
        PointsField = GameObject.Find("Points");
        PointText = PointsField.GetComponent<Text>();

        // Player Spawn point, location and modifiers //
        PlayerSpawner = GameObject.Find("Level/PlayerSpawner");
        rb = gameObject.GetComponent<Rigidbody>();
        startingPosition = rb.transform.position;
        startRotation = rb.transform.rotation;
        m_Speed = 10.0f;
        grounded = true;
        timer = Time.time;

        // Set Gamemanager Player Object to this 
        GameManager.instance.Player = this.gameObject;
    }

    // Take Damage from enemy attacks
    public void TakeDamage(float x)
    {
        // Reduce health based on incoming damage // 
        Health -= x;
        // Add negative reward for taking damage // 
        AddReward(((float)Rewards.takeDamage) / 10);
        // If Helath drops below 0 End Episode // 
        if (Health <= 0f) EndEpisode();

    }


    public override void OnActionReceived(float[] vectorAction)
    {
        // Negative Feedback for every action reduce action count
        //AddReward(-0.0015f);

        float WalkingMotion = Mathf.Clamp(vectorAction[0], -1, 1);
        float TurningMotion = Mathf.Clamp(vectorAction[1], -1, 1);
        // Rotate Around self and move Forward and back depending on Vector action -1 -> 1 
        if (TurningMotion > 0.5)
        {
            transform.Rotate(Vector3.up * 360.0f * Time.deltaTime);
        }
        else if (TurningMotion < -0.5)
        {
            transform.Rotate(-Vector3.up * 360.0f * Time.deltaTime);
        }
        if (TurningMotion < 0.5f && TurningMotion > -0.5f)
        {
            AddReward(-0.01f);
        }

        //transform.RotateAround(transform.position, Vector3.up, 360.0f * Time.deltaTime * vectorAction[0]);
        transform.position += transform.forward * Time.deltaTime * m_Speed * WalkingMotion * 15;
        if (WalkingMotion > 0)
        {
            AddReward(0.01f);
        }

        // IF VectorAction [2] > 0 and Enemy within 5 Distance units attack it and get reward if it dies, Attacks delayed to once per two Time units // 
        if (vectorAction[2] > 0)
        {
            foreach (GameObject x in GameManager.instance.Enemies)
            {
                if (Vector3.Distance(rb.transform.position, x.transform.position) < 15)
                {
                    AddReward(0.1f);
                    if (Time.time - attackTimer >= 1f)
                    {
                        x.GetComponent<R2AI>().gotAttacked(20.0f);
                        AddReward(0.25f);
                        attackTimer = Time.time;
                    }
                }
            }
        }
    }

    // Reset Episodes through Game Manager Instance on EndEpisode()
    public override void OnEpisodeBegin()
    {
        GameManager.instance.ResetGame();
    }


    // Heuristic Override for everything
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;

        // Control player forward, backward and strafing with WASD 
        if (Input.GetKey(KeyCode.W)) transform.position += transform.forward * Time.deltaTime * m_Speed;

        if (Input.GetKey(KeyCode.S)) transform.position -= transform.forward * Time.deltaTime * m_Speed;

        if (Input.GetKey(KeyCode.A)) transform.position -= transform.right * Time.deltaTime * m_Speed;

        if (Input.GetKey(KeyCode.D)) transform.position += transform.right * Time.deltaTime * m_Speed;

        // Jumping, not currently implemented due to NavMesh Constraints
        if (Input.GetKeyDown(KeyCode.Space) && grounded) gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 12, 0), ForceMode.Impulse);


        //Player rotation

        if (Input.GetKey(KeyCode.LeftArrow)) transform.Rotate(-Vector3.up * 360.0f * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow)) transform.Rotate(Vector3.up * 360.0f * Time.deltaTime);


        // Player Attack on Left Control if GameObject within reach and attack timer > 2f 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            foreach (GameObject x in GameManager.instance.Enemies)
            {
                if (Vector3.Distance(rb.transform.position, x.transform.position) < 15)
                {
                    if (Time.time - attackTimer >= 0.5f)
                    {
                        x.GetComponent<R2AI>().gotAttacked(15.0f);
                        attackTimer = Time.time;
                    }
                }
            }
        }
    }

    public void FixedUpdate()
    {
        // Set on Screen texts to current values //
        HealthText.text = "HP: " + Mathf.Floor(Health);
        PointText.text = "P: " + Mathf.Floor(points);
        TimeText.text = "T:" + Mathf.Floor(Time.time - timer);

        // If Player rigidbody has fallen through floor or walked through wall reset game // 
        if (rb.position.y < -60f)
        {
            EndEpisode();
        }

        // Keep time of game and request decision //
        TimeKeeper();
        RequestDecision();
        // Get location of current robots to robot  (Used in attacking ) // 
    }

    // Collect observartions // 
    public override void CollectObservations(VectorSensor sensor)
    {
        // Get player rotation // 
        sensor.AddObservation(transform.rotation.y);

        // Get player current position // 
        sensor.AddObservation(rb.transform.position);

        // Get Player Current Health // 
        sensor.AddObservation(Health);

        // Get Player Current Points // 
        sensor.AddObservation(points);

        foreach (Transform collectible in GameManager.instance.collectibleHolder)
        {
            sensor.AddObservation(Vector3.Distance(rb.transform.position, collectible.position));
        }

        foreach (GameObject x in GameManager.instance.Enemies)
        {
            sensor.AddObservation(Vector3.Distance(rb.transform.position, x.transform.position));
        }
    }

    // Check Collisions with world objects // 
    private void OnCollisionEnter(Collision collision)
    {
        // Add negative feedback if collsion with wall // 
        if (collision.collider.CompareTag("wall")) AddReward(-0.075f);
    }
    private void OnCollisionExit(Collision collision)
    {
        // Check if player gameObject has left ground on a jump, this isn't used // 
        if (collision.collider.CompareTag("ground")) grounded = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        // Check if player gameObject is on the ground // 
        if (collision.collider.CompareTag("ground")) grounded = true;

        // If player gameObject is continously colliding with wall give negative feedback // 
        if (collision.collider.CompareTag("wall")) AddReward(-0.1f);
    }

    // Reset player gameObject // 
    public void Reset()
    {
        // Warp player to start pos, reset rotation and reset health, points and timer// 
        gameObject.GetComponent<NavMeshAgent>().Warp(PlayerSpawner.transform.position);
        rb.rotation = startRotation;
        Health = 100.0f;
        points = 0.0f;
        timer = Time.time;
    }

    // Add Scores and rewards //
    public void AddScore(Rewards x)
    {
        // Add Points and Rewards for current rewards
        points += (float)x;
        AddReward((float)x);

        // If Rewards is from destroying a enemy set death time for use in spawning next enemy //
        if (x == Rewards.killReward) GameManager.instance.SetDeathTime(Time.time);

        // IF Reward is from a hotdog end episode and reset game // 
        if ((float)x == (float)Rewards.hotdogReward) EndEpisode();
    }

    // Time keeping
    public void TimeKeeper()
    {
        // IF Current Episode has been running for over 90 seconds // 
        if (Time.time - timer > 300)
        {
            // Add Time Penalty to rewards // 
            AddReward((float)Rewards.timePenalty);

            // End Episode // 
            EndEpisode();
        }
    }
}