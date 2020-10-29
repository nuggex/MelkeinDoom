using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class R2D2Spawner : MonoBehaviour
{
    // Prefabs and variables that are needed // 
    public R2AI r2PreFab;
    public R2AI r2;
    float nextSpawn = 0;
    public Transform R2D2Holder;
    public string wpName;
    public bool firstSpawn = true;
    // Update is called once per frame
    private void Start()
    {
        // Set spawn timer to current tieme and find game object positions // 
        nextSpawn = Time.time;
        R2D2Holder = GameObject.Find("Level/Enemies").transform;
    }
    void FixedUpdate()
    {
        // If no R2 exists and time since last spawn is over 10 spawn a new // 
        if ((!r2 && Time.time - nextSpawn > 10) ||(!r2 && firstSpawn))
        {
            // Instantiate and initialize the prefab // 
            r2 = Instantiate(r2PreFab, transform.position, transform.rotation, R2D2Holder);
            r2.GetComponent<R2D2Patrol>().initPatrol(wpName);
            firstSpawn = false;
        }
        // Get Death Time from GameManager // 
        nextSpawn = GameManager.instance.GetDeathTime();
    }

    // Old stuff // 
    public void IsKilled()
    {
        nextSpawn = Time.time;
    }
    public void SetNextSpawn(float KilledTime)
    {
        nextSpawn = KilledTime;
    }

}
