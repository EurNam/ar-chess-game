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
    public class Stone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IGameRPC
    {
        #region Variables
        public int stoneIndex;
        public bool isWhite;
        public GameObject boardParent;
        public GameObject currentSkin;
        public GameObject[] stonePrefabs;
        public Material[] stoneMaterials;
        private Tile currentTile;
        private Tile nearestTile;
        private Vector3 mousePosition;
        private Plane dragPlane;
        private Tile[] tiles;
        private bool isDragging = false;
        private Vector3 new3DPosition;
        private bool isKing = false;
        private List<Vector2Int> possibleMoves = new List<Vector2Int>();
        private Vector2Int initialBoardPosition;
        protected bool usingMouse = false;
        protected bool usingVirtualMouse = false;
        #endregion

        #region Unity Methods
        void Awake()
        {
            SetKing(false);
        }

        protected virtual void Start()
        {
            currentSkin = Instantiate(stonePrefabs[0], transform);
            currentSkin.transform.localPosition = Vector3.zero;
            currentSkin.transform.localRotation = Quaternion.identity;
            currentSkin.transform.localScale = Vector3.one;

            SetStoneMaterial(isWhite ? 0 : 1);
        }

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
                    DragStoneMouse();
                }
            }
        }
        #endregion

        #region Get, Set Methods
        public List<Vector2Int> GetPossibleMoves() { return possibleMoves; }
        public Tile GetCurrentTile() { return currentTile; }
        public Tile GetNearestTile() { return nearestTile; }
        public bool IsKing() { return isKing; }
        public bool IsWhite() { return isWhite; }
        public Vector2Int GetInitialBoardPosition() { return initialBoardPosition; }
        public Tile[] GetTiles() { return tiles; }
        public int GetStoneIndex() { return stoneIndex; }

        public void SetPossibleMoves(List<Vector2Int> moves) { possibleMoves = moves; }
        public void SetCurrentTile(Tile tile) { currentTile = tile; }
        public void SetNearestTile(Tile tile) { nearestTile = tile; }
        public void SetKing(bool king) { isKing = king; }
        public void SetWhite(bool white) { isWhite = white; }
        public void SetInitialBoardPosition(Vector2Int position) { initialBoardPosition = position; }
        public void SetTiles(Tile[] tiles) { this.tiles = tiles; }
        public void SetStoneIndex(int index) { stoneIndex = index; }

        public void SetStoneMaterial(int materialIndex)
        {
            Material newMaterial = stoneMaterials[materialIndex];
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
            int direction = isWhite ? 1 : -1;
            List<Vector2Int> potentialMoves = new List<Vector2Int>();

            if (!isKing)
            {
                // Regular stone moves
                potentialMoves.Add(new Vector2Int(currentBoardPosition.x + 1, currentBoardPosition.y + direction));
                potentialMoves.Add(new Vector2Int(currentBoardPosition.x - 1, currentBoardPosition.y + direction));

                // Capture moves
                potentialMoves.Add(new Vector2Int(currentBoardPosition.x + 2, currentBoardPosition.y + 2 * direction));
                potentialMoves.Add(new Vector2Int(currentBoardPosition.x - 2, currentBoardPosition.y + 2 * direction));
            }
            else
            {
                // King moves (can move backwards too)
                for (int dx = -1; dx <= 1; dx += 2)
                {
                    for (int dy = -1; dy <= 1; dy += 2)
                    {
                        potentialMoves.Add(new Vector2Int(currentBoardPosition.x + dx, currentBoardPosition.y + dy));
                        potentialMoves.Add(new Vector2Int(currentBoardPosition.x + 2 * dx, currentBoardPosition.y + 2 * dy));
                    }
                }
            }

            foreach (Vector2Int move in potentialMoves)
            {
                if (IsValidMove(currentBoardPosition, move))
                {
                    possibleMoves.Add(move);
                    CheckersBoardManager.Instance.ShowMoveGuides(move, CheckersBoardManager.CheckersMoveType.Allowed);
                }
            }
        }

        private bool IsValidMove(Vector2Int from, Vector2Int to)
        {
            if (to.x < 1 || to.x > 8 || to.y < 1 || to.y > 8)
                return false;

            if (CheckersBoardManager.Instance.GetTile(to).GetOccupiedByStoneState())
                return false;

            if (Mathf.Abs(to.x - from.x) == 1 && CheckersBoardManager.Instance.GetForcedStone() != null)
            {
                return false;
            }

            if (Mathf.Abs(to.x - from.x) == 2)
            {
                Vector2Int capturedPosition = new Vector2Int((from.x + to.x) / 2, (from.y + to.y) / 2);
                Stone capturedStone = CheckersBoardManager.Instance.GetTile(capturedPosition).GetStone();
                if (capturedStone == null || capturedStone.isWhite == this.isWhite)
                    return false;
            }

            return true;
        }

        public void GeneratePossibleMovesForBoard(Vector2Int currentBoardPosition)
        {
            GeneratePossibleMoves(currentBoardPosition);
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
                currentTile.SetOccupiedByStone(true, this);
            }
            initialBoardPosition = currentTile.GetBoardIndex();
        }

        public Tile FindNearestTile(bool actualMove)
        {
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
                return tempNearestTile;
            }
        }

        #endregion

        #region User Interaction
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

        private void HandleClickDown()
        {
            Stone forcedStone = CheckersBoardManager.Instance.GetForcedStone();
            if (forcedStone != null && forcedStone != this)
            {
                // Don't allow moving this stone if there's a forced move
                return;
            }

            if (CheckersBoardManager.Instance.GetWhiteTurn() == this.IsWhite() && ARChessGameSettings.Instance.GetBoardInitialized() && GameManager.Instance.GetWhitePlayer() == this.IsWhite() && ARChessGameSettings.Instance.GetGameStarted())
            {
                isDragging = true;
                dragPlane = new Plane(boardParent.transform.up, transform.position);
                mousePosition = Input.mousePosition - GameManager.Instance.portalController.ARCamera.WorldToScreenPoint(transform.position);
                GeneratePossibleMoves(currentTile.GetBoardIndex());
                if (SystemInfo.supportsVibration)
                {
                    ISeensioGoSceneUtilities.Instance?.VibratePop();
                }
            }
        }

        private void CustomDrag()
        {
            Vector3 cursorPosition = VirtualMouse.Instance.GetCursorPosition();
            Ray ray = GameManager.Instance.portalController.ARCamera.ScreenPointToRay(cursorPosition);
            if (dragPlane.Raycast(ray, out float distance))
            {
                new3DPosition = ray.GetPoint(distance);
                transform.position = Vector3.Lerp(transform.position, new3DPosition, 0.1f);
            }

            Tile newNearestTile = FindNearestTile(true);
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

        private void DragStoneMouse()
        {
            Ray ray = GameManager.Instance.portalController.ARCamera.ScreenPointToRay(Input.mousePosition);
            if (dragPlane.Raycast(ray, out float distance))
            {
                new3DPosition = ray.GetPoint(distance);
                transform.position = new3DPosition;
            }

            Tile newNearestTile = FindNearestTile(true);
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

        public async void OnPointerUp(PointerEventData eventData)
        {
            await HandleClickUp();
            usingVirtualMouse = false;
        }

        public async void OnMouseUp()
        {
            await HandleClickUp();
            usingMouse = false;
        }

        protected async Task HandleClickUp()
        {
            if (ARChessGameSettings.Instance.GetBoardInitialized() && isDragging)
            {
                isDragging = false;
                await SnapToNearestTile(true);
            }
        }
        #endregion

        #region Snap, Update Stone
        public async Task SnapToNearestTile(bool afterMove)
        {
            Tile tempCurrentTile = currentTile;
            Tile tempNearestTile = nearestTile;
            bool isCaptureMove = false;

            if (nearestTile.GetOccupiedByStoneState() && nearestTile.GetStone() != this)
            {
                Debug.LogError("Invalid move in checkers");
                return;
            }

            if (Mathf.Abs(nearestTile.GetBoardIndex().x - currentTile.GetBoardIndex().x) == 2)
            {
                Vector2Int capturedPosition = new Vector2Int(
                    (currentTile.GetBoardIndex().x + nearestTile.GetBoardIndex().x) / 2,
                    (currentTile.GetBoardIndex().y + nearestTile.GetBoardIndex().y) / 2
                );
                Tile capturedTile = CheckersBoardManager.Instance.GetTile(capturedPosition);
                Stone capturedStone = capturedTile.GetStone();
                if (capturedStone != null)
                {
                    capturedTile.SetOccupiedByStone(false, null);
                    try
                    {
                        IGameRoomManager.Instance.RPC_ScatterActionToRoom(capturedStone, "Captured");
                    }
                    catch (Exception)
                    {
                        capturedStone.Captured();
                    }
                }
                isCaptureMove = true;
            }

            if (afterMove)
            {
                float localTileHeight = nearestTile.transform.localScale.y;
                Vector3 localPosition = nearestTile.transform.localPosition + new Vector3(0, localTileHeight*0.6f, 0);
                Tile convertPoint = CheckersBoardManager.Instance.GetTile(new Vector2Int(0, 0));
                convertPoint.transform.localPosition = localPosition;
                transform.position = convertPoint.transform.position;
            }

            currentTile.SetOccupiedByStone(false, null);
            currentTile = nearestTile;
            currentTile.SetOccupiedByStone(true, this);

            if (!isKing && ((isWhite && currentTile.GetBoardIndex().y == 8) || (!isWhite && currentTile.GetBoardIndex().y == 1)))
            {
                PromoteToKing();
            }

            if (tempNearestTile != tempCurrentTile)
            {
                CheckersBoardManager.Instance.IncrementMoveCount();
                CheckersBoardManager.Instance.UpdateBoardState();
                CheckersBoardManager.Instance.CheckForCheckersWin(!this.isWhite);
                try
                {
                    IGameRoomManager.Instance.RPC_ScatterActionToRoom(this, "Moved");
                }
                catch (Exception)
                {
                    this.Moved();
                }

                // Check for additional captures
                if (isCaptureMove)
                {
                    List<Vector2Int> additionalCaptures = CheckersBoardManager.Instance.GetAdditionalCaptures(this, this.isKing);

                    foreach (Vector2Int capture in additionalCaptures)
                    {
                        Debug.Log("Additional capture: " + capture);
                    }

                    // If there are additional captures, make them, else switch turn
                    if (additionalCaptures.Count > 0)
                    {
                        CheckersBoardManager.Instance.SetForcedStone(this);
                    }
                    else
                    {
                        CheckersBoardManager.Instance.SetForcedStone(null);
                        GameManager.Instance.SwitchRoomTurn();
                        Debug.Log("Switching room turn");
                    }
                }
                else
                {
                    CheckersBoardManager.Instance.SetForcedStone(null);
                    GameManager.Instance.SwitchRoomTurn();
                    Debug.Log("Switching room turn");
                }
            }
            else
            {
                CheckersBoardManager.Instance.HideMoveGuides();
            }
        }

        private void PromoteToKing()
        {
            this.isKing = true;
            //SetStoneMaterial(isWhite ? 2 : 3);
        }

        public void Moved()
        {
            currentTile.SetOccupiedByStone(false, null);
            currentTile = nearestTile;
            currentTile.SetOccupiedByStone(true, this);
            this.SetNearestTile(this.FindNearestTile(false));
            this.UpdatePiecePositionInfo(true);
            CheckersBoardManager.Instance.PlaySnapSound();
        }

        public void Captured()
        {
            this.GetCurrentTile().SetOccupiedByStone(false, null);
            this.GetCurrentTile().ShowBloodTemporarily();
            this.GetCurrentTile().ShowDeathTemporarily();
            CheckersBoardManager.Instance.PlayCaptureSound();
            int index = this.RPC_GetID() - 1;
            IGameRoomManager.Instance.RPC_UnregisterToGameRoom(this);
            this.gameObject.SetActive(false);
            //GameManagerBufferData.Instance.SetBufferPieceData(null, index);
        }

        public void UpdatePiecePositionInfo(bool afterMove)
        {
            Tile tempCurrentTile = currentTile;
            Tile tempNearestTile = nearestTile;

            if (tempNearestTile != tempCurrentTile)
            {
                currentTile.SetOccupiedByStone(false, null);
                currentTile = nearestTile;
                currentTile.SetOccupiedByStone(true, this);

                if (afterMove)
                {
                    CheckersBoardManager.Instance.IncrementMoveCount();
                }

                CheckersBoardManager.Instance.UpdateBoardState();
                if (CheckersBoardManager.Instance.GetMoveCount() == 0)
                {
                    CheckersBoardManager.Instance.HideMoveGuides();
                }
            }
        }
        #endregion

        #region Change Skin
        public void ChangeStonePrefab()
        {
            int prefabIndex = ARChessGameSettings.Instance.GetBoardAppearanceIndex();
            if (prefabIndex < 0 || prefabIndex >= stonePrefabs.Length)
            {
                Debug.LogError("Invalid prefab index");
                return;
            }
            if (currentSkin != null)
            {
                Destroy(currentSkin);
            }

            currentSkin = Instantiate(stonePrefabs[prefabIndex], transform);
            currentSkin.transform.localPosition = Vector3.zero;
            currentSkin.transform.localRotation = Quaternion.identity;
            currentSkin.transform.localScale = Vector3.one;

            int whiteColor = 0;
            int blackColor = 1;
            if (prefabIndex == 1)
            {
                whiteColor = 2;
                blackColor = 3;
            }

            SetStoneMaterial(this.IsWhite() ? whiteColor : blackColor);
        }
        #endregion

        #region Reset Game
        public void ResetPosition()
        {
            transform.position = CheckersBoardManager.Instance.GetTile(initialBoardPosition).GetPosition3D() + Vector3.up;
            currentTile.SetOccupiedByStone(false, null);
            currentTile = CheckersBoardManager.Instance.GetTile(initialBoardPosition);
            currentTile.SetOccupiedByStone(true, this);
            isKing = false;
            SetStoneMaterial(isWhite ? 0 : 1);
        }
        #endregion

        #region Multiplayer
        public void RPC_OnActionReceived(string actionName)
        {
            Invoke(actionName, 0.1f);
        }

        public int RPC_GetID()
        {
            return stoneIndex;
        }
        #endregion
    }
}