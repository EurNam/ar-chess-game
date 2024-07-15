using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Rook : Piece
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

            // Find possible moves: Horizontal right
            for (int x = 0; x <= 8; x++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x+x, currentBoardPosition.y);
                if (newBoardPosition.x<=8){
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

            // Find possible moves: Horizontal left
            for (int x = 0; x <= 8; x++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x-x, currentBoardPosition.y);
                if (newBoardPosition.x>=1){
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

            // Find possible moves: Vertical up
            for (int y = 0; y <= 8; y++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x, currentBoardPosition.y+y);
                if (newBoardPosition.y<=8){
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

            // Find possible moves: Vertical down
            for (int y = 0; y <= 8; y++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x, currentBoardPosition.y-y);
                if (newBoardPosition.y>=1){
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
}