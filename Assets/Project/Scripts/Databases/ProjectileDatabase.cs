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
    public string name;
    public string description;
    public string type;
    public string sprite;
    public ProjectileStats stats;
}

public class ProjectileDatabase : MonoBehaviour
{
    private List<Projectile> projectiles = new List<Projectile>();

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
            Projectile.projectileType projectileType = Projectile.GetType(pJson.type);
            //NOTE: remove this block if the above works and then implement in other databases as needed
            //switch (pJson.type)
            //{
            //    case "kinetic":
            //        projectileType = Projectile.projectileType.kinetic;
            //        break;
            //    case "energy":
            //        projectileType = Projectile.projectileType.energy;
            //        break;
            //    case "missile":
            //        projectileType = Projectile.projectileType.missile;
            //        break;
            //    default:
            //        projectileType = Projectile.projectileType.kinetic;
            //        break;
            //}

            Sprite sprite = (pJson.sprite == null) ? null : Resources.Load<Sprite>(pJson.sprite);

            Dictionary<string, int> stats = new Dictionary<string, int>
            {
               {"Speed", pJson.stats.speed},
               {"Power", pJson.stats.power},
               {"Range", pJson.stats.range},
               {"SplashDamage", pJson.stats.splashDamage},
               {"DropWeight", pJson.stats.dropWeight }
           };

            Projectile projectile = new Projectile(
                pJson.id,
                pJson.name,
                pJson.description,
                projectileType,
                sprite,
                stats);

            this.projectiles.Add(projectile);
        }
    }

    public Projectile GetProjectile(string name)
    {
        Projectile temp = this.projectiles.Find(projectile => projectile.GetName() == name);
        return temp;
    }
}
