using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    #region TextMeshPro
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI winScoreText;
    [SerializeField] private TextMeshProUGUI winScoreTextLabel;
    [SerializeField] private TextMeshProUGUI winText;
    #endregion
    #region GameObject
    [SerializeField] private GameObject roundOverPanel;
    [SerializeField] private GameObject[] stars;
    #endregion
    [SerializeField] private Color loseTextColor;
    void Start()
    {
        
    }
    #region GamePanel
    public void ChangeTimeText(float value)
    {
        timeText.text = value.ToString("0.0") + "s";
    }
    public void ChangeScoreText(float score)
    {
        scoreText.text = score.ToString("0");
    }
    #endregion

    #region EndingPanel
    public void ActivateRoundOverPanel()
    {
        roundOverPanel.SetActive(true);
    }
    public void ChangeWinText(bool win)
    {
        if (win)
            winText.text = "YOU WIN";
        else
        {
            winText.text = "TRY AGAIN";
            ChangeTextColors();
        }

    }
    public void ChangeWinScoreText(int score)
    {
        winScoreText.text = score.ToString();
    }
    public void ActivateStars(int openedStars)
    {
        for (int i = 0; i < openedStars; i++)
        {
            if (i > stars.Length)
                return;
            else
                stars[i].gameObject.SetActive(true);
        }
    }
    private void ChangeTextColors()
    {
        winText.color = loseTextColor;
        winScoreTextLabel.color = loseTextColor;
        winScoreText.color = loseTextColor;
    }
    #endregion 
}
