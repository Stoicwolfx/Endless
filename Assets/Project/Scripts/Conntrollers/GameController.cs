using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Surface surfacePrefab;
    [SerializeField] private EnemyObject enemyPrefab;

    [SerializeField] private StartScreenPanel startScreenPanel;
    [SerializeField] private UpgradePanel upgradePanel;
    [SerializeField] private EndRunPanel endRunPanel;

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI HpText;
    [SerializeField] private CameraController cameraController;

    private Surface lastSurface;
    private Player player;

    private bool clearGame;
    private int gameLevel;
    private float scrollDelay;

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
        if (!Globals.databasesStatus.weaponsBuilt)
            { GameObject.FindAnyObjectByType<WeaponDatabase>().Awake(); }
        if (!Globals.databasesStatus.enemiesBuilt)
            { GameObject.FindAnyObjectByType<EnemyDatabase>().Awake(); }
        if (!Globals.databasesStatus.projetilesBuilt)
            { GameObject.FindAnyObjectByType<ProjectileDatabase>().Awake(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.gameRunning)
        {
            if (this.clearGame) this.EndRun();
            return;
        }

        if (Globals.scrollDelay < 0.0f)
        { 
            Globals.destructionLimit += Globals.scrollRate * Time.deltaTime;
            Globals.creationLimit += Globals.scrollRate * Time.deltaTime;
        }
        else
        {
            Globals.scrollDelay -= Time.deltaTime;
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

        //Update experience
        this.scoreManager.UpdateExperience(this.player.GetExperience());
    }

    private void FixedUpdate()
    {
        if ((int)scoreManager.GetScore() > (this.gameLevel * 10))
        {
            this.gameLevel++;

            if (!this.accelerating)
            {
                this.accCount = 60;
                this.acceleration += 1.0f;
                this.accelerating = true;
            }


            player.Accelerate(this.acceleration);
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

    public void StartRun()
    {
        Globals.gameRunning = true;
        this.accelerating = false;
        this.accCount = 0.0f;
        this.clearGame = true;
        this.scrollDelay = Globals.scrollDelay;

        this.cameraController.ResetCamera();
        this.startScreenPanel.gameObject.SetActive(false);
        this.upgradePanel.gameObject.SetActive(false);
        this.endRunPanel.gameObject.SetActive(false);

        this.scoreManager.gameObject.SetActive(true);
        this.HpText.gameObject.SetActive(true);

        this.scoreManager.ResetScore();
        this.gameLevel = 1;

        Globals.gameRunning = true;
        Globals.playerFiring = false;
        Globals.scrollRate = Globals.startScrollRate;
        Globals.creationLimit = Globals.initialCreationLimit;
        Globals.destructionLimit = Globals.initialDestructionLimit;

        this.waveClock = this.startClock;

        this.player = Instantiate(playerPrefab);
        this.player.Initialize();

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
        Globals.scrollDelay = this.scrollDelay;
        PlayerStats.experience += scoreManager.GetExperience();

        //Need to clear the player, any enemies, projectiles, surfaces, and anything else present

        Destroy(this.player.gameObject);
        foreach (var enemy in FindObjectsOfType<EnemyObject>())
        {
            Destroy(enemy.gameObject);
        }
        foreach (var projectile in FindObjectsOfType<ProjectileObject>())
        {
            Destroy(projectile.gameObject);
        }
        foreach (var surface in FindObjectsOfType<Surface>())
        {
            Destroy(surface.gameObject);
        }
        foreach (var gap in FindObjectsOfType<Gap>())
        {
            Destroy(gap.gameObject);
        }

        this.endRunPanel.DisplayEndRunPanel(scoreManager.GetScore(), scoreManager.GetExperience());

    }

    public void Upgrades()
    {
        this.endRunPanel.gameObject.SetActive(false);
        
        scoreManager.EndRun();

        this.upgradePanel.DisplayUpgradePanel();
    }

    public void Reload()
    {
        //Need to autosave the player's progress
        //this.player.SavePlayer();

        this.endRunPanel.gameObject.SetActive(false);

        //Need to set scrolling back to default start scroll rate
        Globals.scrollRate = Globals.startScrollRate;

        this.StartRun();
    }

    public void Exit()
    {
        //Need to auto-save. When the game starts it ill give the option of continue or new game
        //this.player.SavePlayer();

        Application.Quit();
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
