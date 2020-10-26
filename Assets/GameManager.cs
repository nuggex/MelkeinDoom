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



}

public enum Rewards
{
 
    hotdogReward = 100,
    burgerReward = 5,
    cheeseReward = 20,
    killReward = 10,

    // Penalites

    takeDamage = -10,
    death = -100,
    timePenalty = -50
}
