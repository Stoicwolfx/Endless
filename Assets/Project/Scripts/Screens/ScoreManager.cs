using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private float score = 0f;
    private int exp = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.gameRunning) return;

        this.score += Time.deltaTime;
        UpdateScore();
    }

    public void ResetScore()
    {
        this.score = 0f;
    }
    private void UpdateScore()
    {
        scoreText.text = "Score: ";
        scoreText.text += Mathf.Round(this.score);
        scoreText.text += "\nExp:    ";
        scoreText.text += this.exp;
    }

    public void UpdateExperience(int exp)
    {
        this.exp = exp;
    }

    public float GetScore()
    {
        return this.score;
    }

    public int GetExperience()
    {
        return this.exp + Mathf.RoundToInt(score / 10.0f);
    }

    public void EndRun()
    {
        this.score = 0.0f;
        this.exp = 0;
    }
}
