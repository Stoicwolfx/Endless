using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    private ProjectileDatabase projectileDatabase;

    private Projectile projectile;

    private void Awake()
    {
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

}
