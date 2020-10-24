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
using UnityEngine.SceneManagement;
using System.Net.Sockets;

public class RobotController : Agent
{
    Quaternion originalRotation;

    public float Health = 100;
    Rigidbody rb;
    float m_Speed;
    public float points = 0;
    bool grounded;
    Vector3 startingPosition;
    Quaternion startRotation;
    GameObject[] robot;
    public float timer = 0.0f;
    public float yRot = 0.0f;
    // Start is called before the first frame update


    /*public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        startingPosition = rb.transform.position;
        startRotation = transform.rotation;
        m_Speed = 10.0f;
        grounded = true;
    }*/

    public override void Initialize()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        startingPosition = rb.transform.position;
        startRotation = transform.rotation;
        m_Speed = 10.0f;
        grounded = true;
    }

    public void TakeDamage(float x)
    {
        Health -= x;
        Debug.Log(Health);
        AddReward(10.0f);
        if (Health < 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            AddReward(-100f);
            EndEpisode();
        }
    }
    
    public override void OnActionReceived(float[] vectorAction)
     {
        transform.RotateAround(transform.position, Vector3.up, 360.0f * Time.deltaTime*vectorAction[0]);
        transform.position += transform.forward * Time.deltaTime * m_Speed * vectorAction[1]*100;
        
        if(vectorAction[2] > 0)
        {
            if (grounded)
            {
             // rb.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
            }
        }

        if(vectorAction[3] > 0)
        {
            foreach (GameObject x in robot)
            {
                if (Vector3.Distance(rb.transform.position, x.transform.position) < 15)
                {
                    x.GetComponent<R2AI>().gotAttacked(10.0f);
                }
            }
        }

        Debug.Log(vectorAction[0]);

    }

     public override void OnEpisodeBegin()
     {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Reset();
     }

     public override void Heuristic(float[] actionsOut)
     {

        Debug.Log("heuristic action");
         //Gör ingenting
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;
        actionsOut[3] = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * m_Speed;
            //rb.velocity = transform.forward * m_Speed; 
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * m_Speed;
            //rb.velocity = -transform.forward * m_Speed;

            //transform.Translate(0, 0, -0.08f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * m_Speed;
            //transform.Translate( -0.08f, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * m_Speed;
            //transform.Translate( 0.08f, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (grounded)
            {
                rb.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
            }
        }

        robot = GameObject.FindGameObjectsWithTag("GameController");

        if (Input.GetKey(KeyCode.LeftArrow)) yRot -= 0.5f;


        if (Input.GetKey(KeyCode.RightArrow)) yRot += 0.5f;

        actionsOut[0] = yRot;
             
        if (Input.GetKey(KeyCode.LeftControl))
        {
            robot = GameObject.FindGameObjectsWithTag("GameController");

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

        if(rb.position.y < -60f)
        {
            EndEpisode();
        }
        TimeKeeper();
        RequestDecision();
        /*if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * m_Speed;
            //rb.velocity = transform.forward * m_Speed; 
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * m_Speed;
            //rb.velocity = -transform.forward * m_Speed;

            //transform.Translate(0, 0, -0.08f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * m_Speed;
            //transform.Translate( -0.08f, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * m_Speed;
            //transform.Translate( 0.08f, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (grounded)
            {
                rb.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
            }
        }

        

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(transform.position, Vector3.up, -360.0f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(transform.position, Vector3.up, 360.0f * Time.deltaTime);
        }


        if (Input.GetKey(KeyCode.LeftControl))
        {
            robot = GameObject.FindGameObjectsWithTag("GameController");
            foreach (GameObject x in robot)
            {
                if (Vector3.Distance(rb.transform.position, x.transform.position) < 15)
                {
                    x.GetComponent<R2AI>().gotAttacked(10.0f);
                }
            }
        }*/

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.rotation.y);
        sensor.AddObservation(rb.transform.position);
        sensor.AddObservation(rb.GetComponent<Rigidbody>().velocity);
        robot = GameObject.FindGameObjectsWithTag("GameController");

        foreach (GameObject x in robot)
        {
            
                sensor.AddObservation(Vector3.Distance(rb.transform.position, x.transform.position)); ;
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
    }


    private void Reset()
    {
        rb.transform.position = startingPosition;
        rb.transform.rotation = startRotation;
        //EndEpisode();

    }
    public void AddScore(float x)
    {
        points += x;
        AddReward(x);
    }
    public void TimeKeeper()
    {

        if (Time.time - timer > 90)
        {
            AddReward(-50.0f);
            timer = Time.time;
            EndEpisode();
        }
    }
}

