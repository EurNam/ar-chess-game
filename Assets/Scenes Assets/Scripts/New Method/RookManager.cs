using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookManager : PieceManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void GeneratePossibleMoves(Vector2Int currentBoardPosition)
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        Vector2Int newBoardPosition;

        // Horizontal right
        for (int x = 0; x <= 8; x++)
        {
            if (currentBoardPosition.x+x<=8){
                newBoardPosition = new Vector2Int(currentBoardPosition.x+x, currentBoardPosition.y);
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newBoardPosition)){
                        possibleMoves.Add(newBoardPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            break;
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }

        // Horizontal left
        for (int x = 0; x <= 8; x++)
        {
            if (currentBoardPosition.x-x>=1){
                newBoardPosition = new Vector2Int(currentBoardPosition.x-x, currentBoardPosition.y);
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newBoardPosition)){
                        possibleMoves.Add(newBoardPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            break;
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }

        // Vertical up
        for (int y = 0; y <= 8; y++)
        {
            if (currentBoardPosition.y+y<=8){
                newBoardPosition = new Vector2Int(currentBoardPosition.x, currentBoardPosition.y+y);
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newBoardPosition)){
                        possibleMoves.Add(newBoardPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            break;
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }

        // Vertical down
        for (int y = 0; y <= 8; y++)
        {
            if (currentBoardPosition.y-y>=1){
                newBoardPosition = new Vector2Int(currentBoardPosition.x, currentBoardPosition.y-y);
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newBoardPosition)){
                        possibleMoves.Add(newBoardPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            break;
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }
        this.SetPossibleMoves(possibleMoves);
    }
}