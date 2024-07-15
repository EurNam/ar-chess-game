using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Bishop : Piece
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

            // Find possible moves: Diagonal up right
            for (int x = 0; x <= 8; x++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x+x, currentBoardPosition.y+x);
                if (newBoardPosition.x<=8 && newBoardPosition.y<=8){
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

            // Find possible moves: Diagonal up left
            for (int x = 0; x <= 8; x++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x-x, currentBoardPosition.y+x);
                if (newBoardPosition.x>=1 && newBoardPosition.y<=8){
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

            // Find possible moves: Diagonal down right
            for (int y = 0; y <= 8; y++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x+y, currentBoardPosition.y-y);
                if (newBoardPosition.x<=8 && newBoardPosition.y>=1){
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

            // Find possible moves: Diagonal down left
            for (int y = 0; y <= 8; y++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x-y, currentBoardPosition.y-y);
                if (newBoardPosition.x>=1 && newBoardPosition.y>=1){
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