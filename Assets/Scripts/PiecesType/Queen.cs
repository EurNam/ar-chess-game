using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Queen : Piece
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
            
            this.SetPossibleMoves(possibleMoves);
        }

        protected override void SwitchToAlternativePrefab()
        {
            string prefabName = this.colorWhite() ? "QueenLight" : "QueenDark";
            GameObject alternativeQueenPrefab = Resources.Load<GameObject>($"Queen/{prefabName}");

            if (alternativeQueenPrefab != null)
            {
                // Instantiate the alternative pawn prefab at the current position
                GameObject newQueen = Instantiate(alternativeQueenPrefab, this.transform.position, Quaternion.identity);
                Queen newQueenComponent = newQueen.GetComponent<Queen>();
                newQueenComponent.SetCurrentTile(this.GetCurrentTile());
                newQueenComponent.SetFirstMove(this.isFirstMove());
                newQueenComponent.SetWhite(this.colorWhite());

                // Set the current tile to be occupied by the new pawn
                this.GetCurrentTile().SetOccupied(true, newQueenComponent);

                // Deactivate the current pawn
                this.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError($"Prefab {prefabName} not found in Resources/Queen/");
            }
        }
    }
}