using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class CheckersBoardManager : MonoBehaviour
    {
        #region Variables
        public static CheckersBoardManager Instance;
        public GameObject board;
        public GameObject kingPrefab;
        private Tile[,] boardState;
        private Stone[] boardStateStones;
        private bool whiteTurn = true;
        private int moveCount = 0;
        private Stone forcedStone;
        public AudioClip snapSound; 
        public AudioClip captureSound;
        public AudioSource audioSource;

        public enum CheckersMoveType
        {
            Invalid,
            Allowed,
            Capture,
            Stay
        }
        #endregion

        #region Unity Methods
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {
            InitializeBoard();
        }
        #endregion

        #region Getters, Setters
        public Tile GetTile(Vector2Int boardPosition)
        {
            return boardState[boardPosition.x, boardPosition.y];
        }

        public bool GetWhiteTurn()
        {
            return whiteTurn;
        }

        public int GetMoveCount()
        {
            return moveCount;
        }

        public void SetWhiteTurn()
        {
            whiteTurn = !whiteTurn;
            // Debug.Log("White turn: " + whiteTurn);
        }

        public void IncrementMoveCount()
        {
            moveCount++;
        }

        public void UpdateBoardStateStones()
        {
            boardStateStones = FindObjectsOfType<Stone>();
        }
        #endregion

        #region Board Initialization
        private void InitializeBoard()
        {
            boardState = new Tile[9, 9];

            // Find all tiles with the "Tile" tag
            Tile[] tileObjects = FindObjectsOfType<Tile>();
            foreach (Tile tileObject in tileObjects)
            {
                if (tileObject != null && tileObject.gameObject.activeSelf)
                {
                    Vector2Int boardIndex = tileObject.GetBoardIndex();
                    boardState[boardIndex.x, boardIndex.y] = tileObject;
                }
            }

            ARChessGameSettings.Instance.SetBoardInitialized(true);

            boardStateStones = FindObjectsOfType<Stone>();
            foreach (Stone stone in boardStateStones)
            {
                stone.FindCurrentTile();
                IGameRoomManager.Instance.RPC_RegisterToGameRoom(stone);
            }

            this.GenerateAllPossibleMoves();
            this.HideMoveGuides();
        }

        public void GenerateAllPossibleMoves()
        {
            foreach (Tile tile in boardState)
            {
                if (tile != null && tile.GetBoardIndex().x != 0 && tile.GetBoardIndex().y != 0)
                {
                    if (tile.GetStone() != null)
                    {
                        tile.GetStone().GeneratePossibleMovesForBoard(tile.GetBoardIndex());
                    }
                }
            }
        }

        public void SetBoardSkin()
        {
            foreach (Tile tile in boardState)
            {
                if (tile != null && tile.GetBoardIndex().x != 0 && tile.GetBoardIndex().y != 0)
                {
                    tile.ChangeTilePrefab();
                    if (tile.GetStone() != null)
                    {
                        tile.GetStone().ChangeStonePrefab();
                    }
                }
            }
            ARChessGameSettings.Instance.SetBoardInitialized(true);
        }

        public void SetTileMaterial(int tileColorIndex, int player)
        {
            foreach (Tile tile in boardState)
            {
                if (tile != null && tile.GetBoardIndex().x != 0 && tile.GetBoardIndex().y != 0)
                {
                    if ((tile.GetBoardIndex().x + tile.GetBoardIndex().y) % 2 == player)
                    {
                        tile.SetTileMaterial(tileColorIndex);
                    }
                }
            }
        }
        #endregion

        #region Board Update
        public void UpdateBoardState()
        {
            GenerateAllPossibleMoves();
            HideMoveGuides();
        }
        #endregion

        #region Game Logic
        public void SetForcedStone(Stone stone)
        {
            forcedStone = stone;
        }

        public Stone GetForcedStone()
        {
            return forcedStone;
        }

        public List<Vector2Int> GetAdditionalCaptures(Stone stone, bool isKing)
        {
            List<Vector2Int> captures = new List<Vector2Int>();
            Vector2Int currentPos = stone.GetCurrentTile().GetBoardIndex();

            // Check two diagonal directions for captures
            CheckCapture(stone, currentPos, 2, 2, captures);
            CheckCapture(stone, currentPos, -2, 2, captures);

            // If the stone is a king, check the other two diagonal directions
            if (isKing)
            {
                CheckCapture(stone, currentPos, 2, -2, captures);
                CheckCapture(stone, currentPos, -2, -2, captures);
            }

            return captures;
        }

        private void CheckCapture(Stone stone, Vector2Int currentPos, int dx, int dy, List<Vector2Int> captures)
        {
            Vector2Int newPos = currentPos + new Vector2Int(dx, dy);
            Vector2Int jumpedPos = currentPos + new Vector2Int(dx/2, dy/2);

            if (IsValidPosition(newPos) && GetTile(newPos).GetStone() == null)
            {
                Stone jumpedStone = GetTile(jumpedPos).GetStone();
                if (jumpedStone != null && jumpedStone.IsWhite() != stone.IsWhite())
                {
                    captures.Add(newPos);
                    Debug.Log("Can capture " + jumpedStone.name + " at " + GetTile(jumpedPos).name + " index " + jumpedPos + " from " + stone.name + " at " + GetTile(currentPos).name + " index " + currentPos);
                }
            }
        }

        private bool IsValidPosition(Vector2Int pos)
        {
            return pos.x >= 1 && pos.x <= 8 && pos.y >= 1 && pos.y <= 8;
        }

        public void ShowMoveGuides(List<Vector2Int> positions, CheckersMoveType moveType)
        {
            foreach (Vector2Int pos in positions)
            {
                ShowMoveGuides(pos, moveType);
            }
        }
        public void CheckForCheckersWin(bool currentPlayerIsWhite)
        {
            bool opponentHasMovesLeft = false;
            bool opponentHasStonesLeft = false;

            foreach (Stone stone in boardStateStones)
            {
                if (stone != null && stone.gameObject.activeSelf && stone.IsWhite() != currentPlayerIsWhite)
                {
                    opponentHasStonesLeft = true;
                    stone.GeneratePossibleMovesForBoard(stone.GetCurrentTile().GetBoardIndex());
                    this.HideMoveGuides();
                    if (stone.GetPossibleMoves().Count > 0)
                    {
                        opponentHasMovesLeft = true;
                        break;
                    }
                }
            }

            if (!opponentHasStonesLeft || !opponentHasMovesLeft)
            {
                HandleWin(currentPlayerIsWhite);
            }
        }

        private void HandleWin(bool winnerIsWhite)
        {
            string winMessage = winnerIsWhite ? "White Won!" : "Black Won!";
            Debug.Log(winMessage);
        }
        #endregion

        #region Board Movement
        public CheckersMoveType ValidMove(Vector2Int boardPosition, Stone stone)
        {
            if (boardState[boardPosition.x, boardPosition.y].GetStone() == stone)
            {
                return CheckersMoveType.Stay;
            }
            else if (boardState[boardPosition.x, boardPosition.y].GetStone() == null)
            {
                return CheckersMoveType.Allowed;
            }
            else if (boardState[boardPosition.x, boardPosition.y].GetStone() == stone && boardState[boardPosition.x, boardPosition.y].GetStone().IsWhite() != stone.IsWhite())
            {
                //return HandleOccupiedTile(boardPosition, stone);
                return CheckersMoveType.Capture;
            }
            return CheckersMoveType.Invalid;
        }

        public void ShowMoveGuides(Vector2Int boardPosition, CheckersMoveType moveType)
        {
            boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
            boardState[boardPosition.x, boardPosition.y].SetCheckersMoveGuideColor(moveType);
        }

        public void HideMoveGuides(List<Vector2Int> boardPositions = null)
        {
            if (boardPositions == null)
            {
                foreach (Tile tile in boardState)
                {
                    if (tile != null && tile.GetBoardIndex().x != 0 && tile.GetBoardIndex().y != 0)
                    {
                        tile.SetMoveGuideShown(false);
                    }
                }
            }
            else
            {
                foreach (Vector2Int boardPosition in boardPositions)
                {
                    if (boardState[boardPosition.x, boardPosition.y] != null)
                    {
                        boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(false);
                    }
                }
            }
        }
        #endregion

        #region Utility Methods


        public void PlaySnapSound()
        {
            // Implement sound playing logic
        }

        public void PlayCaptureSound()
        {
            // Implement sound playing logic
        }
        #endregion
    }
}