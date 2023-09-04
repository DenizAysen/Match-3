using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
public class Board : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float gemSpeed;
    [SerializeField] private float bombChance = 2f;
    [SerializeField] private float bonusAmount = .5f;

    [Header("Elements")]
    [SerializeField] private GameObject bgTilePrefab;
    public Gem[] gems;
    public Gem[,] allGems;
    public BoardState currentState = BoardState.move;
    [SerializeField] private Gem Bomb;

    int gemToUse;
    private Gem gem;
    private Vector2 pos;
    private GameObject bgTile;
    public MatchFinder matchFinder;
    private float bonusMultiplier;

    private BoardLayout boardLayout;
    private Gem[,] layoutStore;
    //private RoundManager roundManager;
    //public static Action findAllMatches;
    private void Awake()
    {
        matchFinder = FindObjectOfType<MatchFinder>();
        boardLayout = GetComponent<BoardLayout>();
       // roundManager = FindObjectOfType<RoundManager>();
    }
    void Start()
    {
        allGems = new Gem[width, height];

        layoutStore = new Gem[width, height];

        SetupTheBoard();
    }
    private void SetupTheBoard()
    {
        if(boardLayout != null)
        {
            layoutStore = boardLayout.GetLayout();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                pos = new Vector2(x, y);
                bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity, transform);
                bgTile.name = "BG Tile - " + x + ", " + y;

                if(layoutStore[x,y] != null)
                {
                    SpawnGem(new Vector2Int(x, y), layoutStore[x, y]);
                }
                else
                {
                    gemToUse = Random.Range(0, gems.Length);
                    int iterations = 0;
                    while (MatchesAt(new Vector2Int(x, y), gems[gemToUse]) && iterations < 100)
                    {
                        gemToUse = Random.Range(0, gems.Length);
                        iterations++;
                    }

                    SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
                }              
            }
        }
    }
    private void SpawnGem(Vector2Int spawnPos , Gem gemToSpawn)
    {
        if(Random.Range(0f,100f) < bombChance)
        {
            gemToSpawn = Bomb;
        }

        gem = Instantiate(gemToSpawn, new Vector3(spawnPos.x, spawnPos.y + height,0), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = "Gem - " + pos.x + ", " + pos.y;
        allGems[spawnPos.x, spawnPos.y] = gem;

        gem.SetupGem(spawnPos, this);
    }
    private bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
    {
        if(posToCheck.x > 1)
        {
            if(allGems[posToCheck.x-1, posToCheck.y].gemType == gemToCheck.gemType && (allGems[posToCheck.x - 2, posToCheck.y].gemType == gemToCheck.gemType))
            {
                return true;
            }
        }
        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x , posToCheck.y - 1].gemType == gemToCheck.gemType && (allGems[posToCheck.x , posToCheck.y - 2].gemType == gemToCheck.gemType))
            {
                return true;
            }
        }
        return false;
    }
    private void DestroyMatchedGemAt(Vector2Int pos)
    {
        if (allGems[pos.x, pos.y] != null)
        {
            if(allGems[pos.x, pos.y].isMatched)
            {
                if ((allGems[pos.x, pos.y].gemType == GemType.bomb))
                    SfxManager.Instance.PlayExplode();

                else if ((allGems[pos.x, pos.y].gemType == GemType.stone))
                    SfxManager.Instance.PlayStoneBreak();

                else
                    SfxManager.Instance.PlayGemBreak();

                Instantiate(allGems[pos.x, pos.y].destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);

                Destroy(allGems[pos.x, pos.y].gameObject);
                allGems[pos.x, pos.y] = null;
            }
        }
    }
    public void DestroyMatches()
    {
        for (int i = 0; i < matchFinder.currentMatches.Count; i++)
        {
            if(matchFinder.currentMatches[i] != null)
            {
                ScoreCheck(matchFinder.currentMatches[i]);

                DestroyMatchedGemAt(matchFinder.currentMatches[i].posIndex);
            }
        }
        StartCoroutine(DecreaseRowCo());
    }
    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(.2f);
        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(allGems[x,y] == null)
                {
                    nullCounter++;
                }
                else if(nullCounter > 0)
                {
                    allGems[x, y].posIndex.y -= nullCounter;
                    allGems[x, y - nullCounter] = allGems[x, y];
                    allGems[x, y] = null;
                }
            }
            nullCounter = 0;
        }
        StartCoroutine(FillBoardCo());
    }
    private IEnumerator FillBoardCo()
    {
        //Boards empty places are refilled
        yield return new WaitForSeconds(.5f);
        RefillBoard();

        //Then we check the matches again
        yield return new WaitForSeconds(.5f);
        matchFinder.FindAllMatches();

        //if there is a match we destroy the matches again until no match remains
        if(matchFinder.currentMatches.Count > 0)
        {
            bonusMultiplier++;
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        //if there is no match then player can make a new move
        else
        {
            yield return new WaitForSeconds(.5f);
            ChangeBoardState(BoardState.move);
            bonusMultiplier = 0;
        }
    }
    private void RefillBoard()
    {
        //We check empty places of the board
        // When we find the empty place, we fill the place with gem
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(allGems[x,y] == null)
                {
                    int gemToUse = Random.Range(0, gems.Length);

                    SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
                }               
            }
        }
        //
        CheckMisplacedGems();
    }
    private void CheckMisplacedGems()
    {
        //If there is 2 gems in the same place we destry one of them

        List<Gem> foundGems = new();
        foundGems.AddRange(FindObjectsOfType<Gem>());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (foundGems.Contains(allGems[x, y]))
                {
                    foundGems.Remove(allGems[x, y]);
                }
            }
        }

        foreach (Gem gem in foundGems)
        {
            Destroy(gem.gameObject);
        }
    }
    public void ChangeBoardState(BoardState state)
    {
        currentState = state;
    }
    public void ShuffleTheboard()
    {
        if(currentState != BoardState.wait)
        {
            currentState = BoardState.wait;
            List<Gem> gemsFromBoard = new();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gemsFromBoard.Add(allGems[x, y]);
                    allGems[x, y] = null;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gemToUse = Random.Range(0, gemsFromBoard.Count);

                    while (MatchesAt(new Vector2Int(x, y), gemsFromBoard[gemToUse])  && gemsFromBoard.Count > 1)
                    {
                        gemToUse = Random.Range(0, gemsFromBoard.Count);
                    }
                    gemsFromBoard[gemToUse].SetupGem(new Vector2Int(x, y), this);
                    allGems[x, y] = gemsFromBoard[gemToUse];
                    gemsFromBoard.RemoveAt(gemToUse);
                }
            }
            StartCoroutine(FillBoardCo());
        }
    }
    public void ScoreCheck(Gem gemToCheck)
    {
        RoundManager.Instance.ChangeScore(gemToCheck.scoreValue);
        
        if(bonusMultiplier > 0)
        {
            float bonusToAdd = gemToCheck.scoreValue * bonusMultiplier * bonusAmount;
            RoundManager.Instance.ChangeScore(Mathf.RoundToInt(bonusToAdd));
        }
    }
    #region GetMethods
    public int GetBoardWidth()
    {
        return width;
    }
    public int GetBoardHeight()
    {
        return height;
    }
    public float GetGemSpeed()
    {
        return gemSpeed;
    }
    #endregion
}
