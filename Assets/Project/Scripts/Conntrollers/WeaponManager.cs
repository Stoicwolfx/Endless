using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    private string weapon = "";
    private int currentMagAmmo = 0;
    private int currentTotalAmmo = 0;
    private bool meleeWeapon = false;
    [SerializeField] private TextMeshProUGUI weaponText;
    [SerializeField] private TextMeshProUGUI ammoText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.gameRunning) return;

        UpdateDisplay();
    }

    public void UpdateData(string weapon, int magAmmo, int totalAmmo, Weapon.weaponType wType)
    {
        
        this.currentMagAmmo = magAmmo;
        this.currentTotalAmmo = totalAmmo;
        this.weapon = weapon;
        this.meleeWeapon = (wType == Weapon.weaponType.melee);
    }

    private void UpdateDisplay()
    {
        this.weaponText.text = this.weapon;
        if (this.weaponText.text != "Pistol" && !this.meleeWeapon)
        {
            this.ammoText.text = this.currentMagAmmo + " / " + this.currentTotalAmmo;
        }
        else if (!this.meleeWeapon)
        {
            this.ammoText.text = this.currentMagAmmo + " / INF";
        }
        else
        {
            this.ammoText.text = "";
        }
    }

    public void UpdateMagAmmo(int ammo)
    {
        this.currentMagAmmo += ammo;
    }

    public void UpdateTotalAmmo(int ammo)
    {
        this.currentMagAmmo += ammo;
    }
}
