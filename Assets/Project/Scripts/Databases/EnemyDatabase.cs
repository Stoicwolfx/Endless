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


    private void BuildDatabase()
    {

        string enemyJsonFile = Directory.GetCurrentDirectory() + @"\Assets\Project\Resources\Data\enemyDatabase.json";
        string enemyJson = "{\"enemiesJson\":" + File.ReadAllText(enemyJsonFile) + "}";

        EnemiesJson rootJson = JsonUtility.FromJson<EnemiesJson>(enemyJson);

        foreach (EnemyJson eJson in rootJson.enemiesJson)
        {
            Enemy.domain eDmn = eJson.dmn switch
            {
                "ground" => Enemy.domain.ground,
                "air" => Enemy.domain.air,
                _ => Enemy.domain.ground,
            };
            Weapon weapon = (eJson.weapon == null) ? null : this.weaponDatabase.GetWeapon(eJson.weapon);
            Sprite sprite = (eJson.sprite == null) ? null : Resources.Load<Sprite>(eJson.sprite);

            Dictionary<string, int> stats = new Dictionary<string, int>
            {
               {"Power", eJson.stats.Power},
               {"Defense", eJson.stats.Defense},
               {"HP", eJson.stats.HP},
               {"Speed", eJson.stats.Speed},
               {"Experience", eJson.stats.Experience}
           };

            Enemy enemy = new Enemy(
                eJson.id,
                eJson.name, 
                eDmn,
                eJson.level,
                eJson.description,
                weapon,
                sprite,
                stats);

            this.enemies.Add(enemy);
        }
    }

    public Enemy GetEnemy(string name)
    {
        Enemy temp = this.enemies.Find(enemy => enemy.GetName() == name);
        return temp;
    }

}
