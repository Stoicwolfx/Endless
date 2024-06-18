using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] EndRunPanel endRunPanel;
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

    public bool UpgradeStat(string stat)
    {
        PlayerStats.Stat pStat;

        Enum.TryParse<PlayerStats.Stat>(stat, out pStat);

        return (PlayerStats.UpgradeStat(pStat));
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
