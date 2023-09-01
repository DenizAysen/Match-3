using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private float roundTime = 60f;

    private UIManager _uiManager;
    private Board _board;
    private bool endingRound = false;
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
}
