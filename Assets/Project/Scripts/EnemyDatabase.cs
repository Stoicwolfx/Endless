using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDatabase : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        this.BuildDatabase();
    }


    //    public Enemy(int id, string title, int level, string description, Weapon weapon, Sprite sprite, Dictionary<string, int> stats)

    private void BuildDatabase()
    {
        enemies = new List<Enemy>()
        {
            new Enemy
            (
                0,
                "Ground Pounder",
                Enemy.domain.ground,
                1,
                "A slow moving enemy that moves along the ground. Carries no weapon.",
                null,
                null,
                new Dictionary<string, int>
                {
                    {"Power", 10},
                    {"Defense", 10},
                    {"HP", 2},
                    {"Speed", 3},
                    {"Experience", 1}
                }
            ),
            new Enemy
            (
                0,
                "Air Sweeper",
                Enemy.domain.air,
                1,
                "A slow moving enemy that moves along the ground. Carries no weapon.",
                null,
                null,
                new Dictionary<string, int>
                {
                    {"Power", 10},
                    {"Defense", 10},
                    {"HP", 2},
                    {"Speed", 3},
                    {"Experience", 1}
                }
            )
        };
    }

    public Enemy GetEnemy(string title)
    {
        Enemy temp = this.enemies.Find(enemy => enemy.GetTitle() == title);
        return temp;
    }

}
