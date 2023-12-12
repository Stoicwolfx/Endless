using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy
{
    //Enumerations
    public enum domain
    {
        ground,
        air
    }

    private int id;
    private string title;
    private domain dmn;
    private int level;
    private string description;
    private Weapon weapon;
    private Sprite sprite;
    private Dictionary<string, int> stats = new Dictionary<string, int>();

    public Enemy(Enemy enemy)
    {
        this.id = enemy.id;
        this.title = enemy.title;
        this.dmn = enemy.dmn;
        this.level = enemy.level;
        this.description = enemy.description;
        this.weapon = enemy.weapon;
        this.sprite = enemy.sprite;
        this.stats = enemy.stats;
    }

    public Enemy(int id, string title, domain dmn, int level, string description, Weapon weapon, Sprite sprite, Dictionary<string, int> stats)
    {
        this.id = id;
        this.title = title;
        this.dmn = dmn;
        this.level = level;
        this.description = description;
        this.weapon = weapon;
        this.sprite = sprite;
        this.stats = stats;
    }

    public string GetTitle()
    {
        return this.title;
    }

    public domain GetDomain()
    {
        return this.dmn;
    }

    public int GetStat(string stat)
    {
        return this.stats[stat];
    }

    public void Damage(int damage)
    {
        this.stats["HP"] -= damage;
    }
}
