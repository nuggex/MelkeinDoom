using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ScoringSystem : MonoBehaviour
{
    public Rewards score;
    // Start is called before the first frame update


    public void OnTriggerEnter(Collider other)
    {

        //other.gameObject.GetComponent<RobotMonoS>().AddScore(score);
        other.gameObject.GetComponent<RobotController>().AddScore(score);
        gameObject.SetActive(false);
        if (gameObject.name == "Hotdog")
        {
            GameManager.instance.ResetGame();
        }
    }
}
