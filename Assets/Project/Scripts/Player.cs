using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D playerBody;

    [SerializeField] private TextMeshProUGUI hpText;

    private int jumpCount = 0;
    private float jumpClock = -1;
    private float ground = 0.0f;

    private float jumpForce;
    private int maxJumps;
    private float jumpTime;
    private float maxJump;
    private float maxGap;

    private float moveForce;
    private float maxVelocity;

    private int experience;

    private int hp;
    private int defense;

    private int flashCount;
    private float flashLength;
    private SpriteRenderer sprite;
    private Color spriteColor;

    private GameObject gun;
    private float aimAngle;
    private float lastAimAngle;

    private float lastRotation;

    [SerializeField] private Projectile projectilePreFab;

    void Awake()
    {
        this.Initialize();

        this.gun = this.transform.GetChild(1).gameObject;
        this.aimAngle = 90.0f;
        this.lastRotation = 0.0f;
        this.lastAimAngle = 0.0f;
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
        this.experience = 0;
        this.hp = PlayerStats.hp;
        this.defense = PlayerStats.defense;

        this.jumpClock = -1.0f;

        this.maxGap = -Globals.scrollRate * this.maxJump * PlayerStats.maxJumps * 1.0f;

        this.flashCount = 0;
        this.flashLength = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.gameRunning) return;
        if ((this.transform.position.y < Globals.screenBottom) || (this.transform.position.x < -9.25f) || (PlayerStats.hp == 0))
        {
            Globals.gameRunning = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (this.flashLength > 0 && this.flashCount > 0)
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

        this.lastRotation = this.transform.eulerAngles.z;
        this.lastAimAngle = this.aimAngle;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int flashes = 5;

        if (collision.gameObject.tag == "Surface")
        {
            this.jumpCount = 0;
            this.jumpClock = -1.0f;
        }

        if (collision.gameObject.tag == "Enemy")
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
        this.maxGap = -Globals.scrollRate * this.maxJump * PlayerStats.maxJumps * 1.0f;
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
        Debug.Log(context.ReadValue<float>());
        if (context.ReadValue<float>() < 1.0f) return;

        if (this.jumpCount < this.maxJumps)
        {
            this.ground = this.transform.position.y;

            playerBody.velocity = new Vector3(playerBody.velocity.x, 0.0f, 0.0f);
            playerBody.AddForce(new Vector2(0.0f, this.jumpForce), ForceMode2D.Impulse);

            this.jumpCount++;
            this.jumpClock = 0.0f;
        }
        else if (this.jumpClock < this.jumpTime && this.jumpClock >= 0.0f)
        {
            playerBody.AddForce(new Vector2(0.0f, this.jumpForce), ForceMode2D.Impulse);
            this.jumpClock += Time.deltaTime;
        }

        if ((this.transform.position.y - this.ground) > this.maxJump)
        {
            this.maxJump = (this.transform.position.y - this.ground) * 0.9f;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() != 0.0f)
        {

            if (this.transform.position.x > 5.0f)
            {
                if (context.ReadValue<float>() < 0.0f)
                {
                    playerBody.AddForce(new Vector2(-this.moveForce, 0.0f), ForceMode2D.Impulse);
                }
                else if (context.ReadValue<float>() > 0.0f)
                {
                    playerBody.velocity = new Vector3(0.0f, playerBody.velocity.y, 0.0f);
                }
            }
            else
            {
                if (Mathf.Abs(playerBody.velocity.x) >= Mathf.Abs(this.maxVelocity + Globals.scrollRate))
                {
                    playerBody.velocity = new Vector3(((context.ReadValue<float>() < 0) ? (-1f) : (1f)) * this.maxVelocity + Globals.scrollRate, playerBody.velocity.y, 0.0f);
                }
                else
                {
                    playerBody.AddForce(new Vector2(this.moveForce * context.ReadValue<float>(), 0.0f), ForceMode2D.Impulse);
                }
            }
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        float aimX = context.ReadValue<Vector2>().x;
        float aimY = context.ReadValue<Vector2>().y;

        float deltaRotation = this.transform.eulerAngles.z - this.lastRotation;
        float deltaAim = 0.0f;

        if ((aimX != 0) || (aimY != 0))
        {
            this.aimAngle = Mathf.Atan2(aimY, aimX) * Mathf.Rad2Deg + 90.0f;
            deltaAim = this.lastAimAngle - this.aimAngle;
        }
        

        this.gun.transform.RotateAround(this.transform.position, this.gun.transform.forward, -deltaRotation + deltaAim);

    }

    public void OnFire(InputAction.CallbackContext context)
    {
        Projectile projectile = Instantiate(projectilePreFab);
    }
}
