using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using UnityEngine;
using static Enemy;

[Serializable]
public class EnemiesJson
{
    public EnemyJson[] enemiesJson;
}

[Serializable]
public class EnemyStats
{
    public int Power;
    public int Defense;
    public int HP;
    public int Speed;
    public int Experience;
    public int DropChance;
    public string WeaponDrop;
    public string ProjectileDrop;
}

[Serializable]
public class EnemyJson
{
    public int id;
    public string name;
    public string dmn;
    public int level;
    public string description;
    public string weapon;
    public string sprite;
    public EnemyStats stats;
}

public class EnemyDatabase : MonoBehaviour
{
    private List<Enemy> enemies = new();
    private WeaponDatabase weaponDatabase;

    public void Awake()
    {
        if (!Globals.databasesStatus.weaponsBuilt)
            { GameObject.FindAnyObjectByType<WeaponDatabase>().Awake(); }
        this.weaponDatabase = GameObject.FindAnyObjectByType<WeaponDatabase>();
        this.BuildDatabase();
        Globals.databasesStatus.enemiesBuilt = true;
    }

    private void Start()
    {
        
    }

    private void BuildDatabase()
    {

        string enemyJsonFile = Directory.GetCurrentDirectory() + @"\Assets\Project\Resources\Data\enemyDatabase.json";
        string enemyJson = "{\"enemiesJson\":" + File.ReadAllText(enemyJsonFile) + "}";

        EnemiesJson rootJson = JsonUtility.FromJson<EnemiesJson>(enemyJson);

        foreach (EnemyJson eJson in rootJson.enemiesJson)
        {
            Enemy.Domain eDmn = eJson.dmn switch
            {
                "ground" => Enemy.Domain.ground,
                "air" => Enemy.Domain.air,
                _ => Enemy.Domain.ground,
            };
            Weapon weapon = (eJson.weapon == null) ? null : this.weaponDatabase.GetWeapon(eJson.weapon);
            Sprite sprite = (eJson.sprite == null) ? null : Resources.Load<Sprite>(eJson.sprite);

            Dictionary<string, int> stats = new()
            {
               {"Power", eJson.stats.Power},
               {"Defense", eJson.stats.Defense},
               {"HP", eJson.stats.HP},
               {"Speed", eJson.stats.Speed},
               {"Experience", eJson.stats.Experience},
               {"DropChance", eJson.stats.DropChance}
            };

            //Parse Drops into arrays
            List<int> wpnDropIds = new();
            List<int> projDropIds = new();

            foreach (string wpn in eJson.stats.WeaponDrop.Split(',')) 
            {
                int.TryParse(wpn, out int wpnId);
                wpnDropIds.Add(wpnId);
            }
            foreach (string proj in eJson.stats.ProjectileDrop.Split(','))
            {
                int.TryParse(proj, out int projId);
                projDropIds.Add(projId);
            }

            Enemy enemy = new(
                eJson.id,
                eJson.name, 
                eDmn,
                eJson.level,
                eJson.description,
                weapon,
                sprite,
                stats,
                wpnDropIds,
                projDropIds);

            this.enemies.Add(enemy);
        }
    }

    public Enemy GetEnemy(string name)
    {
        Enemy temp = this.enemies.Find(enemy => enemy.GetName() == name);
        return temp;
    }

    public List<Enemy> AllEnemies()
    {
        return this.enemies;
    }


}
