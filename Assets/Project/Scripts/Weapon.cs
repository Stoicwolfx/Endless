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

    private ProjectileObject projectilePrefab;
    private bool reloading;
    private float reloadClock;

    public Weapon(Weapon weapon)
    {
        this.id = weapon.id;
        this.type = weapon.type;
        this.name = weapon.name;
        this.description = weapon.description;
        this.sprite = weapon.sprite;
        this.projectile = weapon.projectile;
        this.stats = weapon.stats;

        this.reloading = false;
        this.reloadClock = 0.0f;
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

        this.reloading = false;
        this.reloadClock = 0.0f;
    }

    public string GetName()
    {
        return this.name;
    }

    public Projectile GetProjectile()
    {
        return this.projectile;
    }

    public bool Reload(int totalRounds)
    {
        if ((this.stats["MagRounds"] == 0) && (this.reloadClock == 0))
        {
            this.reloading = true;
            this.reloadClock = ((float)this.stats["ReloadTime"]) / 10.0f;
        }

        if ((this.reloadClock <= 0.0f) && this.reloading)
        {
            this.stats["MagRounds"] = (totalRounds >= this.stats["MaxRounds"]) ? this.stats["MaxRounds"] : totalRounds;
            this.reloading = false;
            this.reloadClock = 0;
        }
        else
        {
            this.reloadClock = -Time.deltaTime;
        }

        return this.reloading;
    }

    public int Fire(float aimAngle, ProjectileObject projectile)
    {
        projectile.Create(this.projectile.GetName(), aimAngle, this.stats);

        this.stats["MagRounds"]--;
        return 1;
    }
}
