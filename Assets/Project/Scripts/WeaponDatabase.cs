using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Weapon;

[Serializable]
public class WeaponsJson
{
    //public List<EnemyJson> enemiesJson = new List<EnemyJson>();
    public WeaponJson[] weaponsJson;
}

[Serializable]
public class WeaponStats
{
    public int rateOfFire;
    public int speed;
    public int power;
    public int accuracy;
    public int range;
    public int magRounds;
    public int maxRounds;
    public int reloadTime;
}

[Serializable]
public class WeaponJson
{
    public int id;
    public string type;
    public string name;
    public string dmn;
    public int level;
    public string description;
    public string weapon;
    public string sprite;
    public WeaponStats stats;
}

public class WeaponDatabase : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>();
    private ProjectileDatabase projectileDatabase;

    private void Awake()
    {
        this.projectileDatabase = GameObject.FindAnyObjectByType<ProjectileDatabase>();
        this.BuildDatabase();
    }

    //    public Weapon(int id, weaponType type, string name, string description, Sprite sprite, Dictionary<string, int> stats)

    private void BuildDatabase()
    {

        string weaponJsonFile = Directory.GetCurrentDirectory() + @"\Assets\Project\Resources\Data\enemyDatabase.json";
        string weaponJson = "{\"weaponsJson\":" + File.ReadAllText(weaponJsonFile) + "}";

        WeaponsJson rootJson = JsonUtility.FromJson<WeaponsJson>(weaponJson);

        foreach (WeaponJson wJson in rootJson.weaponsJson)
        {
            Weapon.weaponType weaponType;
            switch (wJson.type)
            {
                case "kinetic":
                    weaponType = Weapon.weaponType.kinetic;
                    break;
                case "explosive":
                    weaponType = Weapon.weaponType.explosive;
                    break;
                case "energy":
                    weaponType = Weapon.weaponType.energy;
                    break;
                case "melee":
                    weaponType = Weapon.weaponType.melee;
                    break;
                default:
                    weaponType = Weapon.weaponType.kinetic;
                    break;
            }

            Sprite sprite = (wJson.sprite == null) ? null : Resources.Load<Sprite>(wJson.sprite);
            Projectile projectile = (wJson.weapon == null) ? null : this.projectileDatabase.GetProjectile(wJson.weapon);

            Dictionary<string, int> stats = new Dictionary<string, int>
            {
               {"RateOfFire", wJson.stats.rateOfFire},
               {"Speed", wJson.stats.speed},
               {"Power", wJson.stats.power},
               {"Accuracy", wJson.stats.accuracy},
               {"Range", wJson.stats.range},
               {"MagRounds", wJson.stats.magRounds},
               {"MaxRounds", wJson.stats.maxRounds},
               {"ReloadTime", wJson.stats.reloadTime}
           };

            Weapon weapon = new Weapon(
                wJson.id,
                weaponType,
                wJson.name,
                wJson.description,
                sprite,
                projectile,
                stats);

            this.weapons.Add(weapon);
        }
    }

    public Weapon GetWeapon(string name)
    {
        Weapon temp = this.weapons.Find(weapon => weapon.GetName() == name);
        return temp;
    }
}
