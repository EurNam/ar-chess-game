using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public GameObject board;
    private Tile[,] boardState;

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

    public MoveType ValidMove(Vector2Int boardPosition, Piece piece)
    {
        if (boardState[boardPosition.x, boardPosition.y].GetPiece() == null)
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