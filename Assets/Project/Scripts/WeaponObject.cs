using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    private WeaponDatabase weaponDatabase;

    private Weapon weapon;

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
    public void Fire(float aimAngle)
    {
        return;
    }
}
