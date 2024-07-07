using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy
{
    //Enumerations
    public enum Domain
    {
        ground,
        air
    }

    private readonly int id;
    private readonly string name;
    private readonly Domain dmn;
    private int level;
    private readonly string description;
    private Weapon weapon;
    private Sprite sprite;
    private Dictionary<string, int> stats = new();
    private List<int> weaponDropIds = new();
    private List<int> projectileDropIds = new();

    public Enemy(Enemy enemy)
    {
        this.id = enemy.id;
        this.name = enemy.name;
        this.dmn = enemy.dmn;
        this.level = enemy.level;
        this.description = enemy.description;
        this.weapon = enemy.weapon;
        this.sprite = enemy.sprite;
        this.stats = enemy.stats;
        this.weaponDropIds = enemy.weaponDropIds;
        this.projectileDropIds = enemy.projectileDropIds;
    }

    public Enemy(int id, string name, Domain dmn, int level, string description, Weapon weapon, Sprite sprite, Dictionary<string, int> stats, List<int> weaponDropIds, List<int> projectileDropIds)
    {
        this.id = id;
        this.name = name;
        this.dmn = dmn;
        this.level = level;
        this.description = description;
        this.weapon = weapon;
        this.sprite = sprite;
        this.stats = stats;
        this.weaponDropIds = weaponDropIds;
        this.projectileDropIds = projectileDropIds;
    }

    public string GetName()
    {
        return this.name;
    }

    public Domain GetDomain()
    {
        return this.dmn;
    }

    public int GetStat(string stat)
    {
        return this.stats[stat];
    }

    public bool Damage(int damage)
    {
        this.stats["HP"] -= damage;

        return (this.stats["HP"] > 0);
    }

    public void LevelUp(int level)
    {
        //Each enemy will have a slightly different leveling algorithm eventually

        this.level = level;
    }

    public List<int> GetWeaponDrops()
    {
        return this.weaponDropIds;
    }

}
