using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Piece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Variables
        public Piece piece;
        public bool isWhite;
        public GameObject boardParent;
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
        private bool usingMouse = false;
        private bool usingVirtualMouse = false;
        #endregion

        #region Unity Methods
        void Awake()
        {
            piece = this;
            this.SetFirstMove(true);
            // boardParent = this.gameObject.GetComponentInParent<GameObject>();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // FindCurrentTile();
            // this.SetFirstMove(true);
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

        public void SetPieceMaterial(int materialIndex)
        {
            Material newMaterial = pieceMaterials[materialIndex];
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material = newMaterial;
            }
        }
        #endregion

        #region Methods
        protected virtual void GeneratePossibleMoves(Vector2Int currentBoardPosition)
        {
            possibleMoves.Clear();
        }

        public void GeneratePossibleMovesForBoard(Vector2Int currentBoardPosition)
        {
            this.GeneratePossibleMoves(currentBoardPosition);
        }

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

        private void HandleClickDown()
        {
            if (BoardManager.Instance.GetWhiteTurn() == this.colorWhite() && ARChessGameSettings.Instance.GetBoardInitialized() && GameManager.Instance.GetWhitePlayer() == this.colorWhite() && ARChessGameSettings.Instance.GetGameStarted()) 
            {
                Debug.Log(this.name + " is being selected");
                // Set the piece to be dragged
                isDragging = true;
                // Set the plane to be the piece
                dragPlane = new Plane(boardParent.transform.up, transform.position);
                // Set the mouse position to be the piece
                mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                // Generate the possible moves for the piece
                GeneratePossibleMoves(currentTile.GetBoardIndex());
                FilterMovesToAvoidCheck();
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
            Tile newNearestTile = FindNearestTile();
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
            Tile newNearestTile = FindNearestTile();
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

        private void HandleClickUp(){
            if (ARChessGameSettings.Instance.GetBoardInitialized() && isDragging)
            {
                isDragging = false;
                SnapToNearestTile(true);
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            HandleClickUp();
            usingVirtualMouse = false;
        }

        public virtual void OnMouseUp()
        {
            HandleClickUp();
            usingMouse = false;
        }

        public void SnapToNearestTile(bool afterMove)
        {
            Debug.Log("Snap to nearest tile");
            Tile tempCurrentTile = currentTile;
            Tile tempNearestTile = nearestTile;

            // Check if the closest tile has a piece and if it is not the same piece
            if (nearestTile.GetPiece() != null && nearestTile.GetPiece() != this)
            {
                // If it does, destroy the piece and set the tile to be empty
                nearestTile.GetPiece().gameObject.SetActive(false);
                nearestTile.SetOccupied(false);
                BoardManager.Instance.PlayCaptureSound();
            } else {
                if (afterMove)
                {
                    BoardManager.Instance.PlaySnapSound();
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
            float localTileHeight = nearestTile.transform.localScale.y;
            Vector3 localPosition = nearestTile.transform.localPosition + new Vector3(0, localTileHeight, 0);

            Tile convertPoint = BoardManager.Instance.GetTile(new Vector2Int(0, 0));
            convertPoint.transform.localPosition = localPosition;

            transform.position = convertPoint.transform.position;

            currentTile.SetOccupied(false);
            currentTile = nearestTile;
            currentTile.SetOccupied(true, this);

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
                GameManager.Instance.SwitchRoomTurn();
            } else {
                BoardManager.Instance.HideMoveGuides();
                BoardManager.Instance.CheckForCheckmate(this.colorWhite());
            }
        } 

        // public void botMovePiece(Tile newTile)
        // {
        //     // Check if the closest tile has a piece and if it is not the same piece
        //     if (newTile.GetPiece() != null && newTile.GetPiece() != this)
        //     {
        //         // If it does, destroy the piece and set the tile to be empty
        //         newTile.GetPiece().gameObject.SetActive(false);
        //         newTile.SetOccupied(false);
        //         BoardManager.Instance.PlayCaptureSound();
        //     } else {
        //         BoardManager.Instance.PlaySnapSound();
        //     }

        //     // Handle En Passant
        //     handleEnPassant(newTile);

        //     // Handle Rook in castling
        //     if (this.isKingPiece() && this.isFirstMove() && this.isCastle(newTile))
        //     {
        //         handleCastling(newTile);
        //         this.SetFirstMove(false);
        //     }

        //     // Move piece to new tile
        //     float tileHeight = newTile.GetComponent<Renderer>().bounds.size.y;
        //     transform.position = newTile.GetPosition3D() + new Vector3(0, tileHeight, 0);
        //     newTile.SetOccupied(false);
        //     newTile = newTile;
        //     currentTile.SetOccupied(true, this);

        //     // Update the board state after move
        //     if (tempNearestTile != tempCurrentTile){
        //         this.SetFirstMove(false);
        //         BoardManager.Instance.SetWhiteTurn();
        //         BoardManager.Instance.IncrementMoveCount();
        //         BoardManager.Instance.UpdateBoardState(tempCurrentTile.GetBoardIndex(), tempNearestTile.GetBoardIndex(), this, true);
        //         BoardManager.Instance.CheckForCheckmate();
        //         if (BoardManager.Instance.GetMoveCount() == 0)
        //         {
        //             BoardManager.Instance.HideMoveGuides();
        //         }
        //     } else {
        //         BoardManager.Instance.HideMoveGuides();
        //         BoardManager.Instance.CheckForCheckmate();
        //     }
        // }

        public Tile FindNearestTile()
        {
            //tiles = FindObjectsOfType<Tile>();
            float minDistance = float.MaxValue;
            Tile nearestTile = null;

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
                        nearestTile = tile;
                    }
                }
            }

            // Check if the closest tile is a valid move and if the distance is less than 1.5 units
            if (possibleMoves.Contains(nearestTile.GetBoardIndex()) && minDistance < 1.3f*boardParent.transform.localScale.x)
            {
                return nearestTile;
            }

            return currentTile;
        }

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
                                    BoardManager.Instance.GetTile(BoardManager.Instance.GetBoardIndexLastMove()).GetPiece().gameObject.SetActive(false);
                                    BoardManager.Instance.GetTile(BoardManager.Instance.GetBoardIndexLastMove()).SetOccupied(false);
                                    BoardManager.Instance.PlayCaptureSound();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FilterMovesToAvoidCheck()
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

        public void ChangePiecePrefab(int prefabIndex)
        {
            if (prefabIndex < 0 || prefabIndex >= piecePrefabs.Length)
            {
                Debug.LogError("Invalid prefab index");
                return;
            }

            // Instantiate the new prefab
            GameObject newPiece = Instantiate(piecePrefabs[prefabIndex], transform.position, transform.rotation, transform.parent);

            // Copy data from the current piece to the new piece
            Piece newPieceComponent = newPiece.GetComponent<Piece>();
            newPieceComponent.gameObject.name = this.gameObject.name;
            newPieceComponent.SetCurrentTile(this.currentTile);
            newPieceComponent.SetNearestTile(this.nearestTile);
            newPieceComponent.SetFirstMove(this.isFirstMove());
            newPieceComponent.SetWhite(this.colorWhite());
            newPieceComponent.isKing = this.isKingPiece();
            newPieceComponent.possibleMoves = this.possibleMoves;
            newPieceComponent.initialBoardPosition = this.initialBoardPosition;
            newPieceComponent.boardParent = this.boardParent;

            // if (this.piece != null)
            // {
            //     this.piece.FindCurrentTile();
            // }

            if (this.colorWhite())
            {
                newPieceComponent.SetPieceMaterial(0);
            }
            else
            {
                newPieceComponent.SetPieceMaterial(1);
            }



            // Destroy the current piece
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

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
    }
}