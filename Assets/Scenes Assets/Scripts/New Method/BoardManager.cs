using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public GameObject board;
    private TileManager[,] boardState;
    private MoveGuideManager[,] moveGuideState;
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
        InitializeBoardState();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeBoardState()
    {
        int boardSize = 9;
        boardState = new TileManager[boardSize, boardSize];
        moveGuideState = new MoveGuideManager[boardSize, boardSize];

        // Find all tiles with the "Tile" tag
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        Debug.Log("Tile objects: " + tileObjects.Length);
        foreach (GameObject tileObject in tileObjects)
        {
            TileManager tile = tileObject.GetComponent<TileManager>();
            if (tile != null)
            {
                Vector2Int position = tile.GetPosition2D();
                boardState[position.x, position.y] = tile;
            }
        }

        // Find all move guides with the MoveGuideManager script and hide them
        MoveGuideManager[] moveGuideObjects = FindObjectsOfType<MoveGuideManager>();
        Debug.Log("Move guide objects: " + moveGuideObjects.Length);
        foreach (MoveGuideManager moveGuide in moveGuideObjects)
        {
            Vector2Int position = moveGuide.GetPosition2D();
            moveGuideState[position.x, position.y] = moveGuide;
            moveGuide.SetShown(false);
        }
    }

    public MoveType ValidMove(Vector2Int position, PieceManager piece)
    {
        if (boardState[position.x, position.y].GetPieceManager() == null)
        {
            moveGuideState[position.x, position.y].SetShown(true);
            moveGuideState[position.x, position.y].SetColor(MoveType.Allowed);
            return MoveType.Allowed;
        }
        else if (boardState[position.x, position.y].GetPieceManager().colorWhite() != piece.colorWhite())
        {
            moveGuideState[position.x, position.y].SetShown(true);
            moveGuideState[position.x, position.y].SetColor(MoveType.Capture);
            return MoveType.Capture;
        }
        else if (boardState[position.x, position.y].GetPieceManager() == piece)
        {
            moveGuideState[position.x, position.y].SetShown(true);
            moveGuideState[position.x, position.y].SetColor(MoveType.Stay);
            return MoveType.Stay;
        }
        return MoveType.Invalid;
    }

    public void HideMoveGuides(List<Vector2Int> positions)
    {
        foreach (Vector2Int position in positions)
        {
            moveGuideState[position.x, position.y].SetShown(false);
        }
    }
}