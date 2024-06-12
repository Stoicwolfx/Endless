using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private WeaponDatabase weaponDatabase;

    public Rigidbody2D playerBody;

    private TextMeshProUGUI hpText;

    private int jumpCount = 0;
    private bool jumping = false;
    private float jumpClock = -1;
    private float ground = 0.0f;

    private float jumpForce;
    private int maxJumps;
    private float jumpTime;
    private float maxJump;
    private float maxGap;

    private float moveForce;
    private float maxVelocity;
    private float maxForward;

    private float lastRot;

    private int hp;
    private int defense;

    private int flashCount;
    private float flashLength;
    private SpriteRenderer sprite;
    private Color spriteColor;

    private GameObject gun;
    private float aimAngle;

    private WeaponObject currentWeapon;
    private List<WeaponObject> weapons;
    private Dictionary<Projectile.projectileType, int> projectiles = new Dictionary<Projectile.projectileType, int>();

    [SerializeField] private Projectile projectilePreFab;

    void Awake()
    {
        if (!Globals.databasesStatus.weaponsBuilt)
            { GameObject.FindAnyObjectByType<WeaponDatabase>().Awake(); }
        this.weaponDatabase = GameObject.FindAnyObjectByType<WeaponDatabase>();

        TextMeshProUGUI[] uiElements = GameObject.FindObjectsOfType<TextMeshProUGUI>();

        foreach (var uiElement in uiElements)
        {
            if (uiElement.name == "HP")
            {
                this.hpText = uiElement;
                break;
            }
        }

        this.Initialize();
    }

    public void LoadSavePlayerStats()
    {
        return;
    }

    public void SavePlayer()
    {
        return;
    }

public void Initialize()
    {
        this.transform.position = new Vector3(Globals.initialX, Globals.initialY, 0.0f);
        this.sprite = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        this.spriteColor = sprite.color;

        this.maxJumps = PlayerStats.maxJumps;
        this.jumpForce = PlayerStats.jumpForce;
        this.maxJump = PlayerStats.maxJump;
        this.jumpTime = PlayerStats.jumpTime;
        this.moveForce = PlayerStats.moveForce;
        this.maxVelocity = PlayerStats.maxVelocity;
        this.hp = PlayerStats.hp;
        this.defense = PlayerStats.defense;

        this.jumpClock = -1.0f;
        this.maxForward = 5.0f;

        this.maxGap = Globals.scrollRate * this.maxJump * PlayerStats.maxJumps * 1.0f;

        this.flashCount = 0;
        this.flashLength = 0.25f;

        this.gun = this.transform.GetChild(1).gameObject;
        this.aimAngle = 90.0f;

        this.projectiles[Projectile.projectileType.kinetic] = 0;
        this.projectiles[Projectile.projectileType.energy] = 0;
        this.projectiles[Projectile.projectileType.missile] = 0;

        this.weapons = new();
        this.currentWeapon = this.transform.GetChild(1).gameObject.GetComponent<WeaponObject>();
        this.currentWeapon.Create(weaponDatabase.GetWeapon("Pistol"));
        this.weapons.Add(this.currentWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.gameRunning) return;
        if ((this.transform.position.y < Globals.screenBottom) || (this.transform.position.x < Globals.destructionLimit) || (PlayerStats.hp == 0))
        {
            Globals.gameRunning = false;
        }

        if (Globals.testing)
        {
            this.playerBody.velocity = new Vector2(Globals.scrollRate, this.playerBody.velocity.y);
            if (!this.jumping)
            {
                this.playerBody.AddForce(new Vector2(0.0f, 10.0f), ForceMode2D.Impulse);
                this.jumping = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if ((this.flashLength > 0) && (this.flashCount > 0))
        {
            this.flashLength -= Time.deltaTime;
            return;
        }
        this.flashLength = 0.1f;
        if (this.flashCount > 0)
        {
            this.flashCount--;
            if (this.flashCount % 2 == 0)
            {
                this.sprite.color = this.spriteColor;
            }
            else
            {
                this.sprite.color = Color.yellow;
            }
        }


    }

    private void LateUpdate()
    {
        if (!Globals.gameRunning) return;

        //undo rotation due to parent rotation
        float deltaRot = this.transform.eulerAngles.z - this.lastRot;
        this.lastRot = this.transform.eulerAngles.z;
        this.gun.transform.RotateAround(this.transform.position, Vector3.forward, -deltaRot);

        //make sure not passed furthest acceptable point
        this.maxForward += Globals.scrollRate * Time.deltaTime;
        if (this.transform.position.x > this.maxForward)
        {
            this.transform.position = new Vector3(this.maxForward, this.transform.position.y, 0.0f);
        }

        //make sure it's not going faster than max velocity
        if (this.playerBody.velocity.x > this.maxVelocity)
        {
            this.playerBody.velocity = new Vector2(this.maxVelocity, this.playerBody.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int flashes = 5;

        if (collision.gameObject.CompareTag("Surface"))
        {
            //Need to figure out how to detect if it's a vertical or inverted side and not treat that as a "landing"
            this.jumpCount = 0;
            this.jumpClock = -1.0f;
            this.jumping = false;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (this.flashCount == 0)
            {
                EnemyObject enemy = collision.gameObject.GetComponent<EnemyObject>();
                this.hp -= (((enemy.GetStat("Power") - this.defense) > 0) ? ((enemy.GetStat("Power") - this.defense)) : (1));
                this.flashCount = flashes;
                this.sprite.color = Color.yellow;
            }
        }

        this.hpText.text = "HP: " + this.hp;

        if (this.hp <= 0)
        {
            Globals.gameRunning = false;
        }
    }

    public void Accelerate(float acceleration)
    {
        this.maxVelocity += acceleration;
        this.maxGap = this.maxVelocity * PlayerStats.maxJumps * 0.8f;
    }

    public float JumpHeight()
    {
        return this.maxJump;
    }

    public float MaxGap()
    {
        return this.maxGap;
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (this.jumping && ((this.transform.position.y - this.ground) > this.maxJump))
        {
            this.maxJump = (this.transform.position.y - this.ground) * 0.9f;
        }

        //Check jump deadzone -- needs refinement/made a global
        if (context.ReadValue<float>() < 0.2f)
        {
            this.jumping = false;
            this.jumpClock = -1.0f;
            return;
        }

        //if not jumping then start a jump
        if (!this.jumping && (this.jumpCount < this.maxJumps))
        {
            this.jumping = true;
            this.jumpClock = 0.0f;
            this.jumpCount++;

            this.ground = this.transform.position.y;

            playerBody.velocity = new Vector2(playerBody.velocity.x, 0.0f);
            playerBody.AddForce(new Vector2(0.0f, this.jumpForce), ForceMode2D.Impulse);
        }

        else if ((this.jumpClock < this.jumpTime) &&
                 (this.jumpClock >= 0.0f) && this.jumping)
        {
            playerBody.AddForce(new Vector2(0.0f, this.jumpForce), ForceMode2D.Force);
            this.jumpClock += Time.deltaTime;
        }
    }
    
    //Something is off with the movement/jumping. Pretty sure jumping is correct though.
    //Maybe have the initial move be a impulse then change to Force? Need to figure out
    //why moving can result in unlimited air-jumps too (may just be related to rdp connection though).
    public void OnMove(InputAction.CallbackContext context)
    {
        //Deadband check (should add a modifiable setting
        if (context.ReadValue<float>() == 0.02f) return;

        float xMod = context.ReadValue<float>();

        //Velocity method works but may change to the AddForce method in the future (needs tweaking though)
        if (this.playerBody.velocity.x < (this.maxVelocity + Globals.scrollRate))
        {
            //this.playerBody.velocity = new Vector2((this.maxVelocity + Globals.scrollRate) * xMod,
            //                                        this.playerBody.velocity.y);
            this.playerBody.AddForce(new Vector2(this.moveForce * xMod, 0.0f), ForceMode2D.Impulse);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        float aimX = context.ReadValue<Vector2>().x;
        float aimY = context.ReadValue<Vector2>().y;
        float newAngle = 0.0f;
        float deltaAngle = 0.0f;
        
        if ((aimX != 0) || (aimY != 0))
        {
            newAngle = Mathf.Atan2(aimY, aimX) * Mathf.Rad2Deg + 90.0f;
        }
        deltaAngle = newAngle - this.aimAngle;
        this.aimAngle = newAngle;

        this.gun.transform.RotateAround(this.transform.position, this.gun.transform.forward, deltaAngle);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        this.currentWeapon.Fire(this.aimAngle);
    }

    public int GetNumProjectile(Projectile.projectileType projectileType)
    {
        return this.projectiles[projectileType];
    }

    public void UseProjectile(Projectile.projectileType projectileType, int count)
    {
        this.projectiles[projectileType] -= count;
    }
}
