using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    [SerializeField] WeaponObject weapon;
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

    public void Create(Vector3 pos, Weapon weapon)
    {
        this.transform.position = pos;
        this.weapon = Instantiate(this.weapon);
        this.weapon.Create(new Weapon(weapon));
    }

    public WeaponObject GetWeapon()
    {
        return this.weapon;
    }
}
