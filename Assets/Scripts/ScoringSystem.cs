using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ScoringSystem : MonoBehaviour
{
    public float score = 0f;
    // Start is called before the first frame update


    public void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<RobotMonoS>().AddScore(score);
        other.gameObject.GetComponent<RobotController>().AddScore(score);
        Destroy(gameObject);
    }
}
