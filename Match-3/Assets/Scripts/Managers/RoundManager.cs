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
    public int scoreTarget1,scoreTarget2,scoreTarget3;
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

        _uiManager.ChangeWinScoreText(_currentScore);

        if (_currentScore >= scoreTarget3)
        {
            _uiManager.ChangeWinText(true);
            _uiManager.ActivateStars(3);
        }
        else if (_currentScore >= scoreTarget2)
        {
            _uiManager.ChangeWinText(true);
            _uiManager.ActivateStars(2);
        }
        else if (_currentScore >= scoreTarget1)
        {
            _uiManager.ChangeWinText(true);
            _uiManager.ActivateStars(1);
        }
        else
            _uiManager.ChangeWinText(false);

    }
    public float GetCurrentRoundTime()
    {
        return roundTime;
    }
    public void ChangeScore(int score)
    {
        _currentScore += score;
        StartCoroutine(ScoreTextChangeAnim());
    }
    private IEnumerator ScoreTextChangeAnim()
    {    
        while(displayScore < _currentScore )
        {
            yield return new WaitForSeconds(.1f);
            displayScore = Mathf.Lerp(displayScore, _currentScore, scoreSpeed*Time.deltaTime);
            _uiManager.ChangeScoreText(displayScore);
            if (_currentScore - displayScore < 3f)
                displayScore = _currentScore;
        }
    }
}
