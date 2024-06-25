using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class PlayerStats : object
{
    public enum Stat
    {
        JumpForce,
        MaxJumps,
        JumpTime,
        MoveForce,
        MaxVelocity,
        HP,
        Defense,
        MaxStats
    }
    public static float jumpForce = 5.0f;
    public static int maxJumps = 1;
    public static float jumpTime = 0.1f;
    public static float maxJump = 1.0f;

    public static float moveForce = 4.5f;
    public static float maxVelocity = 4.5f;

    public static int experience = 0;

    public static int hp = 50;
    public static int defense = 0;

    //current levels for player and player stats -- used to calculate actual properties
    public static int playerLevel = 1;
    public static int jumpForceLevel = 0;
    public static int maxJumpsLevel = 0;
    public static int jumpTimeLevel = 0;
    public static int moveForceLevel = 0;
    public static int maxVelocityLevel = 0;
    public static int hpLevel = 0;
    public static int defenseLevel = 0;
    
    private static void PlayerLevelUp()
    {
        PlayerStats.playerLevel++;
    }

    private static void JumpForceLevelUp()
    {
        PlayerStats.jumpForceLevel++;
        PlayerStats.jumpForce *= 1.01f;
    }

    private static void MaxJumpsLevelUp()
    {
        PlayerStats.maxJumpsLevel++;
        PlayerStats.maxJumps++;
    }

    private static void JumpTimeLevelUp()
    {
        PlayerStats.jumpTimeLevel++;
        PlayerStats.jumpTime *= 1.01f;
    }

    private static void MoveForceLevelUp()
    {
        PlayerStats.moveForceLevel++;
        PlayerStats.moveForce *= 1.01f;
    }

    private static void MaxVelocityLevelUp()
    {
        PlayerStats.maxVelocityLevel++;
        PlayerStats.maxVelocity *= 1.01f;
    }

    private static void HpLevelUp()
    {
        PlayerStats.hpLevel++;
        PlayerStats.hp = (int)((float)PlayerStats.hp * 1.01f);
    }

    private static void DefenseLevelUp()
    {
        PlayerStats.defenseLevel++;
        PlayerStats.defense = (int)((float)PlayerStats.defense * 1.01f);
    }

    public static bool UpgradeStat(PlayerStats.Stat stat)
    {
        int cost = PlayerStats.UpgradeCost(stat);

        if (cost > PlayerStats.experience) return false;
        
        PlayerStats.experience -= cost;
        PlayerStats.playerLevel++;
        switch (stat)
        {
            case PlayerStats.Stat.JumpForce:
                PlayerStats.JumpForceLevelUp();
                break;
            case PlayerStats.Stat.MaxJumps:
                PlayerStats.MaxJumpsLevelUp();
                break;
            case PlayerStats.Stat.JumpTime:
                PlayerStats.JumpTimeLevelUp();
                break;
            case PlayerStats.Stat.MoveForce:
                PlayerStats.MoveForceLevelUp();
                break;
            case PlayerStats.Stat.MaxVelocity:
                PlayerStats.MaxVelocityLevelUp();
                break;
            case PlayerStats.Stat.HP:
                PlayerStats.HpLevelUp();
                break;
            case PlayerStats.Stat.Defense:
                PlayerStats.DefenseLevelUp();
                break;
            default:
                return false;
        }
        return true;
    }

    public static int UpgradeCost(PlayerStats.Stat stat)
    {
        int cost;

        switch (stat)
        {
            case PlayerStats.Stat.JumpForce:
                cost = PlayerStats.Fibonacci(PlayerStats.jumpForceLevel);
                break;
            case PlayerStats.Stat.MaxJumps:
                cost = PlayerStats.Fibonacci(PlayerStats.maxJumpsLevel);
                break;
            case PlayerStats.Stat.JumpTime:
                cost = PlayerStats.Fibonacci(PlayerStats.jumpTimeLevel);
                break;
            case PlayerStats.Stat.MoveForce:
                cost = PlayerStats.Fibonacci(PlayerStats.moveForceLevel);
                break;
            case PlayerStats.Stat.MaxVelocity:
                cost = PlayerStats.Fibonacci(PlayerStats.maxVelocityLevel);
                break;
            case PlayerStats.Stat.HP:
                cost = PlayerStats.Fibonacci(PlayerStats.hpLevel);
                break;
            case PlayerStats.Stat.Defense:
                cost = PlayerStats.Fibonacci(PlayerStats.defenseLevel);
                break;
            default:
                cost = 0;
                break;
        }

        return cost;
    }

    private static int Fibonacci(int n)
    {
        int n0 = 0;
        int n1 = 1;

        for (int i = 0; i < n; i++)
        {
            (n0, n1) = (n1, n0 + n1);
        }

        return n0;
    }
}
