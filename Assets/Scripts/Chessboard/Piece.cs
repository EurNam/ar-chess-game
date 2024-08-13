using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using JKTechnologies.SeensioGo.Scene;

using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Piece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IGameRPC
    {
        #region Variables
        public int pieceIndex;
        public Piece piece;
        public bool isWhite;
        public GameObject boardParent;
        public GameObject currentSkin;
        public GameObject[] piecePrefabs;
        public Material[] pieceMaterials;
        private Tile currentTile;
        private Tile nearestTile;
        private Vector3 mousePosition;
        private Plane dragPlane;
        private Tile[] tiles;
        private bool isDragging = false;
        private Vector3 new3DPosition;
        private bool firstMove = true;
        private bool isKing = false;
        private List<Vector2Int> possibleMoves = new List<Vector2Int>();
        private Vector2Int initialBoardPosition;
        protected bool usingMouse = false;
        protected bool usingVirtualMouse = false;
        #endregion

        #region Unity Methods
        void Awake()
        {
            piece = this;
            this.SetFirstMove(true);
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // IGameRoomManager.Instance.RPC_RegisterToGameRoom(this);
            currentSkin = Instantiate(piecePrefabs[0], transform);
            currentSkin.transform.localPosition = Vector3.zero;
            currentSkin.transform.localRotation = Quaternion.identity;
            currentSkin.transform.localScale = Vector3.one;

            if (this.colorWhite())
            {
                this.SetPieceMaterial(0);
            }
            else
            {
                this.SetPieceMaterial(1);
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (isDragging && ARChessGameSettings.Instance.GetBoardInitialized())
            {
                if (usingVirtualMouse)
                {
                    CustomDrag();
                }
                if (usingMouse)
                {
                    DragPieceMouse();
                }
            }
        }
        #endregion

        #region Get, Set Methods
        public List<Vector2Int> GetPossibleMoves()
        {
            return possibleMoves;
        }

        public Tile GetCurrentTile()
        {
            return currentTile;
        }

        public Tile GetNearestTile()
        {
            return nearestTile;
        }

        public bool isKingPiece()
        {
            return isKing;
        }

        public bool isFirstMove()
        {
            return firstMove;
        }

        public bool colorWhite()
        {
            return isWhite;
        }

        public Vector2Int GetInitialBoardPosition()
        {
            return initialBoardPosition;
        }

        public Tile[] GetTiles()
        {
            return tiles;
        }

        public int GetPieceIndex()
        {
            return pieceIndex;
        }

        public void SetPossibleMoves(List<Vector2Int> moves)
        {
            possibleMoves = moves;
        }

        public void SetCurrentTile(Tile tile)
        {
            currentTile = tile;
        }

        public void SetNearestTile(Tile tile)
        {
            nearestTile = tile;
        }

        public void SetKing(bool king)
        {
            isKing = king;
        }

        public void SetFirstMove(bool move)
        {
            firstMove = move;
        }

        public void SetWhite(bool white)
        {
            isWhite = white;
        }

        public void SetInitialBoardPosition(Vector2Int position)
        {
            initialBoardPosition = position;
        }

        public void SetTiles(Tile[] tiles)
        {
            this.tiles = tiles;
        }

        public void SetPieceIndex(int index)
        {
            pieceIndex = index;
        }

        public void SetPieceMaterial(int materialIndex)
        {
            Material newMaterial = pieceMaterials[materialIndex];
            Renderer[] renderers = currentSkin.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material = newMaterial;
            }
        }
        #endregion

        #region Generate Moves
        protected virtual void GeneratePossibleMoves(Vector2Int currentBoardPosition)
        {
            possibleMoves.Clear();
        }

        public void GeneratePossibleMovesForBoard(Vector2Int currentBoardPosition)
        {
            this.GeneratePossibleMoves(currentBoardPosition);
        }

        private void FilterMovesToAvoidCheck(bool afterMove)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();

            foreach (Vector2Int move in possibleMoves)
            {
                Tile targetTile = BoardManager.Instance.GetTile(move);
                Piece originalPiece = targetTile.GetPiece();
                targetTile.SetOccupied(true, this);
                currentTile.SetOccupied(false);
                BoardManager.Instance.UpdateBoardState(currentTile.GetBoardIndex(), move, this, false);

                if (!BoardManager.Instance.IsKingChecked(this.colorWhite()))
                {
                    validMoves.Add(move);
                }

                // Revert the move
                targetTile.SetOccupied(true, originalPiece);
                currentTile.SetOccupied(true, this);
                BoardManager.Instance.UpdateBoardState(move, currentTile.GetBoardIndex(), this, false);
            }

            possibleMoves = validMoves;

            if (afterMove)
            {
                foreach (Vector2Int move in possibleMoves)
                {
                    if (BoardManager.Instance.GetTile(move).GetPiece() != null)
                    {
                        BoardManager.Instance.ShowMoveGuides(move, BoardManager.MoveType.Capture);
                    } else {
                        BoardManager.Instance.ShowMoveGuides(move, BoardManager.MoveType.Allowed);
                    }
                }
                BoardManager.Instance.ShowMoveGuides(this.GetCurrentTile().GetBoardIndex(), BoardManager.MoveType.Stay);
            }
        }
        #endregion

        #region Find Tile
        public void FindCurrentTile()
        {
            tiles = FindObjectsOfType<Tile>();
            float minDistance = float.MaxValue;
            Tile startingNearestTile = null;

            foreach (Tile tile in tiles)
            {
                if (tile != null && tile.GetBoardIndex().x != 0 && tile.GetBoardIndex().y != 0 && tile.gameObject.activeSelf)
                {
                    float distance = Vector3.Distance(transform.position, tile.GetPosition3D());
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        startingNearestTile = tile;
                    }
                }
            }

            if (startingNearestTile != null)
            {
                currentTile = startingNearestTile;
                nearestTile = startingNearestTile;
                currentTile.SetOccupied(true, this);
            }
            initialBoardPosition = currentTile.GetBoardIndex();
        }

        public Tile FindNearestTile(bool actualMove)
        {
            //tiles = FindObjectsOfType<Tile>();
            float minDistance = float.MaxValue;
            Tile tempNearestTile = null;

            // Find the closest tile to the piece position to snap the piece to
            foreach (Tile tile in tiles)
            {
                if (tile != null && tile.GetBoardIndex().x != 0 && tile.GetBoardIndex().y != 0 && tile.gameObject.activeSelf)
                {
                    float distance = Vector3.Distance(this.transform.position, tile.GetPosition3D());
                    // Update the closest tile if the distance is less than the current closest tile
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        tempNearestTile = tile;
                    }
                }
            }

            if (actualMove)
            {
                // Check if the closest tile is a valid move and if the distance is less than 1.5 units
                if (possibleMoves.Contains(tempNearestTile.GetBoardIndex()) && minDistance < 1.3f*boardParent.transform.localScale.x)
                {
                    return tempNearestTile;
                }
                return currentTile;   
            }
            else
            {
                // if (currentTile!=tempNearestTile)
                // {
                //     Debug.Log(this.name + " is moved to a new tile by the opponent.");
                // }
                return tempNearestTile;
            }
        }

        #endregion

        #region User Interaction
        private void HandleClickDown()
        {
            Debug.Log(ARChessGameSettings.Instance.GetBoardInitialized());
            if (BoardManager.Instance.GetWhiteTurn() == this.colorWhite() && ARChessGameSettings.Instance.GetBoardInitialized() && GameManager.Instance.GetWhitePlayer() == this.colorWhite() && ARChessGameSettings.Instance.GetGameStarted()) 
            {
                // Set the piece to be dragged
                isDragging = true;
                // Set the plane to be the piece
                dragPlane = new Plane(boardParent.transform.up, transform.position);
                // Set the mouse position to be the piece
                mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                // Generate the possible moves for the piece
                GeneratePossibleMoves(currentTile.GetBoardIndex());
                FilterMovesToAvoidCheck(true);
                if (SystemInfo.supportsVibration)
                {
                    ISeensioGoSceneUtilities.Instance?.VibratePop();
                    //Vibration.Vibrate(100);
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleClickDown();
            usingVirtualMouse = true;
        }

        public void OnMouseDown()
        {
            HandleClickDown();
            usingMouse = true;
        }

        public void CustomDrag()
        {
            Vector3 cursorPosition = VirtualMouse.Instance.GetCursorPosition();

            Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
            float distance;
            // Allow the piece to be dragged on the board
            if (dragPlane.Raycast(ray, out distance))
            {
                new3DPosition = ray.GetPoint(distance);
                // Interpolate the position to create a dragging effect
                transform.position = Vector3.Lerp(transform.position, new3DPosition, 0.1f); 
            }

            // Find the nearest tile to the piece
            Tile newNearestTile = FindNearestTile(true);
            // If the nearest tile has changed, update the move guide colors
            if (newNearestTile != nearestTile)
            {
                if (nearestTile != currentTile)
                {
                    nearestTile.SetMoveGuideColor(Color.green);
                }
                newNearestTile.SetMoveGuideColor(Color.yellow);
                nearestTile = newNearestTile;
                // Debug.Log("Nearest tile is " + nearestTile.GetBoardIndex());
            }
        }

        private void DragPieceMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            // Allow the piece to be dragged on the board
            if (dragPlane.Raycast(ray, out distance))
            {
                new3DPosition = ray.GetPoint(distance);
                transform.position = new3DPosition;
            }

            // Find the nearest tile to the piece
            Tile newNearestTile = FindNearestTile(true);
            // If the nearest tile has changed, update the move guide colors
            if (newNearestTile != nearestTile)
            {
                if (nearestTile != currentTile)
                {
                    nearestTile.SetMoveGuideColor(Color.green);
                }
                newNearestTile.SetMoveGuideColor(Color.yellow);
                nearestTile = newNearestTile;
            }
        }

        protected async Task HandleClickUp(){
            if (ARChessGameSettings.Instance.GetBoardInitialized() && isDragging)
            {
                isDragging = false;
                await SnapToNearestTile(true);
            }
        }

        public async virtual void OnPointerUp(PointerEventData eventData)
        {
            await HandleClickUp();
            usingVirtualMouse = false;
        }

        public async virtual void OnMouseUp()
        {
            await HandleClickUp();
            usingMouse = false;
        }
        #endregion

        #region Snap, Update Piece
        public async Task SnapToNearestTile(bool afterMove)
        {
            Tile tempCurrentTile = currentTile;
            Tile tempNearestTile = nearestTile;

            // Check if the closest tile has a piece and if it is not the same piece
            if (nearestTile.GetPiece() != null && nearestTile.GetPiece() != this)
            {
                // If it does, destroy the piece and set the tile to be empty
                Piece capturedPiece = nearestTile.GetPiece();
                //Debug.Log("Captured piece: " + capturedPiece.name);
                try
                {
                    IGameRoomManager.Instance.RPC_ScatterActionToRoom(capturedPiece, "Captured"); 
                }
                catch (Exception e)
                {
                    capturedPiece.Captured();
                }
            }

            // Handle En Passant
            handleEnPassant(nearestTile);

            // Handle Rook in castling
            if (this.isKingPiece() && this.isFirstMove() && this.isCastle(nearestTile))
            {
                handleCastling(nearestTile);
                this.SetFirstMove(false);
            }


            // Move piece to new tile
            if (afterMove)
            {
                float localTileHeight = nearestTile.transform.localScale.y;
                Vector3 localPosition = nearestTile.transform.localPosition + new Vector3(0, localTileHeight, 0);

                Tile convertPoint = BoardManager.Instance.GetTile(new Vector2Int(0, 0));
                convertPoint.transform.localPosition = localPosition;

                transform.position = convertPoint.transform.position;
            }

            currentTile.SetOccupied(false);
            currentTile = nearestTile;
            currentTile.SetOccupied(true, this);
            //Debug.Log(this.name + " is moved to " + currentTile.name);

            // Update the board state after move
            if (tempNearestTile != tempCurrentTile){
                this.SetFirstMove(false);
                BoardManager.Instance.IncrementMoveCount();
                BoardManager.Instance.UpdateBoardState(tempCurrentTile.GetBoardIndex(), tempNearestTile.GetBoardIndex(), this, true);
                BoardManager.Instance.CheckForCheckmate(!this.colorWhite());
                Debug.Log("Move count: " + BoardManager.Instance.GetMoveCount());
                if (BoardManager.Instance.GetMoveCount() == 0)
                {
                    BoardManager.Instance.HideMoveGuides();
                }
                await Task.Delay(100);
                //Debug.Log("Wait");
                try
                {
                    IGameRoomManager.Instance.RPC_ScatterActionToRoom(this, "Moved");
                }
                catch (Exception e)
                {
                    this.Moved();
                }
                GameManager.Instance.SwitchRoomTurn();
            } else {
                BoardManager.Instance.HideMoveGuides();
                BoardManager.Instance.CheckForCheckmate(this.colorWhite());
            }
        } 

        public void UpdatePiecePositionInfo(bool afterMove)
        {
            Tile tempCurrentTile = currentTile;
            Tile tempNearestTile = nearestTile;

            // Update the board state after move
            if (tempNearestTile != tempCurrentTile){
                currentTile.SetOccupied(false);
                currentTile = nearestTile;
                currentTile.SetOccupied(true, this);
                this.SetFirstMove(false);

                if (afterMove)
                {
                    BoardManager.Instance.IncrementMoveCount();
                }

                BoardManager.Instance.UpdateBoardState(tempCurrentTile.GetBoardIndex(), tempNearestTile.GetBoardIndex(), this, true);
                BoardManager.Instance.CheckForCheckmate(!this.colorWhite());
                //Debug.Log("Move count: " + BoardManager.Instance.GetMoveCount());
                if (BoardManager.Instance.GetMoveCount() == 0)
                {
                    BoardManager.Instance.HideMoveGuides();
                }
                // Debug.Log(this.name + " is moved from " + tempCurrentTile.GetBoardIndex() + " to " + tempNearestTile.GetBoardIndex() + " by opponent.");
            } 
        }

        public void Moved()
        {
            currentTile.SetOccupied(false);
            currentTile = nearestTile;
            currentTile.SetOccupied(true, this);
            //Debug.Log(this.name + " is moved to " + currentTile.name);
            this.SetNearestTile(this.FindNearestTile(false));
            //Debug.Log(this.name + " new nearest tile is " + this.nearestTile.name);
            this.UpdatePiecePositionInfo(true);
            BoardManager.Instance.PlaySnapSound();
        }

        public void Captured()
        {
            //Debug.Log(this.name + " is captured by opponent.");
            this.GetCurrentTile().SetOccupied(false);
            this.GetCurrentTile().ShowBloodTemporarily();
            this.GetCurrentTile().ShowDeathTemporarily();
            BoardManager.Instance.PlayCaptureSound();
            int index = this.RPC_GetID()-1;
            IGameRoomManager.Instance.RPC_UnregisterToGameRoom(this);
            this.gameObject.SetActive(false);
            GameManagerBufferData.Instance.SetBufferPieceData(null, index);
        }
        #endregion

        #region Handle Special
        private bool isCastle(Tile nearestTile)
        {
            // Check if the new move is a castle
            List<Vector2Int> castleMoves = new List<Vector2Int>();
            castleMoves.Add(new Vector2Int(7, 1));
            castleMoves.Add(new Vector2Int(3, 1));
            castleMoves.Add(new Vector2Int(7, 8));
            castleMoves.Add(new Vector2Int(3, 8));
            return castleMoves.Contains(nearestTile.GetBoardIndex());
        }

        private void handleCastling(Tile kingNewTile)
        {
            Vector2Int kingPosition = kingNewTile.GetBoardIndex();
            int rookCol = (kingPosition.x == 3) ? 1 : 8; // Determine which rook to move based on king's new position
            int rookNewCol = (kingPosition.x == 3) ? 4 : 6; // New position for the rook

            Tile rookTile = BoardManager.Instance.GetTile(new Vector2Int(rookCol, kingPosition.y));
            if (rookTile.GetPiece() is Rook rook)
            {
                // Move rook to castling position
                Tile newRookTile = BoardManager.Instance.GetTile(new Vector2Int(rookNewCol, kingPosition.y));
                float localTileHeight = rookTile.transform.localScale.y;
                Vector3 localPosition = newRookTile.transform.localPosition + new Vector3(0, localTileHeight, 0);

                Tile convertPoint = BoardManager.Instance.GetTile(new Vector2Int(0, 0));
                convertPoint.transform.localPosition = localPosition;

                rook.transform.position = convertPoint.transform.position;

                rookTile.SetOccupied(false);
                newRookTile.SetOccupied(true, rook);
                rook.SetCurrentTile(newRookTile);
                rook.SetNearestTile(newRookTile);

                try
                {
                    IGameRoomManager.Instance.RPC_ScatterActionToRoom(rook, "Moved");
                }
                catch (Exception e)
                {
                    rook.Moved();
                }
            }
        }

        private void handleEnPassant(Tile nearestTile)
        {
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
                                // Determine the direction of the pawn that moved two tiles forward right before
                                int yChange = 1;
                                if (BoardManager.Instance.GetBoardIndexLastMove().y > BoardManager.Instance.GetBoardIndexBeforeLastMove().y)
                                {
                                    yChange = -1;
                                }
                                // Perform En Passant
                                if (nearestTile.GetBoardIndex() == new Vector2Int(BoardManager.Instance.GetBoardIndexLastMove().x, BoardManager.Instance.GetBoardIndexLastMove().y+yChange))
                                {
                                    Piece capturedPiece = BoardManager.Instance.GetTile(BoardManager.Instance.GetBoardIndexLastMove()).GetPiece();
                                    try
                                    {
                                        IGameRoomManager.Instance.RPC_ScatterActionToRoom(capturedPiece, "Captured");
                                    }
                                    catch (Exception e)
                                    {
                                        capturedPiece.Captured();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Change Skin
        public void ChangePiecePrefab()
        {
            int prefabIndex = ARChessGameSettings.Instance.GetBoardAppearanceIndex();
            if (prefabIndex < 0 || prefabIndex >= piecePrefabs.Length)
            {
                Debug.LogError("Invalid prefab index");
                return;
            }
            // Destroy current prefab
            if (currentSkin != null)
            {
                Destroy(currentSkin);
            }

            // Instantiate the new prefab
            currentSkin = Instantiate(piecePrefabs[prefabIndex], transform);
            currentSkin.transform.localPosition = Vector3.zero;
            currentSkin.transform.localRotation = Quaternion.identity;
            currentSkin.transform.localScale = Vector3.one;

            // Set side colors accordingly to new prefabs
            int whiteColor = 0;
            int blackColor = 1;
            if (prefabIndex == 1)
            {
                whiteColor = 2;
                blackColor = 3;
            }

            // Set color for white and black pieces
            if (this.colorWhite())
            {
                this.SetPieceMaterial(whiteColor);
            }
            else
            {
                this.SetPieceMaterial(blackColor);
            }
        }
        #endregion

        #region Reset Game
        public void ResetPosition()
        {
            // Reset the piece to its initial position
            transform.position = BoardManager.Instance.GetTile(initialBoardPosition).GetPosition3D() + Vector3.up;
            currentTile.SetOccupied(false);
            currentTile = BoardManager.Instance.GetTile(initialBoardPosition);
            currentTile.SetOccupied(true, this);
            firstMove = true;
        }

        #endregion

        #region Multiplayer
        public void RPC_OnActionReceived(string actionName)
        {
            // Debug.Log("Action received: " + actionName);
            Invoke(actionName, 0.1f);
        }

        public int RPC_GetID()
        {
            return pieceIndex;
        }
        #endregion
    }
}