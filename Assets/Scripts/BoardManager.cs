using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class BoardManager : MonoBehaviour
    {
        public static BoardManager Instance;
        public GameObject board;
        public GameObject queenWhitePrefab;
        public GameObject queenBlackPrefab;
        private Tile[,] boardState;
        private Vector2Int boardIndexBeforeLastMove;
        private Vector2Int boardIndexLastMove;
        private Piece pieceLastMoved;
        private bool whiteTurn = true;
        private Vector2Int whiteKingPosition = new Vector2Int(5, 1);
        private Vector2Int blackKingPosition = new Vector2Int(5, 8);
        private int moveCount = 0;
        private bool inCheck = false;

        public enum MoveType
        {
            Invalid,
            Allowed,
            Capture,
            Check,
            Stay
        }

        void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (ARChessGameSettings.Instance.GetGameStarted())
            {
                InitializeBoardState();
                ARChessGameSettings.Instance.SetBoardInitialized(true);
                ARChessGameSettings.Instance.SetGameStarted(false);
                GenerateAllPossibleMoves();
                HideMoveGuides();
            }
        }

        private void InitializeBoardState()
        {
            int boardSize = 9;
            boardState = new Tile[boardSize, boardSize];

            // Find all tiles with the "Tile" tag
            Tile[] tileObjects = FindObjectsOfType<Tile>();
            foreach (Tile tileObject in tileObjects)
            {
                if (tileObject != null)
                {
                    Vector2Int boardIndex = tileObject.GetBoardIndex();
                    boardState[boardIndex.x, boardIndex.y] = tileObject;
                }
            }
        }

        public Tile GetTile(Vector2Int boardPosition)
        {
            return boardState[boardPosition.x, boardPosition.y];
        }

        public Vector2Int GetBoardIndexBeforeLastMove()
        {
            return boardIndexBeforeLastMove;
        }

        public Vector2Int GetBoardIndexLastMove()
        {
            return boardIndexLastMove;
        }

        public Piece GetPieceLastMoved()
        {
            return pieceLastMoved;
        }

        public bool GetWhiteTurn()
        {
            return whiteTurn;
        }

        public Vector2Int GetWhiteKingPosition()
        {
            return whiteKingPosition;
        }

        public Vector2Int GetBlackKingPosition()
        {
            return blackKingPosition;
        }

        public int GetMoveCount()
        {
            return moveCount;
        }

        public bool GetInCheck()
        {
            return inCheck;
        }

        public void SetBoardIndexBeforeLastMove(Vector2Int boardIndex)
        {
            boardIndexBeforeLastMove = boardIndex;
        }

        public void SetBoardIndexLastMove(Vector2Int boardIndex)
        {
            boardIndexLastMove = boardIndex;
        }

        public void SetPieceLastMoved(Piece piece)
        {
            pieceLastMoved = piece;
        }

        public void SetWhiteTurn()
        {
            whiteTurn = !whiteTurn;
        }

        public void SetInCheck(bool value)
        {
            inCheck = value;
        }

        public void SetWhiteKingPosition(Vector2Int boardPosition)
        {
            whiteKingPosition = boardPosition;
        }

        public void SetBlackKingPosition(Vector2Int boardPosition)
        {
            blackKingPosition = boardPosition;
        }

        public void IncrementMoveCount()
        {
            moveCount++;
        }

        public void GenerateAllPossibleMoves()
        {
            foreach (Tile tile in boardState)
            {
                if (tile != null)
                {
                    if (tile.GetPiece() != null)
                    {
                        tile.GetPiece().GeneratePossibleMovesForBoard(tile.GetBoardIndex());
                    }
                }
            }
        }

        public MoveType ValidMove(Vector2Int boardPosition, Piece piece, bool enPassant = false)
        {
            if (enPassant)
            {
                // boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
                // boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Capture);
                return MoveType.Capture;
            }
            else if (boardState[boardPosition.x, boardPosition.y].GetPiece() == null)
            {
                // boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
                // boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Allowed);
                return MoveType.Allowed;
            }
            else if (boardState[boardPosition.x, boardPosition.y].GetPiece().colorWhite() != piece.colorWhite())
            {
                // boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
                // boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Capture);
                return MoveType.Capture;
            }
            else if (boardState[boardPosition.x, boardPosition.y].GetPiece() == piece)
            {
                // boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
                // boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Stay);
                return MoveType.Stay;
            }
            return MoveType.Invalid;
        }

        public void ShowMoveGuides(Vector2Int boardPosition, MoveType moveType)
        {
            boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
            boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(moveType);
        }

        public void HideMoveGuides(List<Vector2Int> boardPositions = null)
        {
            if (boardPositions == null)
            {
                foreach (Tile tile in boardState)
                {
                    if (tile != null)
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

        public bool IsKingChecked(bool isWhite)
        {
            Vector2Int kingPosition = isWhite ? GetWhiteKingPosition() : GetBlackKingPosition();
            //Debug.Log(isWhite ? "White King is at: " + kingPosition : "Black King is at: " + kingPosition);
            foreach (Tile tile in boardState)
            {
                if (tile != null)
                {
                    Piece piece = tile.GetPiece();
                    if (piece != null && piece.colorWhite() != isWhite)
                    {
                        //Debug.Log("Checking if : " + piece.name + " can capture king from " + tile.GetBoardIndex());
                        List<Vector2Int> possibleMoves = piece.GetPossibleMoves();
                        if (possibleMoves.Contains(kingPosition))
                        {
                            // Debug.Log(piece.name + " can capture king from " + tile.GetBoardIndex());
                            // Debug.Log("possibleMoves: " + possibleMoves);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CanRemoveCheck(bool isWhite)
        {
            foreach (Tile tile in boardState)
            {
                if (tile != null)
                {
                    Piece piece = tile.GetPiece();
                    if (piece != null && piece.colorWhite() == isWhite)
                    {
                        List<Vector2Int> possibleMoves = piece.GetPossibleMoves();
                        foreach (Vector2Int move in possibleMoves)
                        {
                            Tile targetTile = GetTile(move);
                            Piece originalPiece = targetTile.GetPiece();
                            targetTile.SetOccupied(true, piece);
                            tile.SetOccupied(false);
                            UpdateBoardState(tile.GetBoardIndex(), move, piece);

                            if (!IsKingChecked(isWhite))
                            {
                                // Revert the move
                                targetTile.SetOccupied(true, originalPiece);
                                tile.SetOccupied(true, piece);
                                UpdateBoardState(move, tile.GetBoardIndex(), piece);
                                return true;
                            }

                            // Revert the move
                            targetTile.SetOccupied(true, originalPiece);
                            tile.SetOccupied(true, piece);
                            UpdateBoardState(move, tile.GetBoardIndex(), piece);
                        }
                    }
                }
            }
            return false;
        }

        public void UpdateBoardState(Vector2Int moveBefore, Vector2Int moveAfter, Piece piece)
        {
            SetBoardIndexBeforeLastMove(moveBefore);
            SetBoardIndexLastMove(moveAfter);
            SetPieceLastMoved(piece);

            if (piece.isKingPiece())
            {
                if (piece.colorWhite())
                {
                    BoardManager.Instance.SetWhiteKingPosition(moveAfter);
                }
                else
                {
                    BoardManager.Instance.SetBlackKingPosition(moveAfter);
                }
            }

            GenerateAllPossibleMoves();
            BoardManager.Instance.HideMoveGuides();
        }

        public void CheckForCheckmate()
        {
            if (IsKingChecked(whiteTurn))
            {
                if (CanRemoveCheck(whiteTurn))
                {
                    Debug.Log("Check");
                    SetInCheck(true);
                }
                else
                {
                    Debug.Log("Checkmate");
                    ResetBoard();
                }
                if (whiteTurn)
                {
                    boardState[GetWhiteKingPosition().x, GetWhiteKingPosition().y].SetMoveGuideShown(true);
                    boardState[GetWhiteKingPosition().x, GetWhiteKingPosition().y].SetMoveGuideColor(MoveType.Check);
                }
                else
                {
                    boardState[GetBlackKingPosition().x, GetBlackKingPosition().y].SetMoveGuideShown(true);
                    boardState[GetBlackKingPosition().x, GetBlackKingPosition().y].SetMoveGuideColor(MoveType.Check);
                }
            }
        }

        public void ResetBoard()
        {
            // Reset all pieces to their initial positions
            foreach (Tile tile in boardState)
            {
                if (tile != null && tile.GetPiece() != null)
                {
                    tile.GetPiece().ResetPosition();
                }
            }

            // Reset game settings
            ARChessGameSettings.Instance.ResetGameSettings();

            // Reset turn to white
            whiteTurn = true;
            moveCount = 0;
            inCheck = false;
            ARChessGameSettings.Instance.ResetPlayerTurn();
        }
    }
}