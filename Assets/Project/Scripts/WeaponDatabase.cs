using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>();

    private void Awake()
    {
        this.BuildDatabase();
    }

    //    public Weapon(int id, weaponType type, string name, string description, Sprite sprite, Dictionary<string, int> stats)

    private void BuildDatabase()
    {
        weapons = new List<Weapon>()
        {
            new Weapon
            (
               0,
               Weapon.weaponType.pistol,
               "Pistol",
               "Simple starting weapon.",
               null,
               null,
               new Dictionary<string, int>
               {
                   {"RateOfFire", 3},
                   {"Speed", 4},
                   {"Power", 1},
                   {"Accuracy", 9},
                   {"Range", 7},
                   {"MagRounds", 6},
                   {"MaxRounds", -1},
                   {"ReloadTime", 5},
                   {"SplashDamage", 0}
               }
            ),
            new Weapon
            (
               1,
               Weapon.weaponType.pistol,
               "1911",
               "More power but slower.",
               null,
               null,
               new Dictionary<string, int>
               {
                   {"RateOfFire", 2},
                   {"Speed", 3},
                   {"Power", 2},
                   {"Accuracy", 10},
                   {"Range", 8},
                   {"MagRounds", 7},
                   {"MaxRounds", 54},
                   {"ReloadTime", 6},
                   {"SplashDamage", 0}
               }
            )
        };
    }

    public Weapon GetWeapon(string name)
    {
        Weapon temp = this.weapons.Find(weapon => weapon.GetName() == name);
        return temp;
    }
}
