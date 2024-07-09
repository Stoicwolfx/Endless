using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ProjectilesJson
{
    //public List<EnemyJson> enemiesJson = new List<EnemyJson>();
    public ProjectileJson[] projectilesJson;
}

[Serializable]
public class ProjectileStats
{
    public int speed;
    public int power;
    public int range;
    public int splashDamage;
    public int dropWeight;
}

[Serializable]
public class ProjectileJson
{
    public int id;
    public string description;
    public string projectileType;
    public string pClass;
    public string sprite;
    public ProjectileStats stats;
}

public class ProjectileDatabase : MonoBehaviour
{
    private List<Projectile> projectiles = new();

    public void Awake()
    {
        this.BuildDatabase();
        Globals.databasesStatus.projetilesBuilt = true;
    }


    private void BuildDatabase()
    {

        string projectileJsonFile = Directory.GetCurrentDirectory() + @"\Assets\Project\Resources\Data\projectileDatabase.json";
        string projectileJson = "{\"projectilesJson\":" + File.ReadAllText(projectileJsonFile) + "}";

        ProjectilesJson rootJson = JsonUtility.FromJson<ProjectilesJson>(projectileJson);

        foreach (ProjectileJson pJson in rootJson.projectilesJson)
        {

            Sprite sprite = (pJson.sprite == null) ? null : Resources.Load<Sprite>(pJson.sprite);

            Dictionary<string, int> stats = new()
            {
               {"Speed", pJson.stats.speed},
               {"Power", pJson.stats.power},
               {"Range", pJson.stats.range},
               {"SplashDamage", pJson.stats.splashDamage},
               {"DropWeight", pJson.stats.dropWeight }
           };

            Projectile projectile = new(
                pJson.id,
                Projectile.GetType(pJson.projectileType),
                pJson.description,
                Projectile.GetClass(pJson.pClass),
                sprite,
                stats);

            this.projectiles.Add(projectile);
        }
    }

    public Projectile GetProjectile(Projectile.ProjectileType type)
    {
        Projectile temp = this.projectiles.Find(projectile => projectile.GetType() == type);
        return temp;
    }
}
