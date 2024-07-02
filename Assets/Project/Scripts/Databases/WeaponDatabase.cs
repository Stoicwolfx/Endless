using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Weapon;


[Serializable]
public class WeaponsJson
{
    public WeaponJson[] weaponsJson;
}

[Serializable]
public class WeaponStats
{
    public int RateOfFire;
    public int Speed;
    public int Power;
    public int Accuracy;
    public int Range;
    public int MagRounds;
    public int MaxRounds;
    public int ReloadTime;
}

[Serializable]
public class WeaponJson
{
    public int id;
    public string type;
    public string name;
    public string description;
    public string sprite;
    public string projectile;
    public WeaponStats stats;
}

public class WeaponDatabase : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>();
    private ProjectileDatabase projectileDatabase;

    public void Awake()
    {
        if (!Globals.databasesStatus.projetilesBuilt)
            { GameObject.FindAnyObjectByType<ProjectileDatabase>().Awake(); }
        this.projectileDatabase = GameObject.FindAnyObjectByType<ProjectileDatabase>();
        this.BuildDatabase();
        Globals.databasesStatus.weaponsBuilt = true;
    }

    private void Start()
    {
        
    }

    //    public Weapon(int id, weaponType type, string name, string description, Sprite sprite, Dictionary<string, int> stats)

    private void BuildDatabase()
    {

        string weaponJsonFile = Directory.GetCurrentDirectory() + @"\Assets\Project\Resources\Data\weaponDatabase.json";
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
                case "gernade":
                    weaponType = Weapon.weaponType.gernade;
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
            Projectile projectile = (wJson.projectile == null) ? null : this.projectileDatabase.GetProjectile(wJson.projectile);

            Dictionary<string, int> stats = new Dictionary<string, int>
            {
               {"RateOfFire", wJson.stats.RateOfFire},
               {"Speed", wJson.stats.Speed},
               {"Power", wJson.stats.Power},
               {"Accuracy", wJson.stats.Accuracy},
               {"Range", wJson.stats.Range},
               {"MagRounds", wJson.stats.MagRounds},
               {"MaxRounds", wJson.stats.MaxRounds},
               {"ReloadTime", wJson.stats.ReloadTime}
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
        string weaponName = (name == "Random") ? ((Weapon.weaponType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Weapon.weaponType)).Length)).ToString() : name;
        Weapon temp = this.weapons.Find(weapon => weapon.GetName() == weaponName);
        return temp;
    }
}
