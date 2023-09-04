using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
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
    private Board theBoard;
    public string levelSelect;
    private void Awake()
    {        
        theBoard = FindObjectOfType<Board>();
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
    public void ShuffleBoard()
    {
        theBoard.ShuffleTheboard();
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
    public void LevelSelectButton()
    {
        SceneManager.LoadScene(levelSelect);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion 
}
