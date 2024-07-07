using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    private Player player;

    private ProjectileDatabase projectileDatabase;

    private Projectile projectile;
    private Vector3 velocity;
    Collider2D cldr;

    private float aimAngle;

    private Dictionary<string, int> stats = new();

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
        else
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + this.velocity.x * Time.deltaTime,
                                                             this.gameObject.transform.position.y + this.velocity.y * Time.deltaTime,
                                                             0.0f);
        }
    }

    public void Create(Projectile projectile, Player player)
    {
        this.projectile = projectile;

        foreach (Transform child in this.transform)
        {
            if (child.name == projectile.GetType().ToString())
            {
                child.gameObject.SetActive(true);
                this.cldr = child.gameObject.GetComponent<Collider2D>();
                break;
            }
        }

        projectile.GetStats(this.stats);
        this.player = player;
    }

    public void Fire(float aimAngle, Dictionary<string, int> weaponStats)
    {
        this.aimAngle = aimAngle;

        foreach (Transform child in this.transform)
        {
            if (child.name == name)
            {
                child.gameObject.SetActive(true);
                break;
            }
        }

        this.stats["Speed"] += weaponStats["Speed"] + (int)Globals.scrollRate + 2;
        this.stats["Power"] += weaponStats["Power"];
        this.stats["Range"] += weaponStats["Range"];

        //calculate x and y velocities
        float xVelocity = (float)this.stats["Speed"] * Mathf.Cos((aimAngle - 90.0f) * Mathf.Deg2Rad);
        float yVelocity = (float)this.stats["Speed"] * Mathf.Sin((aimAngle - 90.0f) * Mathf.Deg2Rad);

        this.velocity = new Vector3(xVelocity, yVelocity, 0.0f);
        this.transform.Rotate(0, 0, aimAngle - 90.0f);

        //for the differences in behavior for the different types of projectiles
        if (this.projectile.GetClass() == Projectile.ProjectileClass.kinetic)
        {
            GameObject projectileType = Globals.GetChildObjectByName(this.gameObject, "basicKinetic");
            projectileType.SetActive(true);
        }
        else if (this.projectile.GetClass() == Projectile.ProjectileClass.energy)
        {
            ;
        }
        else if (this.projectile.GetClass() == Projectile.ProjectileClass.missile)
        {
            ;
        }
    }

    public int GetPower()
    {
        return this.stats["Power"];
    }
}