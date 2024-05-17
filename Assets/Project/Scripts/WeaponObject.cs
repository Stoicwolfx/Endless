using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ProjectileObject projectilePrefab;

    private WeaponDatabase weaponDatabase;
    private ProjectileDatabase projectileDatabase;

    private Weapon weapon;
    private Projectile projectile;

    private bool reloading;

    private void Awake()
    {
        //if (player == null)
        //{
        //    player = GameObject.FindAnyObjectByType<Player>();
        //}

        if (!Globals.databasesStatus.projetilesBuilt)
            { GameObject.FindAnyObjectByType<ProjectileDatabase>().Awake(); }
        if (!Globals.databasesStatus.weaponsBuilt)
            { GameObject.FindAnyObjectByType<WeaponDatabase>().Awake(); }
        this.weaponDatabase = GameObject.FindAnyObjectByType<WeaponDatabase>();
        this.projectileDatabase = GameObject.FindAnyObjectByType<ProjectileDatabase>();
        this.reloading = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.reloading)
        {
            this.reloading = this.weapon.Reload(player.GetNumProjectile(this.projectile.GetType()));
        }
    }

    public void Create(Weapon weapon)
    {
        this.weapon = weapon;
        this.projectile = weapon.GetProjectile();
    }

    public void Fire(float aimAngle)
    {
        if ((player.GetNumProjectile(this.projectile.GetType()) == 0) &&
            (this.weapon.GetName() != "Pistol")) return;

        if (this.reloading)
        {
            return;
        }
        else
        {
            this.reloading = this.weapon.Reload(player.GetNumProjectile(this.projectile.GetType()));
        }

        Vector3 position = new Vector3(this.transform.position.x + Mathf.Cos(this.transform.rotation.z) * (this.transform.localScale.y / 2.0f), 
                                       this.transform.position.y + Mathf.Sin(this.transform.rotation.z) * (this.transform.localScale.x / 2.0f), 
                                       this.transform.position.z);
        ProjectileObject newProjectile = Instantiate(this.projectilePrefab, position, Quaternion.identity);
        newProjectile.Create(this.projectile, this.player, this);

        int count = this.weapon.Fire(aimAngle, newProjectile);

        if (this.weapon.GetName() != "Pistol")
        {
            player.UseProjectile(this.projectile.GetType(), count);
        }
    }
}
