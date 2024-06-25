using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] EndRunPanel endRunPanel;

    [SerializeField] TextMeshProUGUI jumpForceCost;
    [SerializeField] TextMeshProUGUI jumpForceLevel;
    [SerializeField] TextMeshProUGUI maxJumpsCost;
    [SerializeField] TextMeshProUGUI maxJumpsLevel;
    [SerializeField] TextMeshProUGUI jumpTimeCost;
    [SerializeField] TextMeshProUGUI jumpTimeLevel;
    [SerializeField] TextMeshProUGUI moveForceCost;
    [SerializeField] TextMeshProUGUI moveForceLevel;
    [SerializeField] TextMeshProUGUI maxVelocityCost;
    [SerializeField] TextMeshProUGUI maxVelocityLevel;
    [SerializeField] TextMeshProUGUI hpCost;
    [SerializeField] TextMeshProUGUI hpLevel;
    [SerializeField] TextMeshProUGUI defenseCost;
    [SerializeField] TextMeshProUGUI defenseLevel;

    public void DisplayUpgradePanel()
    {
        //Update the levels of attributes and the cost to upgrade
        this. UpdatePanelLevels();
        this.UpdatePanelCosts();

        this.gameObject.SetActive(true);
    }

    public void CloseUpgradePanel()
    {
        this.gameObject.SetActive(false);
        this.endRunPanel.gameObject.SetActive(true);
    }

    public void UpgradeStat(string stat)
    {
        PlayerStats.Stat pStat;

        Enum.TryParse<PlayerStats.Stat>(stat, out pStat);

        PlayerStats.UpgradeStat(pStat);

        //Update correct text boxes
        switch (pStat)
        {
            case PlayerStats.Stat.JumpForce:
                jumpForceLevel.text = PlayerStats.jumpForceLevel.ToString();
                jumpForceCost.text = (PlayerStats.UpgradeCost(PlayerStats.Stat.JumpForce)).ToString() + "Exp";
                break;
            case PlayerStats.Stat.MaxJumps:
                maxJumpsLevel.text = PlayerStats.maxJumpsLevel.ToString();
                maxJumpsCost.text = (PlayerStats.UpgradeCost(PlayerStats.Stat.MaxJumps)).ToString() + "Exp";
                break;
            case PlayerStats.Stat.JumpTime:
                jumpTimeLevel.text = PlayerStats.jumpTimeLevel.ToString();
                jumpTimeCost.text = (PlayerStats.UpgradeCost(PlayerStats.Stat.JumpTime)).ToString() + "Exp";
                break;
            case PlayerStats.Stat.MoveForce:
                moveForceLevel.text = PlayerStats.moveForceLevel.ToString();
                moveForceCost.text = (PlayerStats.UpgradeCost(PlayerStats.Stat.MoveForce)).ToString() + "Exp";
                break;
            case PlayerStats.Stat.MaxVelocity:
                maxVelocityLevel.text = PlayerStats.maxVelocityLevel.ToString();
                maxVelocityCost.text = (PlayerStats.UpgradeCost(PlayerStats.Stat.MaxVelocity)).ToString() + "Exp";
                break;
            case PlayerStats.Stat.HP:
                hpLevel.text = PlayerStats.hpLevel.ToString();
                hpCost.text = (PlayerStats.UpgradeCost(PlayerStats.Stat.HP)).ToString() + "Exp";
                break;
            case PlayerStats.Stat.Defense:
                defenseLevel.text = PlayerStats.defenseLevel.ToString();
                defenseCost.text = (PlayerStats.UpgradeCost(PlayerStats.Stat.Defense)).ToString() + "Exp";
                break;
            default:
                break;
        }
    }

    private void UpdatePanelLevels()
    {

    }

    private void UpdatePanelCosts()
    {

    }

    private int GetStatLevel(PlayerStats.Stat stat)
    {
        switch (stat)
        {
            default:
                return 0;
        }
    }

    private int GetStatUpgradeCost(PlayerStats.Stat stat)
    {
        switch (stat)
        {
            default:
                return 0;
        }
    }
}
