using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;
    //[HideInInspector]
    public Board board;

    private Vector3 firstTouchPos, finalTouchPos;

    private bool mousePressed;
    private float swipeAngel = 0;

    private Gem otherGem;

    public GemType gemType;
    public bool isMatched;
    private Vector2Int previousPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,posIndex) >.01f)
            transform.position = Vector2.Lerp(transform.position, posIndex, board.GetGemSpeed() * Time.deltaTime);
        else
        {
            transform.position = new Vector3(posIndex.x,posIndex.y,0f);
            board.allGems[posIndex.x, posIndex.y] = this;
        }

        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;

            if(board.currentState == BoardState.move)
            {
                finalTouchPos = Input.mousePosition;
                CalculateAngle();
            }          
        }
    }
    public void SetupGem(Vector2Int pos , Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }
    private void OnMouseDown()
    {
        if(board.currentState == BoardState.move)
        {
            firstTouchPos = Input.mousePosition;
            mousePressed = true;
        }     
    }
    private void CalculateAngle()
    {
        swipeAngel = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x);
        swipeAngel *= Mathf.Rad2Deg;
        Debug.Log("Angel : " + swipeAngel);

        if(Vector3.Distance(firstTouchPos,finalTouchPos) > 5f)
            MovePieces();
    }
    private void MovePieces()
    {
        previousPos = posIndex;
        if (swipeAngel < 45 && swipeAngel > -45 && posIndex.x < board.GetBoardWidth() - 1)
        {
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            otherGem.posIndex.x--;
            posIndex.x++;
        }
        else if (swipeAngel > 45 && swipeAngel <= 135 && posIndex.y < board.GetBoardHeight() - 1)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            otherGem.posIndex.y--;
            posIndex.y++;
        }
        else if (swipeAngel < -45 && swipeAngel >= -135 && posIndex.y > 0)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            otherGem.posIndex.y++;
            posIndex.y--;
        }
        else if (swipeAngel > 135 || swipeAngel < -135 && posIndex.x > 0)
        {
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            otherGem.posIndex.x++;
            posIndex.x--;
        }
        board.allGems[posIndex.x, posIndex.y] = this;
        board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;

        StartCoroutine(CheckMoveCor());
    }
    public IEnumerator CheckMoveCor()
    {
        board.ChangeBoardState(BoardState.wait);

        yield return new WaitForSeconds(.5f);

        board.matchFinder.FindAllMatches();

        if(otherGem != null)
        {
            // if there is no match gems return their orginal positions and player will be able to make a move again
            if (!isMatched && !otherGem.isMatched)
            {
                otherGem.posIndex = posIndex;
                posIndex = previousPos;

                board.allGems[posIndex.x, posIndex.y] = this;
                board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;

                yield return new WaitForSeconds(.5f);

                board.ChangeBoardState(BoardState.move);

                Debug.Log("Bu noktaya gelindi");
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }
}