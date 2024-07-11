using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public GameObject board;
    private TileManager[,] boardState;
    private MoveGuideManager[,] moveGuideState;
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
        foreach (GameObject tileObject in tileObjects)
        {
            TileManager tile = tileObject.GetComponent<TileManager>();
            if (tile != null)
            {
                Vector2Int position = tile.GetPosition2D();
                boardState[position.x, position.y] = tile;
                // if(tile.GetPieceManager() != null)
                // {
                //     Debug.Log(tile.GetPieceManager().name + " is on tile: " + tile.name);
                // }
            }
        }

        // Find all move guides with the "MoveGuide" tag and hide them
        GameObject[] moveGuideObjects = GameObject.FindGameObjectsWithTag("MoveGuide");
        foreach (GameObject moveGuideObject in moveGuideObjects)
        {
            MoveGuideManager moveGuide = moveGuideObject.GetComponent<MoveGuideManager>();
            if (moveGuide != null)
            {
                Vector2Int position = moveGuide.GetPosition2D();
                moveGuideState[position.x, position.y] = moveGuide;
                moveGuideState[position.x, position.y].SetShown(false);
            }
        }
    }

    public bool ValidMove(Vector2Int position, PieceManager piece)
    {
        if (boardState[position.x, position.y].GetPieceManager() == null || boardState[position.x, position.y].GetPieceManager().colorWhite() != piece.colorWhite() || boardState[position.x, position.y].GetPieceManager() == piece)
        {
            return true;
        }
        return false;
    }
}