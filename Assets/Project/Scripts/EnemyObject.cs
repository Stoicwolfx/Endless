using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class EnemyObject : MonoBehaviour
{
    private EnemyDatabase enemyDatabase;

    private Enemy enemy;
    private Rigidbody2D rig;

    public bool jumping;

    private void Awake()
    {
        this.jumping = false;
        this.enemyDatabase = GameObject.FindAnyObjectByType<EnemyDatabase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.gameRunning) return;

        if (this.enemy.GetDomain() == Enemy.domain.air)
        {
            this.AirBehavior(this.enemy.GetName());
        }

        if (this.enemy.GetDomain() == Enemy.domain.ground)
        {
            this.GroundBehavior(this.enemy.GetName());
        }

        if (this.transform.position.x < Globals.destructionLimit)
        {
            Destroy(this.gameObject);
        }
    }

    public void Create(string title, float xSurface, float ySurface)
    {
        this.enemy = new Enemy(this.enemyDatabase.GetEnemy(title));
        this.rig = this.GetComponent<Rigidbody2D>();

        foreach (Transform child in this.transform)
        {
            if (child.name == title)
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
        if (enemy.GetDomain() == Enemy.domain.ground)
        {
            this.gameObject.AddComponent<CircleCollider2D>();
            this.transform.position = new Vector3(xSurface, ySurface + 0.5f * this.transform.localScale.y, 0);
            this.GroundBehavior(title);
        }
        else if (enemy.GetDomain() == Enemy.domain.air)
        {
            this.gameObject.AddComponent<BoxCollider2D>();
            this.transform.position = new Vector3(xSurface, Random.Range(Globals.maxSurfaceHeight + 0.75f - 5.0f, 5.0f - 0.5f), 0);
            this.rig.gravityScale = 0.0f;
            this.AirBehavior(title);
        }
    }

    public int GetStat(string stat)
    {
        return this.enemy.GetStat(stat);
    }

    public void Damage(int damage)
    {
        this.enemy.Damage(damage);
    }

    public Enemy.domain GetDomain()
    {
        return this.enemy.GetDomain();
    }

    private void GroundBehavior(string title)
    {
        if (this.rig.velocity.x > (Globals.scrollRate - this.enemy.GetStat("Speed")))
        {
            this.rig.AddForce(new Vector2(-1.0f, 0.0f), ForceMode2D.Impulse);
        }
        else
        {
            this.rig.velocity = new Vector3(Globals.scrollRate - this.enemy.GetStat("Speed"), this.rig.velocity.y, 0.0f);
        }
    }

    private void AirBehavior(string title)
    {
        this.rig.velocity = new Vector3(Globals.scrollRate - this.enemy.GetStat("Speed"), 0.5f * Mathf.Sin(this.transform.position.x), 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Surface") && 
            (this.transform.position.x > (collision.transform.position.x + 0.5f * collision.transform.localScale.x)))
        {
            this.rig.AddForce(new Vector2(0.0f, 5.0f), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Gap") &&
            (this.transform.position.x > (collision.transform.position.x + 0.5f * collision.transform.localScale.x)))
        {
            this.rig.AddForce(new Vector2(0.0f, 5.0f), ForceMode2D.Impulse);
        }
    }
}
