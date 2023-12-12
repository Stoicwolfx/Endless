using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Surface surfacePrefab;
    [SerializeField] private EnemyObject enemyPrefab;

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private EndRunPanel endRunPanel;
    [SerializeField] private UpgradePanel upgradePanel;

    private Surface lastSurface;

    private bool clearGame;
    private int gameLevel;

    private float acceleration;
    private float accCount;
    private bool accelerating;

    private bool groundEnemyWave;
    private bool airEnemyWave;

    private float groundClockStart = 0.75f;
    private float airClockStart = 0.75f;

    private float groundClock;
    private float airClock;
    private float waveClock;
    private float startClock = 4.0f;

    private int groundEnemyCount;
    private int airEnemyCount;

    void Awake()
    {
        this.StartRun();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.gameRunning)
        {
            if (this.clearGame) this.EndRun();
            return;
        }

        if (this.lastSurface.maxX < Globals.creationLimit)
        {
            float lastX = this.lastSurface.maxX;
            float lastY = this.lastSurface.maxY;
            Surface surface = Instantiate(surfacePrefab);
            surface.Create(lastX, lastY, player.JumpHeight(), player.MaxGap());
            this.lastSurface = surface;
        }

        if (!this.groundEnemyWave && (this.waveClock <= 0f))
        {
            if (Random.value < 0.5) this.StartGroundWave();
        }
        else
        {
            if (groundEnemyCount > 0)
            {
                if (groundClock <= 0f)
                {
                    EnemyObject enemy = Instantiate(this.enemyPrefab);
                    enemy.Create("Ground Pounder", this.lastSurface.maxX, this.lastSurface.maxY);

                    this.groundEnemyCount--;
                    this.groundClock = this.groundClockStart;
                }
                else
                {
                    this.groundClock -= Time.deltaTime;
                }
            }
            else if (this.groundEnemyWave)
            {
                this.groundEnemyWave = false;
                this.waveClock = this.startClock;
            }
        }
        
        if (!this.airEnemyWave && (this.waveClock <= 0f))
        {
            if (Random.value < 0.5) this.StartAirWave();
        }
        else
        {
            if (this.airEnemyCount > 0)
            {
                if (this.airClock <= 0f)
                {
                    EnemyObject enemy = Instantiate(this.enemyPrefab);
                    enemy.Create("Air Sweeper", this.lastSurface.maxX, this.lastSurface.maxY);

                    this.airEnemyCount--;
                    this.airClock = this.airClockStart;
                }
                else
                {
                    this.airClock -= Time.deltaTime;
                }
            }
            else if (this.airEnemyWave)
            {
                this.airEnemyWave = false;
                this.waveClock = this.startClock;
            }
        }
        if (this.waveClock > 0f)
        {
            this.waveClock -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if ((int)scoreManager.GetScore() > (this.gameLevel * 10))
        {
            this.gameLevel++;

            if (!this.accelerating)
            {
                this.accCount = 60;
                this.acceleration += -1.0f;
                this.accelerating = true;
            }


            player.Accelerate(-this.acceleration);
            Globals.scrollRate += this.acceleration;

            this.accCount--;
            if (this.accCount == 0) this.accelerating = false;
        }
    }

    private void SaveData()
    {

    }

    private void LoadData()
    {
    }

    private void StartRun()
    {
        this.clearGame = true;
        this.endRunPanel.gameObject.SetActive(false);
        this.gameLevel = 1;
        Globals.gameRunning = true;
        Globals.scrollRate = Globals.startScrollRate;

        this.waveClock = this.startClock;

        Surface surface = Instantiate(surfacePrefab);
        surface.Create();

        while (surface.maxX < Globals.creationLimit)
        {
            float lastX = surface.maxX;
            float lastY = surface.maxY;
            surface = Instantiate(surfacePrefab);
            surface.Create(lastX, lastY, player.JumpHeight(), player.MaxGap());
        }
        this.lastSurface = surface;
    }

    private void EndRun()
    {
        clearGame = false;

        PlayerStats.experience += scoreManager.GetExperience();

        this.endRunPanel.DisplayEndRunPanel(scoreManager.GetScore(), scoreManager.GetExperience());

    }

    public void PostCleanUp()
    {
        this.endRunPanel.gameObject.SetActive(false);

        foreach (var surface in FindObjectsOfType<Surface>())
        {
            Destroy(surface.gameObject);
        }

        scoreManager.EndRun();

        this.upgradePanel.gameObject.SetActive(true);
    }

    public void Reload()
    {

    }

    private void StartGroundWave()
    {
        this.groundEnemyWave = true;
        this.groundClock = 0.5f;
        this.groundEnemyCount = 5;
    }

    private void StartAirWave()
    {
        this.airEnemyWave = true;
        this.airClock = 0.5f;
        this.airEnemyCount = 5;
    }
}
