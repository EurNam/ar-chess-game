using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
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

        int moveUp = 1;
        if (this.isFirstMove())
        {
            moveUp = 2;
        }

        // Find possible moves: Vertical
        int isWhite = 1;
        if (!this.colorWhite())
        {
            isWhite = -1;
        }
        for (int y = 0; y <= moveUp; y++)
        {
            newBoardPosition = new Vector2Int(currentBoardPosition.x, currentBoardPosition.y+y*isWhite);
            if (newBoardPosition.y<=8){
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newBoardPosition)){
                        possibleMoves.Add(newBoardPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            possibleMoves.Remove(newBoardPosition);
                            BoardManager.Instance.GetTile(newBoardPosition).SetMoveGuideShown(false);
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }

        // Check if diagonal tiles have opponent
        List<int> pawnCapture = new List<int>{-1,1};
        foreach (int x in pawnCapture)
        {
            newBoardPosition = new Vector2Int(currentBoardPosition.x + x, currentBoardPosition.y + 1*isWhite);
            if (newBoardPosition.x >= 1 && newBoardPosition.x <= 8 && newBoardPosition.y >= 1 && newBoardPosition.y <= 8){
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                if (moveType == BoardManager.MoveType.Capture){
                    possibleMoves.Add(newBoardPosition);
                } else {
                    BoardManager.Instance.GetTile(newBoardPosition).SetMoveGuideShown(false);
                }
            }
        }

        this.SetPossibleMoves(possibleMoves);
    }
}