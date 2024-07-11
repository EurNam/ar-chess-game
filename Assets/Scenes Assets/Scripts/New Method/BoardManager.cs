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
                Vector2Int position = tileObject.GetPosition2D();
                boardState[position.x, position.y] = tileObject;
            }
        }
    }

    public MoveType ValidMove(Vector2Int position, PieceManager piece)
    {
        if (boardState[position.x, position.y].GetPieceManager() == null)
        {
            boardState[position.x, position.y].SetMoveGuideShown(true);
            boardState[position.x, position.y].SetMoveGuideColor(MoveType.Allowed);
            return MoveType.Allowed;
        }
        else if (boardState[position.x, position.y].GetPieceManager().colorWhite() != piece.colorWhite())
        {
            boardState[position.x, position.y].SetMoveGuideShown(true);
            boardState[position.x, position.y].SetMoveGuideColor(MoveType.Capture);
            return MoveType.Capture;
        }
        else if (boardState[position.x, position.y].GetPieceManager() == piece)
        {
            boardState[position.x, position.y].SetMoveGuideShown(true);
            boardState[position.x, position.y].SetMoveGuideColor(MoveType.Stay);
            return MoveType.Stay;
        }
        return MoveType.Invalid;
    }

    public void HideMoveGuides(List<Vector2Int> positions)
    {
        foreach (Vector2Int position in positions)
        {
            boardState[position.x, position.y].SetMoveGuideShown(false);
        }
    }
}