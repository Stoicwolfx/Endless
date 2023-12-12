using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndRunPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;

    public void DisplayEndRunPanel(float score, int exp)
    {
        this.gameObject.SetActive(true);
        m_Text.text = "Score: " + Mathf.RoundToInt(score).ToString() + "\nExp:  " + exp.ToString();
    }
}
