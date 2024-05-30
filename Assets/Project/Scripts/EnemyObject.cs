using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using static Enemy;

public class EnemyObject : MonoBehaviour
{
    private EnemyDatabase enemyDatabase;

    private Enemy enemy;
    private Rigidbody2D rig;

    private bool jumping;

    private float checkRadius = 0.01f;

    private void Awake()
    {
        this.jumping = false;
        if (!Globals.databasesStatus.enemiesBuilt)
            { GameObject.FindAnyObjectByType<EnemyDatabase>().Awake(); }
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

    public void Create(string name, float xSurface, float ySurface)
    {
        this.enemy = new Enemy(this.enemyDatabase.GetEnemy(name));
        this.rig = this.GetComponent<Rigidbody2D>();

        foreach (Transform child in this.transform)
        {
            if (child.name == name)
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
        if (enemy.GetDomain() == Enemy.domain.ground)
        {
            this.gameObject.AddComponent<CircleCollider2D>();
            this.transform.position = new Vector3(xSurface - this.transform.localScale.x, ySurface + 0.5f * this.transform.localScale.y, 0);
            this.GroundBehavior(name);
        }
        else if (enemy.GetDomain() == Enemy.domain.air)
        {
            this.gameObject.AddComponent<BoxCollider2D>();
            this.transform.position = new Vector3(xSurface, Random.Range(Globals.maxSurfaceHeight + 0.75f - 5.0f, 5.0f - 0.5f), 0);
            this.rig.gravityScale = 0.0f;
            this.AirBehavior(name);
        }
    }

    public int GetStat(string stat)
    {
        return this.enemy.GetStat(stat);
    }

    public void Damage(int damage)
    {
        if(!this.enemy.Damage(damage))
        {
            Destroy(this.gameObject);
        }
    }

    public Enemy.domain GetDomain()
    {
        return this.enemy.GetDomain();
    }

    //Need to update this behaviour to 'see' ahead and determine if it needs to jump
    private void GroundBehavior(string title)
    {
        bool wall = false;
        bool hill = false;
        bool gap = false;

        float perpendicular = 0.0f;
        float checkX = 0.0f;
        float checkY = 0.0f;
        float radius = this.checkRadius;
        Collider2D[] intersections = null;


        //Note - I should be able to refactor this to a much smaller code considering the multiple repeated loops
        if (!this.jumping)
        {
            //check for approaching gap
            //Note - need to check that this checkpoint is where I want it to be
            checkX = this.transform.position.x - this.enemy.GetStat("Speed") * Time.deltaTime;
            checkY = this.transform.position.y;

            intersections = Physics2D.OverlapCircleAll(new Vector2(checkX, checkY), radius);

            foreach (Collider2D intersection in intersections)
            {
                if (intersection.gameObject.CompareTag("Gap"))
                {
                    gap = true;
                    break;
                }
            }
            intersections = null;

            //check for being on a hill
            float travelDirection = Mathf.Atan2(this.rig.velocity.y, this.rig.velocity.x); //Radians

            float rotation = (travelDirection * Mathf.Rad2Deg > 270.0f) ? (travelDirection * Mathf.Rad2Deg - 270.0f) 
                                                                        : (270.0f - travelDirection * Mathf.Rad2Deg);
            if ((rotation > Globals.minRotationDeg) && (rotation < Globals.maxRotationDeg))
            {
                perpendicular = travelDirection - (Mathf.PI / 2);
                checkX = this.transform.position.x - this.transform.localScale.x * Mathf.Cos(perpendicular) * 1.1f;
                checkY = this.transform.position.y - this.transform.localScale.y * Mathf.Sin(perpendicular) * 1.1f;

                intersections = Physics2D.OverlapCircleAll(new Vector2(checkX, checkY), radius);

                foreach (Collider2D intersection in intersections)
                {
                    if (intersection.gameObject.CompareTag("Surface"))
                    {
                        hill = true;
                        break;
                    }
                }
                intersections = null;
            }

            //check for approaching wall
            //3 cases to consider
            // 1 - flat ground
            // 2 - uphill
            // 3 - downhill

        }

        if (this.jumping)
        {
            //jumping action
        }
        else if (gap)
        {
            //gap action - start jump
            //this.jumping = true;
        }
        else if (hill && !wall)
        {
            //hill with no wall at top
        }
        else if (hill && wall)
        {
            //hill and wall action
        }
        else if (wall && !hill)
        {
            //wall action
        }
        else
        {
            //flat terrain or downhill
            if (this.rig.velocity.x > (Globals.scrollRate - this.GetStat("Speed")) &&
                this.rig.velocity.y >= 0.0f)
            {
                //Accelerate to max speed
                this.rig.AddForce(new Vector2(-0.5f, 0.0f), ForceMode2D.Impulse);
            }
            else
            {
                //Maintain speed
                this.rig.velocity = new Vector2(this.rig.velocity.x, this.rig.velocity.y);
            }
        }

        wall = false;
        hill = false;
    }

    private void AirBehavior(string title)
    {
        this.rig.velocity = new Vector3(Globals.scrollRate - this.enemy.GetStat("Speed"), 0.5f * Mathf.Sin(this.transform.position.x), 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null)
        {
            return;
        }

        //this will need to be adjusted or removed after I update the ground behavior
        if (collision.gameObject.tag == "Surface")
        {
            this.jumping = false;
        }


        else if (collision.gameObject.CompareTag("Projectile"))
        {
            ProjectileObject projectile = collision.gameObject.GetComponent<ProjectileObject>();
            
            //If projectile was already destroyed (hit two things at once) then return
            if (projectile == null)
            {
                return;
            }

            this.Damage(projectile.GetPower());
            if (collision == null) return;
            Destroy(projectile);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
