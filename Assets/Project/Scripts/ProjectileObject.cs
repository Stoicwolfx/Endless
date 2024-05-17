using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    private Player player;
    private WeaponObject weapon;

    private ProjectileDatabase projectileDatabase;

    private Projectile projectile;
    private Rigidbody2D rig;

    private float aimAngle;

    private Dictionary<string, int> stats = new Dictionary<string, int>();

    private void Awake()
    {
        if (!Globals.databasesStatus.projetilesBuilt)
            { GameObject.FindAnyObjectByType<ProjectileDatabase>().Awake(); }
        this.projectileDatabase = GameObject.FindAnyObjectByType<ProjectileDatabase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((this.transform.position.x > Globals.creationLimit) ||
            (this.transform.position.x < Globals.destructionLimit) ||
            (this.transform.position.y > -Globals.screenBottom) ||
            (this.transform.position.y < Globals.screenBottom))
        {
            Destroy(this.gameObject);
        }

    }

    public void Create(Projectile projectile, Player player, WeaponObject weapon)
    {
        this.projectile = projectile;
        this.rig = this.GetComponent<Rigidbody2D>();

        foreach (Transform child in this.transform)
        {
            if (child.name == projectile.GetName())
            {
                child.gameObject.SetActive(true);
                break;
            }
        }

        this.stats = projectile.GetStats();
        this.weapon = weapon;
        this.player = player;

        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(weapon.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    public void Create(string name, float aimAngle, Dictionary<string, int> weaponStats)
    {
        this.projectile = new Projectile(this.projectileDatabase.GetProjectile(name));
        this.rig = this.GetComponent<Rigidbody2D>();
        this.aimAngle = aimAngle;

        foreach (Transform child in this.transform)
        {
            if (child.name == name)
            {
                child.gameObject.SetActive(true);
                break;
            }
        }

        this.stats["Speed"] += weaponStats["Speed"];
        this.stats["Power"] += weaponStats["Power"];
        this.stats["Range"] += weaponStats["Range"];

        //calculate x and y velocities
        float xVelocity = (float)this.stats["Speed"] * Mathf.Cos((aimAngle - 90.0f) * Mathf.Deg2Rad);
        float yVelocity = (float)this.stats["Speed"] * Mathf.Sin((aimAngle - 90.0f) * Mathf.Deg2Rad);

        this.rig.velocity = new Vector3(xVelocity, yVelocity, 0.0f);
        this.transform.Rotate(aimAngle, 0, 0);


        //for the differences in behavior for the different types of projectiles
        if (this.projectile.GetType() == Projectile.projectileType.kinetic)
        {
            ;
        }
        else if (this.projectile.GetType() == Projectile.projectileType.energy)
        {
            ;
        }
        else if (this.projectile.GetType() == Projectile.projectileType.missile)
        {
            ;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check what the collision is with
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyObject enemy = collision.gameObject.GetComponent<EnemyObject>();
            enemy.Damage(this.stats["Power"]);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Surface"))
        {
            Destroy(this.gameObject);
        }


    }
}
