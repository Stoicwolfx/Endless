using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{

    public enum weaponType
    {
        pistol,
        SMG,
        HMG,
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
    private Sprite playerWeapon;
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
    //  SplashDamage

    public Weapon(Weapon weapon)
    {
        this.id = weapon.id;
        this.type = weapon.type;
        this.name = weapon.name;
        this.description = weapon.description;
        this.playerWeapon = weapon.playerWeapon;
        this.projectile = weapon.projectile;
        this.stats = weapon.stats;
    }

    public Weapon(int id, weaponType type, string name, string description, Sprite playerWeapon, Projectile projectile, Dictionary<string, int> stats)
    {
        this.id = id;
        this.type = type;
        this.name = name;
        this.description = description;
        this.playerWeapon = playerWeapon;
        this.projectile = projectile;
        this.stats = stats;
    }

    public string GetName()
    {
        return this.name;
    }

    public void Fire(float aimAngle)
    {
        return;
    }
}
