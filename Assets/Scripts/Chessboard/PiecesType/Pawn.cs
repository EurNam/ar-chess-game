using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
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
                if (currentBoardPosition.y+y*isWhite >= 1 && currentBoardPosition.y+y*isWhite <= 8)
                {
                    newBoardPosition = new Vector2Int(currentBoardPosition.x, currentBoardPosition.y+y*isWhite);
                    if (newBoardPosition.y<=8){
                        BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newBoardPosition, this);
                        if (moveType != BoardManager.MoveType.Invalid)
                        {
                            if (!possibleMoves.Contains(newBoardPosition)){
                                possibleMoves.Add(newBoardPosition);
                                BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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
                        BoardManager.Instance.ShowMoveGuides(newBoardPosition, moveType);
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
                    //Debug.Log("Last piece moved is a pawn");
                    // If that pawn moved two tiles forward
                    if (Mathf.Abs(BoardManager.Instance.GetBoardIndexLastMove().y - BoardManager.Instance.GetBoardIndexBeforeLastMove().y) == 2)
                    {
                        //Debug.Log("Previous piece moved 2 tiles forward");
                        // If this pawn is in the same row as that pawn that moved two tiles forward right before
                        if (this.GetCurrentTile().GetBoardIndex().y == BoardManager.Instance.GetBoardIndexLastMove().y)
                        {
                            //Debug.Log("This pawn is in the same row as the pawn that moved two tiles forward right before");
                            // If this pawn is next to the pawn that moved two tiles forward right before
                            if (Mathf.Abs(this.GetCurrentTile().GetBoardIndex().x - BoardManager.Instance.GetBoardIndexLastMove().x) == 1)
                            {
                                //Debug.Log("This pawn is next to the pawn that moved two tiles forward right before");
                                int yChange = 1;
                                if (BoardManager.Instance.GetBoardIndexLastMove().y > BoardManager.Instance.GetBoardIndexBeforeLastMove().y)
                                {
                                    yChange = -1;
                                }

                                Vector2Int enPassant = new Vector2Int(BoardManager.Instance.GetBoardIndexLastMove().x, BoardManager.Instance.GetBoardIndexLastMove().y+yChange);
                                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(enPassant, this, true);
                                possibleMoves.Add(enPassant);
                                BoardManager.Instance.ShowMoveGuides(enPassant, moveType);
                                //Debug.Log("En Passant move is possible");
                            }
                        }
                    }
                }
            }

            this.SetPossibleMoves(possibleMoves);
        }

        public override async void OnMouseUp()
        {
            await HandleClickUp();
            usingMouse = false;
            CheckPromotion();
        }

        public override async void OnPointerUp(PointerEventData eventData)
        {
            await HandleClickUp();
            usingVirtualMouse = false;
            CheckPromotion();
        }

        private void CheckPromotion()
        {
            int promotionRow = this.colorWhite() ? 8 : 1;
            if (this.GetCurrentTile().GetBoardIndex().y == promotionRow)
            {
                Debug.Log("Pawn promotion check");
                IGameRoomManager.Instance?.RPC_ScatterActionToRoom(this, "PromoteToQueen");
            }
        }

        private void PromoteToQueen()
        {
            IGameRoomManager.Instance.RPC_UnregisterToGameRoom(this);
            Debug.Log("Promoting pawn to queen");
            // Instantiate a new queen at the pawn's position
            GameObject queenPrefab = ARChessGameSettings.Instance.GetBoardAppearanceIndex() == 0 ? BoardManager.Instance.defaultQueenPrefab : BoardManager.Instance.desertQueenPrefab;
            GameObject newQueen = Instantiate(queenPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
            if (this.colorWhite())
            {
                newQueen.name = this.name + ": Queen Promotion";
            } else {
                newQueen.name = this.name + ": Queen Promotion";
            }
            Queen queenComponent = newQueen.GetComponent<Queen>();

            // Transfer properties
            queenComponent.SetCurrentTile(this.GetCurrentTile());
            queenComponent.SetNearestTile(this.GetNearestTile());
            queenComponent.SetFirstMove(false);
            queenComponent.SetWhite(this.colorWhite());

            queenComponent.SetKing(this.isKingPiece());
            queenComponent.SetInitialBoardPosition(this.GetInitialBoardPosition());
            queenComponent.boardParent = this.boardParent;
            queenComponent.SetTiles(this.GetTiles());
            queenComponent.SetPieceMaterial(this.colorWhite() ? 0 : 1);
            queenComponent.SetPieceIndex(this.GetPieceIndex());

            // Update the tile to be occupied by the new queen
            Tile currentTile = this.GetCurrentTile();
            currentTile.SetOccupied(false); // Clear the pawn
            currentTile.SetOccupied(true, queenComponent); // Set the new queen

            // Set the current tile to be occupied by the new queen
            BoardManager.Instance.UpdateBoardState(currentTile.GetBoardIndex(), currentTile.GetBoardIndex(), queenComponent, true);

            // Explicitly update the BoardManager's piece array
            BoardManager.Instance.UpdatePieceInBoardState(this.GetPieceIndex(), queenComponent);

            // Generate possible moves for new queen
            queenComponent.GeneratePossibleMovesForBoard(currentTile.GetBoardIndex());

            // Register queen to room
            IGameRoomManager.Instance.RPC_RegisterToGameRoom(queenComponent);

            // Deactivate the pawn
            this.gameObject.SetActive(false);

            Debug.Log("Pawn promoted to queen");
            BoardManager.Instance.GenerateAllPossibleMoves();
            BoardManager.Instance.CheckForCheckmate(!this.colorWhite());
        }
    }
}