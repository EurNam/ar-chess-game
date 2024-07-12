using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public PieceManager pieceManager;
    public bool isWhite;
    private TileManager currentTile;
    private TileManager nearestTile;
    private Vector3 mousePosition;
    private Plane dragPlane;
    private TileManager[] tiles;
    private bool isDragging = false;
    private Vector3 new3DPosition;
    private List<Vector2Int> possibleMoves = new List<Vector2Int>();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        FindCurrentTile();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDragging)
        {
            DragPiece();
        }
    }

    public List<Vector2Int> GetPossibleMoves()
    {
        return possibleMoves;
    }

    public bool colorWhite()
    {
        return isWhite;
    }

    public void SetPossibleMoves(List<Vector2Int> moves)
    {
        possibleMoves = moves;
    }

    public void SetCurrentTile(TileManager tile)
    {
        currentTile = tile;
    }

    protected virtual void GeneratePossibleMoves(Vector2Int currentBoardPosition)
    {
        possibleMoves.Clear();
    }

    private void FindCurrentTile()
    {
        tiles = FindObjectsOfType<TileManager>();
        float minDistance = float.MaxValue;
        TileManager startingNearestTile = null;

        foreach (TileManager tile in tiles)
        {
            float distance = Vector3.Distance(transform.position, tile.GetPosition3D());
            if (distance < minDistance)
            {
                minDistance = distance;
                startingNearestTile = tile;
            }
        }

        if (startingNearestTile != null)
        {
            currentTile = startingNearestTile;
            nearestTile = startingNearestTile;
            currentTile.SetOccupied(true, this);
        }
    }

    private void OnMouseDown()
    {
        // Set the piece to be dragged
        isDragging = true;
        // Set the plane to be the piece
        dragPlane = new Plane(Vector3.up, transform.position);
        // Set the mouse position to be the piece
        mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        // Generate the possible moves for the piece
        GeneratePossibleMoves(currentTile.GetBoardIndex());

    }
    
    private void DragPiece()
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
        TileManager newNearestTile = FindNearestTile();
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

    private void OnMouseUp()
    {
        isDragging = false;
        SnapToNearestTile();
        BoardManager.Instance.HideMoveGuides(possibleMoves);
    }

    private void SnapToNearestTile()
    {
        // Check if the closest tile has a piece and if it is not the same piece
        if (nearestTile.GetPieceManager() != null && nearestTile.GetPieceManager() != this)
        {
            // If it does, destroy the piece and set the tile to be empty
            nearestTile.GetPieceManager().gameObject.SetActive(false);
            nearestTile.SetOccupied(false);
        }
        // Move the piece to the new tile and update the board state
        transform.position = nearestTile.GetPosition3D() + Vector3.up;
        currentTile.SetOccupied(false);
        currentTile = nearestTile;
        currentTile.SetOccupied(true, this);
    }

    private TileManager FindNearestTile()
    {
        float minDistance = float.MaxValue;
        TileManager nearestTile = null;

        // Find the closest tile to the mouse position to snap the piece to
        foreach (TileManager tile in tiles)
        {
            float distance = Vector3.Distance(new3DPosition, tile.GetPosition3D());
            // Update the closest tile if the distance is less than the current closest tile
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }

        // Check if the closest tile is a valid move and if the distance is less than 1.3 units
        if (possibleMoves.Contains(nearestTile.GetBoardIndex()) && minDistance < 1.3f)
        {
            return nearestTile;
        }

        return currentTile;
    }
}