using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private float roundTime = 60f;

    private UIManager _uiManager;
    private Board _board;

    private bool endingRound = false;

    private int _currentScore;
    public float displayScore;
    public float scoreSpeed;
    #region Singleton
    public static RoundManager Instance;
    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _board = FindObjectOfType<Board>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    private void Start()
    {
        _uiManager.ChangeScoreText(_currentScore);
    }
    void Update()
    {
        if(roundTime > 0f)
        {
            roundTime -= Time.deltaTime;
            if(roundTime <= 0f)
            {
                roundTime = 0;

                endingRound = true;
            }
        }
        if(endingRound && _board.currentState == BoardState.move)
        {
            WinCheck();
            endingRound = false;
        }

        _uiManager.ChangeTimeText(roundTime);
    }
    private void WinCheck()
    {
        _uiManager.ActivateRoundOverPanel();
    }
    public float GetCurrentRoundTime()
    {
        return roundTime;
    }
    public void ChangeScore(int score)
    {
        _currentScore += score;
        //if (_currentScore < 0)
        //{
        //    _currentScore = 0;
        //}
        _uiManager.ChangeScoreText(_currentScore);
    }
    private IEnumerator ScoreTextChange()
    {
        yield return null;
    }
}
