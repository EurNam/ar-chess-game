using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class King : Piece
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            this.SetKing(true);
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

            // Find possible moves: Diagonal up right by 1
            for (int x = 0; x <= 1; x++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x+x, currentBoardPosition.y+x);
                if (newBoardPosition.x<=8 && newBoardPosition.y<=8){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!possibleMoves.Contains(newBoardPosition)){
                            possibleMoves.Add(newBoardPosition);
                            BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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

            // Find possible moves: Diagonal up left by 1
            for (int x = 0; x <= 1; x++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x-x, currentBoardPosition.y+x);
                if (newBoardPosition.x>=1 && newBoardPosition.y<=8){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!possibleMoves.Contains(newBoardPosition)){
                            possibleMoves.Add(newBoardPosition);
                            BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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

            // Find possible moves: Diagonal down right by 1
            for (int y = 0; y <= 1; y++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x+y, currentBoardPosition.y-y);
                if (newBoardPosition.x<=8 && newBoardPosition.y>=1){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!possibleMoves.Contains(newBoardPosition)){
                            possibleMoves.Add(newBoardPosition);
                            BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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

            // Find possible moves: Diagonal down left by 1
            for (int y = 0; y <= 1; y++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x-y, currentBoardPosition.y-y);
                if (newBoardPosition.x>=1 && newBoardPosition.y>=1){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!possibleMoves.Contains(newBoardPosition)){
                            possibleMoves.Add(newBoardPosition);
                            BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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

            // Find possible moves: Horizontal right by 1
            for (int x = 0; x <= 1; x++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x+x, currentBoardPosition.y);
                if (newBoardPosition.x<=8){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!possibleMoves.Contains(newBoardPosition)){
                            possibleMoves.Add(newBoardPosition);
                            BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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

            // Find possible moves: Horizontal left by 1
            for (int x = 0; x <= 1; x++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x-x, currentBoardPosition.y);
                if (newBoardPosition.x>=1){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!possibleMoves.Contains(newBoardPosition)){
                            possibleMoves.Add(newBoardPosition);
                            BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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

            // Find possible moves: Vertical up by 1
            for (int y = 0; y <= 1; y++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x, currentBoardPosition.y+y);
                if (newBoardPosition.y<=8){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!possibleMoves.Contains(newBoardPosition)){
                            possibleMoves.Add(newBoardPosition);
                            BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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

            // Find possible moves: Vertical down by 1
            for (int y = 0; y <= 1; y++)
            {
                newBoardPosition = new Vector2Int(currentBoardPosition.x, currentBoardPosition.y-y);
                if (newBoardPosition.y>=1){
                    BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                    if (moveType != BoardManager.MoveType.Invalid)
                    {
                        if (!possibleMoves.Contains(newBoardPosition)){
                            possibleMoves.Add(newBoardPosition);
                            BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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
            
            // Handle castling
            if (this.isFirstMove()){
                Vector2Int kingPosition = this.GetCurrentTile().GetBoardIndex();
                List<int> rookCol = new List<int>{1,8};
                int right = 1;


                foreach (int col in rookCol)
                {
                    int direction = (col == 8) ? 1 : -1; // 1 for kingside, -1 for queenside
                    bool isQueenside = (col == 1);

                    // Check if the tiles between the king and rook are empty
                    bool pathClear = true;
                    for (int i = 1; i <= (isQueenside ? 3 : 2); i++)
                    {
                        if (BoardManager.Instance.GetTile(new Vector2Int(kingPosition.x + i * direction, kingPosition.y)).GetPiece() != null)
                        {
                            pathClear = false;
                            break;
                        }
                    }

                // foreach (int col in rookCol)
                // {
                //     if (col == 1){
                //         right = -1;
                //     } else {
                //         right = 1;
                //     }

                //     bool pathClear = true;
                //     for (int i = 1; i <= (isQueenside ? 3 : 2); i++)
                //     {
                //         if (BoardManager.Instance.GetTile(new Vector2Int(kingPosition.x + i * direction, kingPosition.y)).GetPiece() != null)
                //         {
                //             pathClear = false;
                //             break;
                //         }
                //     }
                //   // Check if the tile next to the king is empty
                //   if (BoardManager.Instance.GetTile(new Vector2Int(kingPosition.x+right, kingPosition.y)).GetPiece() == null){

                //       // Check if the tile two tiles next to the king is empty
                //       if (BoardManager.Instance.GetTile(new Vector2Int(kingPosition.x+2*right, kingPosition.y)).GetPiece() == null){

                    if (pathClear)
                    {
                        // Check if the rook hasn't moved
                        Tile rookTile = BoardManager.Instance.GetTile(new Vector2Int(col, kingPosition.y));
                        if (rookTile.GetPiece() is Rook rook && this.colorWhite() == rook.colorWhite() && rook.isFirstMove())
                        {
                            BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(new Vector2Int(kingPosition.x+2*direction, kingPosition.y), this);
                            possibleMoves.Add(new Vector2Int(kingPosition.x+2*direction, kingPosition.y));
                            BoardManager.Instance.ShowMoveGuides(new Vector2Int(kingPosition.x+2*direction, kingPosition.y), moveType);
                        }
                    }
                }
            }

            this.SetPossibleMoves(possibleMoves);
        }
    }
}