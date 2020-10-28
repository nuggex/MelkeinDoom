using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    public RobotController rc;
    private void Awake()
    {
        
        instance = this;
    }

    public void ResetGame()
    {
        Debug.Log("Resetting");
        resetCollectibles();
        //resetEnemies();
        resetPlayer();
    }

    private void resetCollectibles()
    {
        Debug.Log("Resetting collectibles");
        Transform collectibleHolder = GameObject.Find("Level/Collectibles").transform;
        
        Transform[] rewards = collectibleHolder.GetComponentsInChildren<Transform>(includeInactive: true);

        //collectibleHolder.gameObject.SetActive(true);
        foreach (Transform reward in rewards)
        {
            reward.gameObject.SetActive(true);
        }
        return;
    }
    private void resetPlayer()
    {
        Debug.Log("Resetting player");
        rc.Reset();
    }
    private void resetEnemies()
    {

    }

    public float DeathTime = 0;
    public float GetDeathTime()
    {
        return DeathTime;
    }
    public void SetDeathTime(float IncomingDeath)
    {
        DeathTime = IncomingDeath;
    }



}

public enum Rewards
{
 
    hotdogReward = 10,
    burgerReward = 1,
    cheeseReward = 4,
    killReward = 3,

    // Penalites

    takeDamage = -1,
    death = -5,
    timePenalty = -2
}
