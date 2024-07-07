using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile
{

    public enum ProjectileType
    {
        basicKinetic,
        basicEnergy,
        basicGernade,
        basicMissile
    }
    public enum ProjectileClass
    {
        kinetic,
        energy,
        gernade,
        missile
    }
    private int id;
    private string description;
    private ProjectileClass projectileClass;
    private ProjectileType type;
    private Sprite sprite;
    private Dictionary<string, int> stats = new();


    public Projectile(Projectile projectile)
    {
        this.id = projectile.id;
        this.type = projectile.type;
        this.projectileClass = projectile.projectileClass;
        this.description = projectile.description;
        this.sprite = projectile.sprite;
        this.stats = projectile.stats;
    }

    public Projectile(int id, ProjectileType type, string description, ProjectileClass projectileClass, Sprite sprite, Dictionary<string, int> stats)
    {
        this.id = id;
        this.type = type;
        this.description = description;
        this.projectileClass = projectileClass;
        this.sprite = sprite;
        this.stats = stats;
    }

    public new ProjectileType GetType()
    {
        return this.type;
    }

    public ProjectileClass GetClass()
    { 
        return this.projectileClass;
    }

    public static ProjectileType GetType(String type)
    {
        Enum.TryParse(type, out Projectile.ProjectileType projectile);
        return projectile;
    }

    public static ProjectileClass GetClass(String pClass)
    {
        Enum.TryParse(pClass, out Projectile.ProjectileClass projectile);
        return projectile;
    }

    public int GetSpeed()
    {
        return this.stats["Speed"];
    }

    public void GetStats(Dictionary<string, int> newStats)
    {
        foreach (var item in this.stats)
        {
            string key = item.Key;
            int value = item.Value;
            newStats.Add(key, value);
        }
    }
}
