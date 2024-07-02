using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    private Weapon weapon;
    private WeaponDatabase weaponDatabase;

    private void Awake()
    {
        this.weaponDatabase = GameObject.FindAnyObjectByType<WeaponDatabase>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create(Vector3 pos,string weapon)
    {
        this.transform.position = pos;
        this.weapon = weaponDatabase.GetWeapon(weapon);
    }

    public Weapon GetWeapon()
    {
        return this.weapon;
    }
}
