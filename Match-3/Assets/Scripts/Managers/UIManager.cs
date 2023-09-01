using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject roundOverPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTimeText(float value)
    {
        timeText.text = value.ToString("0.0") + "s";
    }

    public void ActivateRoundOverPanel()
    {
        roundOverPanel.SetActive(true);
    }
}
