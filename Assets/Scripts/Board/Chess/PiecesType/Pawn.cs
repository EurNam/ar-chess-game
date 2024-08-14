using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Pawn : Piece
    {
        private bool promoted = false;

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

        protected override bool isPromoted()
        {
            return promoted;
        }

        protected override void GeneratePossibleMoves(Vector2Int currentBoardPosition)
        {
            if (promoted)
            {
                GeneratePossibleMovesForQueen(currentBoardPosition);
            }
            else
            {
                GeneratePossibleMovesForPawn(currentBoardPosition);
            }
        }

        private void GeneratePossibleMovesForPawn(Vector2Int currentBoardPosition)
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

        private void GeneratePossibleMovesForQueen(Vector2Int currentBoardPosition)
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
            Debug.Log("Promoting pawn to queen");
            
            // Set promoted to true
            promoted = true;

            // Change the pawn's skin to the queen's skin
            int prefabIndex = ARChessGameSettings.Instance.GetBoardAppearanceIndex();
            GameObject queenPrefab = prefabIndex == 0 ? BoardManager.Instance.defaultQueenPrefab : BoardManager.Instance.desertQueenPrefab;

            // Destroy current skin
            if (currentSkin != null)
            {
                Destroy(currentSkin);
            }

            // Instantiate the new queen skin
            currentSkin = Instantiate(queenPrefab, transform);
            currentSkin.transform.localPosition = Vector3.zero;
            currentSkin.transform.localRotation = Quaternion.identity;
            currentSkin.transform.localScale = Vector3.one;

            // Set the material for the new queen skin
            int materialIndex = this.colorWhite() ? 0 : 1;
            if (prefabIndex == 1)
            {
                materialIndex = this.colorWhite() ? 2 : 3;
            }
            SetPieceMaterial(materialIndex);

            // Update the board state
            BoardManager.Instance.UpdatePieceInBoardState(this.GetPieceIndex(), this);

            // Generate possible moves for the new queen
            GeneratePossibleMovesForBoard(this.GetCurrentTile().GetBoardIndex());

            Debug.Log("Pawn promoted to queen");
            BoardManager.Instance.GenerateAllPossibleMoves();
            BoardManager.Instance.CheckForCheckmate(!this.colorWhite());
        }
    }
}