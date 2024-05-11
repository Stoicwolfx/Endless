using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        this.BuildDatabase();
    }


    private void BuildDatabase()
    {



        //This is to be update tomorrow
        projectiles = new List<Projectile>()
        {
            new Projectile
            (
               0,
               "basicKinetic",
               null,
               Projectile.projectileType.kinetic,
               null,
               new Dictionary<string, int>
               {
                   {"Speed", 0},
                   {"Power", 0},
                   {"Range", 0}
               }
            ),
            new Projectile
            (
               1,
               "basicEnergy",
               null,
               Projectile.projectileType.energy,
               null,
               new Dictionary<string, int>
               {
                   {"Speed", 0},
                   {"Power", 0},
                   {"Range", 0}
               }
            )
        };
    }

    public Projectile GetProjectile(string name)
    {
        Projectile temp = this.projectiles.Find(projectile => projectile.GetName() == name);
        return temp;
    }
}
