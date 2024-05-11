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
    public int power;
    public int defense;
    public int hp;
    public int speed;
    public int experience;
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
    private List<Enemy> enemies = new List<Enemy>();
    private WeaponDatabase weaponDatabase;

    private void Awake()
    {
        this.weaponDatabase = GameObject.FindAnyObjectByType<WeaponDatabase>();
        this.BuildDatabase();
    }


    private void BuildDatabase()
    {

        string enemyJsonFile = Directory.GetCurrentDirectory() + @"\Assets\Project\Resources\Data\enemyDatabase.json";
        string enemyJson = "{\"enemiesJson\":" + File.ReadAllText(enemyJsonFile) + "}";

        EnemiesJson rootJson = JsonUtility.FromJson<EnemiesJson>(enemyJson);

        foreach (EnemyJson eJson in rootJson.enemiesJson)
        {
            Enemy.domain eDmn;
            switch (eJson.dmn)
            {
                case "ground": 
                    eDmn = Enemy.domain.ground;
                    break;
                case "air": 
                    eDmn = Enemy.domain.air;
                    break;
                default:
                    eDmn = Enemy.domain.ground;
                    break;
            }

            Weapon weapon = (eJson.weapon == null) ? null : this.weaponDatabase.GetWeapon(eJson.weapon);
            Sprite sprite = (eJson.sprite == null) ? null : Resources.Load<Sprite>(eJson.sprite);

            Dictionary<string, int> stats = new Dictionary<string, int>
            {
               {"Power", eJson.stats.power},
               {"Defense", eJson.stats.defense},
               {"HP", eJson.stats.hp},
               {"Speed", eJson.stats.speed},
               {"Experience", eJson.stats.experience}
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
