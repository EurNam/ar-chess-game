using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public GameObject board;
    private TileManager[,] boardState;

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
        boardState = new TileManager[boardSize, boardSize];

        // Find all tiles with the "Tile" tag
        TileManager[] tileObjects = FindObjectsOfType<TileManager>();
        Debug.Log("Tile objects: " + tileObjects.Length);
        foreach (TileManager tileObject in tileObjects)
        {
            if (tileObject != null)
            {
                Vector2Int boardIndex = tileObject.GetBoardIndex();
                boardState[boardIndex.x, boardIndex.y] = tileObject;
            }
        }
    }

    public MoveType ValidMove(Vector2Int boardPosition, PieceManager piece)
    {
        if (boardState[boardPosition.x, boardPosition.y].GetPieceManager() == null)
        {
            boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
            boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Allowed);
            return MoveType.Allowed;
        }
        else if (boardState[boardPosition.x, boardPosition.y].GetPieceManager().colorWhite() != piece.colorWhite())
        {
            boardState[boardPosition.x, boardPosition.y].SetMoveGuideShown(true);
            boardState[boardPosition.x, boardPosition.y].SetMoveGuideColor(MoveType.Capture);
            return MoveType.Capture;
        }
        else if (boardState[boardPosition.x, boardPosition.y].GetPieceManager() == piece)
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