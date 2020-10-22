using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.MLAgents;
using Quaternion = UnityEngine.Quaternion;

public class RobotController : Agent
{
    Quaternion originalRotation;
    public float sensitivityX = 7F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float Health = 100;
    Rigidbody rb;
    float m_Speed;
    bool grounded;
    Vector3 startingPosition;
    GameObject[] robot;
    // Start is called before the first frame update


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        m_Speed = 10.0f;
        grounded = true;
    }

    public void TakeDamage(float x)
    {
        Health -= x;
        Debug.Log(Health);

        if (Health < 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    /* public override void OnActionReceived(float[] vectorAction)
     {
         //Kollar, ska vi hoppa?
         if (vectorAction[0] == 1)
         {
             jump();

         }
     }

     public override void OnEpisodeBegin()
     {
         Reset();
     }

     public override void Heuristic(float[] actionsOut)
     {
         //Gör ingenting
         actionsOut[0] = 0;
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


         if (Input.GetKey(KeyCode.LeftArrow))
         {
             transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * m_Speed, Space.World);
         }

         if (Input.GetKey(KeyCode.RightArrow))
         {
             transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * m_Speed, Space.World);
         }
     }*/
    public void FixedUpdate()
    {
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

            foreach (GameObject x in robot)
            {
                if (Vector3.Distance(rb.transform.position, x.transform.position) < 15)
                {
                    x.GetComponent<R2AI>().gotAttacked(10.0f);
                }
            }
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


    /*
    public void jump()
    {
        if (grounded)
            rb.velocity = new UnityEngine.Vector3(0, 5, 0);
    }
    private void Reset()
    {
        transform.position = startingPosition;
        rb.velocity = Vector3.zero;
        pointsText.GetComponent<TextMesh>().text = points.ToString();
    }*/
}
