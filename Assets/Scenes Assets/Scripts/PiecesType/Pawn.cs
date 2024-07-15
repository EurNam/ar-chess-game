using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// namespace JKTechnologies.SeensioGo.ARChess
// {
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

            // Check for En Passant capture
            if (!this.isFirstMove())
            {
                // If last piece moved is a pawn
                if (BoardManager.Instance.GetPieceLastMoved().GetType() == typeof(Pawn))
                {
                    // If that pawn moved two tiles forward
                    if (Mathf.Abs(BoardManager.Instance.GetBoardIndexLastMove().y - BoardManager.Instance.GetBoardIndexBeforeLastMove().y) == 2)
                    {
                        // If this pawn is in the same row as that pawn that moved two tiles forward right before
                        if (this.GetCurrentTile().GetBoardIndex().y == BoardManager.Instance.GetBoardIndexLastMove().y)
                        {
                            // If this pawn is next to the pawn that moved two tiles forward right before
                            if (Mathf.Abs(this.GetCurrentTile().GetBoardIndex().x - BoardManager.Instance.GetBoardIndexLastMove().x) == 1)
                            {
                                Debug.Log("En passant capture allowed");

                                int yChange = 1;
                                if (BoardManager.Instance.GetBoardIndexLastMove().y > BoardManager.Instance.GetBoardIndexBeforeLastMove().y)
                                {
                                    yChange = -1;
                                }

                                Vector2Int enPassant = new Vector2Int(BoardManager.Instance.GetBoardIndexLastMove().x, BoardManager.Instance.GetBoardIndexLastMove().y+yChange);
                                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(enPassant, this, true);
                                possibleMoves.Add(enPassant);
                            }
                        }
                    }
                }
            }

            this.SetPossibleMoves(possibleMoves);
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            CheckPromotion();
        }

        private void CheckPromotion()
        {
            int promotionRow = this.colorWhite() ? 8 : 1;
            if (this.GetCurrentTile().GetBoardIndex().y == promotionRow)
            {
                PromoteToQueen();
            }
        }

        private void PromoteToQueen()
        {
            // Instantiate a new queen at the pawn's position
            GameObject queenPrefab = this.colorWhite() ? BoardManager.Instance.queenWhitePrefab : BoardManager.Instance.queenBlackPrefab;
            GameObject newQueen = Instantiate(queenPrefab, this.transform.position, Quaternion.identity);
            if (this.colorWhite())
            {
                newQueen.name = this.name + ": Queen Promotion";
            } else {
                newQueen.name = this.name + ": Queen Promotion";
            }
            Queen queenComponent = newQueen.GetComponent<Queen>();
            queenComponent.SetCurrentTile(this.GetCurrentTile());
            queenComponent.SetFirstMove(false);
            queenComponent.SetWhite(this.colorWhite());

            // Set the current tile to be occupied by the new queen
            this.GetCurrentTile().SetOccupied(true, queenComponent);

            // Deactivate the pawn
            this.gameObject.SetActive(false);
        }
    }
// }