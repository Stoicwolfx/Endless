using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Weapon
{
    public enum weaponType
    {
        kinetic,
        explosive,
        energy,
        melee,
        gernade
    }

    public enum enhancementType
    {
        none,
        fire,
        ice,
        lightning,
        wind,
        speed,
        range,
        rate,
        accuracy,
        reload,
        splash
    }

    private int id;
    private weaponType type;
    private string name;
    private string description;
    private int ammoCount;
    private Sprite sprite;
    private Projectile projectile;
    private Dictionary<string, int> stats = new();
    //Deictionary Components:
    //  RateOfFire
    //  Speed
    //  Power
    //  Accuracy
    //  Range
    //  MagRounds
    //  MaxRounds
    //  ReloadTime

    private ProjectileObject projectilePrefab;

    public Weapon(Weapon weapon)
    {
        this.id = weapon.id;
        this.type = weapon.type;
        this.name = weapon.name;
        this.description = weapon.description;
        this.ammoCount = weapon.ammoCount;
        this.sprite = weapon.sprite;
        this.projectile = weapon.projectile;
        this.stats = weapon.stats;
    }

    public Weapon(int id, weaponType type, string name, string description, int ammoCount, Sprite sprite, Projectile projectile, Dictionary<string, int> stats)
    {
        this.id = id;
        this.type = type;
        this.name = name;
        this.description = description;
        this.ammoCount = ammoCount;
        this.sprite = sprite;
        this.projectile = projectile;
        this.stats = stats;
    }

    public string GetName()
    {
        return this.name;
    }

    public int GetId() 
    {
        return this.id;
    }

    public Projectile GetProjectile()
    {
        return this.projectile;
    }

    public int GetRateOfFire()
    {
        return this.stats["RateOfFire"];
    }

    public int GetMagRounds()
    {
        return this.stats["MagRounds"];
    }

    public float GetReloadTime()
    {
        return (float)this.stats["ReloadTime"] / 10.0f;
    }

    public int GetDropWeight()
    {
        return this.stats["DropWeight"];
    }

    public Dictionary<string, int> GetStats()
    {
        return this.stats;
    }

    public void UseProjectile(int projectiles)
    {
        this.stats["MagRounds"] -= projectiles;
    }

    public void ReloadMag(int totalRounds)
    {
        this.stats["MagRounds"] = (totalRounds >= this.stats["MaxRounds"]) ? this.stats["MaxRounds"] : totalRounds;
    }

    public int GetDefaultAmmo()
    {
        return this.ammoCount;
    }

    public Weapon.weaponType GetWeaponType()
    {
        return this.type;
    }
}
