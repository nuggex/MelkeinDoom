﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GameManager makes it easier to manage the states of the game // 
    // Create instance of GameManager // 
    public static GameManager instance;

    public List<GameObject> Enemies;
    public GameObject Player;
    public float DeathTime = 0;
    // Get a RobotController // 
    public RobotController rc;

    public Transform collectibleHolder;
    private void Awake()
    {
        collectibleHolder = GameObject.Find("Level/Collectibles").transform;
        instance = this;
    }

    // Reset Game // 
    public void ResetGame()
    {
        resetCollectibles();
        //resetEnemies();
        resetPlayer();
    }
    // Reset all collectible items // 
    private void resetCollectibles()
    {
        // Get all collectible items // 
        
        // List all collectible children to a array //
        Transform[] rewards = collectibleHolder.GetComponentsInChildren<Transform>(includeInactive: true);

        // Loop through all items and set them to active // 
        foreach (Transform reward in rewards)
        {
            reward.gameObject.SetActive(true);
        }
        return;
    }
    // Reset player // 
    private void resetPlayer()
    {
        //Call reset from RobotController // 
        rc.Reset();
    }
    private void resetEnemies()
    {

    }

    // Get robot death time
    public float GetDeathTime()
    {
        return DeathTime;
    }
    // Set robot death time
    public void SetDeathTime(float IncomingDeath)
    {
        DeathTime = IncomingDeath;
    }
}

// Enums for rewards // 
public enum Rewards
{
 
    hotdogReward = 100,
    burgerReward = 10,
    cheeseReward = 15,
    killReward = 15,

    // Penalites

    takeDamage = -1,
    death = -50,
    timePenalty = -100
}
