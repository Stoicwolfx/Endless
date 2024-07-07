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
    private float reloadClock;
    private float fireClock;

    private void Awake()
    {
        if (!Globals.databasesStatus.projetilesBuilt)
            { GameObject.FindAnyObjectByType<ProjectileDatabase>().Awake(); }
        if (!Globals.databasesStatus.weaponsBuilt)
            { GameObject.FindAnyObjectByType<WeaponDatabase>().Awake(); }
        this.weaponDatabase = GameObject.FindAnyObjectByType<WeaponDatabase>();
        this.projectileDatabase = GameObject.FindAnyObjectByType<ProjectileDatabase>();
        this.reloading = false;
        this.reloadClock = 0.0f;
        this.fireClock = -1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.fireClock > 0.0f)
        {
            this.fireClock -= Time.deltaTime;
        }
        if (this.reloadClock > 0.0f)
        {
            this.reloadClock -= Time.deltaTime;
        }
    }

    public void Create(Weapon weapon)
    {
        this.weapon = weapon;
        this.projectile = weapon.GetProjectile();
    }

    public bool Reload(int totalRounds)
    {
        if ((this.weapon.GetMagRounds() == 0) &&
            !this.reloading)
        {
            this.reloading = true;
            this.reloadClock = this.weapon.GetReloadTime();
            return this.reloading;
        }

        if ((this.reloadClock <= 0.0f) && this.reloading)
        {
            this.weapon.ReloadMag(totalRounds);
            this.reloading = false;
            this.reloadClock = 0.0f;
        }

        return this.reloading;
    }

    public void Fire(float aimAngle)
    {
        //Currently detects on pull and release. Only want it detected on pull or hold... need to research
        int count = 0;
        if ((this.player.GetNumProjectile(this.projectile.GetType()) == 0) &&
            (this.weapon.GetName() != "Pistol")) return;

        if (this.reloading && (this.reloadClock > 0.0f))
        {
            return;
        }
        else if ((this.weapon.GetMagRounds() == 0) &&
                  !this.reloading)
        {
            this.Reload(this.player.GetNumProjectile(this.projectile.GetType()));
        }
        else if (this.reloading)
        {
            this.Reload(this.player.GetNumProjectile(this.projectile.GetType()));
        }

        Vector3 position = new(this.transform.position.x + Mathf.Cos(this.transform.rotation.z) * (this.transform.localScale.y / 2.0f), 
                               this.transform.position.y + Mathf.Sin(this.transform.rotation.z) * (this.transform.localScale.x / 2.0f), 
                               this.transform.position.z);

        //Copied from weapon to her -- temp note
        if (this.fireClock > 0.0f)
        {
            return;
        }
        else
        {
            ProjectileObject newProjectile = Instantiate(this.projectilePrefab, position, Quaternion.identity);

            newProjectile.Create(this.weapon.GetProjectile(), this.player);
            newProjectile.Fire(aimAngle, this.weapon.GetStats());
            this.fireClock = 1.0f / (float)this.weapon.GetRateOfFire();

            if (this.weapon.GetName() != "Pistol")
            {
                this.weapon.UseProjectile(count);
                this.player.UseProjectile(this.projectile.GetType(), count);
            }
        }
        //
    }

    public string GetName()
    {
        return this.weapon.GetName();
    }
}
