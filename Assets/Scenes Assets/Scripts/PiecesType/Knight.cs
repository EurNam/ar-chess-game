using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Knight : Piece
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
            List<Vector2Int> validMoves = new List<Vector2Int>();

            // Find possible moves: Diagonal up right
            possibleMoves.Add(currentBoardPosition);
            possibleMoves.Add(new Vector2Int(currentBoardPosition.x + 1, currentBoardPosition.y + 2));
            possibleMoves.Add(new Vector2Int(currentBoardPosition.x + 2, currentBoardPosition.y + 1));
            possibleMoves.Add(new Vector2Int(currentBoardPosition.x + 2, currentBoardPosition.y - 1));
            possibleMoves.Add(new Vector2Int(currentBoardPosition.x + 1, currentBoardPosition.y - 2));
            possibleMoves.Add(new Vector2Int(currentBoardPosition.x - 1, currentBoardPosition.y - 2));
            possibleMoves.Add(new Vector2Int(currentBoardPosition.x - 2, currentBoardPosition.y - 1));
            possibleMoves.Add(new Vector2Int(currentBoardPosition.x - 2, currentBoardPosition.y + 1));
            possibleMoves.Add(new Vector2Int(currentBoardPosition.x - 1, currentBoardPosition.y + 2));

            foreach (Vector2Int move in possibleMoves)
            {
                if (move.x >= 1 && move.x <= 8 && move.y >= 1 && move.y <= 8){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(move, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!validMoves.Contains(move)){
                            validMoves.Add(move);
                        }
                    } 
                }
            }

            this.SetPossibleMoves(validMoves);
        }
    }
}