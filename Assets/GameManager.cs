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
        resetCollectibles();
        resetPlayer();
        resetEnemies();
    }

    private void resetCollectibles()
    {
        Transform collectibleHolder = GameObject.Find("Level/Collectibles").transform;
        Transform[] rewards = collectibleHolder.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (Transform reward in rewards)
        {
            reward.gameObject.SetActive(true);
        }
        return;
    }
    private void resetPlayer()
    {
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

    takeDamage = -5,
    death = -10,
    timePenalty = -5
}
