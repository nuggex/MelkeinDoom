using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2D2Spawner : MonoBehaviour
{

    public R2AI r2PreFab;
    public R2AI r2;
    float timer = 0;
    public Transform R2D2Holder;
    public string wpName;

    // Update is called once per frame
    private void Start()
    {
        timer = Time.time;
        R2D2Holder = GameObject.Find("Level/Enemies").transform;
    }
    void FixedUpdate()
    {

        if (!r2 && Time.time - timer >1)
        {
            r2 = Instantiate(r2PreFab, transform.position, transform.rotation, R2D2Holder);
            r2.GetComponent<R2D2Patrol>().initPatrol(wpName);
        }
    }
}
