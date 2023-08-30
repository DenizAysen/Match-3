using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MatchFinder : MonoBehaviour
{
    private Board board;
    private Gem currentGem, leftGem, rightGem, aboveGem, underGem;
    public List<Gem> currentMatches = new();
    private void Awake()
    {
        board = FindObjectOfType<Board>();
       // Board.findAllMatches += FindAllMatches;
    }
    //private void OnDestroy()
    //{
    //    Board.findAllMatches -= FindAllMatches;
    //}
    public void FindAllMatches()
    {
        currentMatches.Clear();

        for (int x = 0; x < board.GetBoardWidth(); x++)
        {
            for (int y = 0; y < board.GetBoardHeight(); y++)
            {
                currentGem = board.allGems[x, y];
                if (currentGem != null)
                {
                    if (x > 0 && x < board.GetBoardWidth() - 1)
                    {
                        leftGem = board.allGems[x - 1, y];
                        rightGem = board.allGems[x + 1, y];
                        if(leftGem != null && rightGem != null)
                        {
                            if(leftGem.gemType == currentGem.gemType && currentGem.gemType == rightGem.gemType)
                            {
                                currentGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                            }
                        }
                    }
                    if (y > 0 && y < board.GetBoardHeight() - 1)
                    {
                        aboveGem = board.allGems[x, y + 1];
                        underGem = board.allGems[x, y - 1];
                        if (aboveGem != null && underGem != null)
                        {
                            if(aboveGem.gemType == currentGem.gemType && currentGem.gemType == underGem.gemType)
                            {
                                currentGem.isMatched = true;
                                aboveGem.isMatched = true;
                                underGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(aboveGem);
                                currentMatches.Add(underGem);
                            }
                        }
                    }
                }
            }
        }
        if(currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();
        }

    }
}
