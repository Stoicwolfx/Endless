using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

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
    
    //Max Levels
    private static int jumpForceMaxLevel = 5;
    private static int maxJumpsMaxLevel = 3;
    private static int jumpTimeMaxLevel = 9;
    private static int moveForceMaxLevel = 9;
    private static int maxVelocityMaxLevel = 9;
    private static int hpMaxLevel = 9;
    private static int defenseMaxLevel = 9;

    
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

        if ((cost > PlayerStats.experience) || (PlayerStats.GetStatMaxLevel(stat) <= PlayerStats.GetStatLevel(stat))) return false;
        
        PlayerStats.experience -= cost;
        PlayerStats.PlayerLevelUp();
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
                cost = PlayerStats.Fibonacci(PlayerStats.maxJumpsLevel * 3);
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

    public static int GetStatLevel(PlayerStats.Stat stat)
    {
        int level = 0;

        switch (stat)
        {
            case PlayerStats.Stat.JumpForce:
                level = PlayerStats.jumpForceLevel;
                break;
            case PlayerStats.Stat.MaxJumps:
                level = PlayerStats.maxJumpsLevel;
                break;
            case PlayerStats.Stat.JumpTime:
                level = PlayerStats.jumpTimeLevel;
                break;
            case PlayerStats.Stat.MoveForce:
                level = PlayerStats.moveForceLevel;
                break;
            case PlayerStats.Stat.MaxVelocity:
                level = PlayerStats.maxVelocityLevel;
                break;
            case PlayerStats.Stat.HP:
                level = PlayerStats.hpLevel;
                break;
            case PlayerStats.Stat.Defense:
                level = PlayerStats.defenseLevel;
                break;
            default:
                level = 0;
                break;
        }

        return level;
    }

    public static int GetStatMaxLevel(PlayerStats.Stat stat)
    {
        int maxLevel = 0;

        switch (stat)
        {
            case PlayerStats.Stat.JumpForce:
                maxLevel = PlayerStats.jumpForceMaxLevel;
                break;
            case PlayerStats.Stat.MaxJumps:
                maxLevel = PlayerStats.maxJumpsMaxLevel;
                break;
            case PlayerStats.Stat.JumpTime:
                maxLevel = PlayerStats.jumpTimeMaxLevel;
                break;
            case PlayerStats.Stat.MoveForce:
                maxLevel = PlayerStats.moveForceMaxLevel;
                break;
            case PlayerStats.Stat.MaxVelocity:
                maxLevel = PlayerStats.maxVelocityMaxLevel;
                break;
            case PlayerStats.Stat.HP:
                maxLevel = PlayerStats.hpMaxLevel;
                break;
            case PlayerStats.Stat.Defense:
                maxLevel = PlayerStats.defenseMaxLevel;
                break;
            default:
                maxLevel = 0;
                break;
        }

        return maxLevel;
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
