using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{

    public enum weaponType
    {
        kinetic,
        explosive,
        energy,
        melee
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
    private Sprite sprite;
    private Projectile projectile;
    private Dictionary<string, int> stats = new Dictionary<string, int>();
    //Deictionary Components:
    //  RateOfFire
    //  Speed
    //  Power
    //  Accuracy
    //  Range
    //  MagRounds
    //  MaxRounds
    //  ReloadTime

    public Weapon(Weapon weapon)
    {
        this.id = weapon.id;
        this.type = weapon.type;
        this.name = weapon.name;
        this.description = weapon.description;
        this.sprite = weapon.sprite;
        this.projectile = weapon.projectile;
        this.stats = weapon.stats;
    }

    public Weapon(int id, weaponType type, string name, string description, Sprite sprite, Projectile projectile, Dictionary<string, int> stats)
    {
        this.id = id;
        this.type = type;
        this.name = name;
        this.description = description;
        this.sprite = sprite;
        this.projectile = projectile;
        this.stats = stats;
    }

    public string GetName()
    {
        return this.name;
    }

    public Projectile GetProjectile()
    {
        return this.projectile;
    }
}
