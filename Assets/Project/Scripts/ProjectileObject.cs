using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    private ProjectileDatabase projectileDatabase;

    private Projectile projectile;
    private Rigidbody2D rig;

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
        
    }

    public void Create(Projectile projectile)
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
    }

    public void Create(string name, float aimAngle, Dictionary<string, int> weaponStats)
    {
        this.projectile = new Projectile(this.projectileDatabase.GetProjectile(name));
        this.rig = this.GetComponent<Rigidbody2D>();

        foreach (Transform child in this.transform)
        {
            if (child.name == name)
            {
                child.gameObject.SetActive(true);
                break;
            }
        }

        int speed = this.projectile.GetSpeed() + weaponStats["Speed"];
        //calculate x and y velocities
        float xVelocity = (float)speed * Mathf.Cos((aimAngle - 90.0f) * Mathf.Deg2Rad);
        float yVelocity = (float)speed * Mathf.Sin((aimAngle - 90.0f) * Mathf.Deg2Rad);

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
}
