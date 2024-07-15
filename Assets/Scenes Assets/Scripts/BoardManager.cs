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

        public enum MoveType
        {
            Invalid,
            Allowed,
            Capture,
            Stay
        }

        void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            InitializeBoardState();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void InitializeBoardState()
        {
            int boardSize = 9;
            boardState = new Tile[boardSize, boardSize];

            // Find all tiles with the "Tile" tag
            Tile[] tileObjects = FindObjectsOfType<Tile>();
            Debug.Log("Tile objects: " + tileObjects.Length);
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

        public MoveType ValidMove(Vector2Int boardPosition, Piece piece, bool enPassant = false)
        {
            if (enPassant)
            {
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Capture);
                return MoveType.Capture;
            }
            else if (boardState[boardPosition.x, boardPosition.y].GetPiece() == null)
            {
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Allowed);
                return MoveType.Allowed;
            }
            else if (boardState[boardPosition.x, boardPosition.y].GetPiece().colorWhite() != piece.colorWhite())
            {
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Capture);
                return MoveType.Capture;
            }
            else if (boardState[boardPosition.x, boardPosition.y].GetPiece() == piece)
            {
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Stay);
                return MoveType.Stay;
            }
            return MoveType.Invalid;
        }

        public void HideMoveGuides(List<Vector2Int> boardPositions)
        {
            foreach (Vector2Int boardPosition in boardPositions)
            {
                boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(false);
            }
        }
    }
}