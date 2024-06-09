using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{

    public static float jumpForce = 25.0f;
    public static int maxJumps = 1;
    public static float jumpTime = 0.1f;
    public static float maxJump = 1.0f;

    public static float moveForce = 12.5f;
    public static float maxVelocity = 3.5f;

    public static int experience = 0;
    public static int upgradePoints = 0;

    public static int hp = 50;
    public static int defense = 0;

    //current levels for player and player stats -- used to calculate actual properties
    public static int playerLevel = 1;
    public static int jumpForceLevel = 1;
    public static int maxJumpsLevel = 1;
    public static int jumpTimeLevel = 1;
    public static int moveForceLevel = 1;
    public static int maxVelocityLevel = 1;
    public static int hpLevel = 1;
    public static int defenseLevel = 1;
    
    public static void playerLevelUp()
    {
        PlayerStats.playerLevel += 1;
    }
}
