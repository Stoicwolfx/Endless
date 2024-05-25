using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile
{
    public enum projectileType
    {
        kinetic,
        energy,
        missile
    }
    private int id;
    private string name;
    private string description;
    private projectileType type;
    private Sprite sprite;
    private Dictionary<string, int> stats = new Dictionary<string, int>();


    public Projectile(Projectile projectile)
    {
        this.id = projectile.id;
        this.name = projectile.name;
        this.description = projectile.description;
        this.type = projectile.type;
        this.sprite = projectile.sprite;
        this.stats = projectile.stats;
    }

    public Projectile(int id, string name, string description, projectileType type, Sprite sprite, Dictionary<string, int> stats)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.type = type;
        this.sprite = sprite;
        this.stats = stats;
    }

    public string GetName()
    {
        return this.name;
    }

    public new projectileType GetType()
    { 
        return this.type;
    }

    public int GetSpeed()
    {
        return this.stats["Speed"];
    }

    //CMC
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
