using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ScoringSystem : MonoBehaviour
{
    // Get Rewards enum // 
    public Rewards score;


    public void OnTriggerEnter(Collider other)
    {

        //other.gameObject.GetComponent<RobotMonoS>().AddScore(score);

        // Call AddScore from RobotController // 

        other.gameObject.GetComponent<RobotController>().AddScore(score);

        // Set Collided object Active to False unless it is Hotdog, game is reset when hotdog is hit with EndEpisode // 

        if (!gameObject.CompareTag("hotdog")) gameObject.SetActive(false);


    }
}
