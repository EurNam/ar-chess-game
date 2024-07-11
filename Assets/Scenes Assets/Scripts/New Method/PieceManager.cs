using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public PieceManager pieceManager;
    public bool isWhite;
    private TileManager currentTile;
    private Vector3 mousePosition;
    private Plane dragPlane;
    private TileManager[] tiles;
    private bool isDragging = false;
    private Vector3 newPosition;
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

    protected virtual void GeneratePossibleMoves(Vector2Int position)
    {
        possibleMoves.Clear();
    }

    private void FindCurrentTile()
    {
        tiles = FindObjectsOfType<TileManager>();
        float minDistance = float.MaxValue;
        TileManager nearestTile = null;

        foreach (TileManager tile in tiles)
        {
            float distance = Vector3.Distance(transform.position, tile.GetPosition3D());
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }

        if (nearestTile != null)
        {
            currentTile = nearestTile;
            currentTile.SetOccupied(true, this);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        dragPlane = new Plane(Vector3.up, transform.position);
        mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        GeneratePossibleMoves(currentTile.GetPosition2D());

    }
    
    private void DragPiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (dragPlane.Raycast(ray, out distance))
        {
            newPosition = ray.GetPoint(distance);
            transform.position = newPosition;
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
        float minDistance = float.MaxValue;
        TileManager nearestTile = null;

        foreach (TileManager tile in tiles)
        {
            float distance = Vector3.Distance(newPosition, tile.GetPosition3D());
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }

        if (possibleMoves.Contains(nearestTile.GetPosition2D()) && minDistance < 1.3f)
        {
            transform.position = nearestTile.GetPosition3D() + Vector3.up;
            currentTile.SetOccupied(false);
            currentTile = nearestTile;
            currentTile.SetOccupied(true, this);
        } else {
            transform.position = currentTile.GetPosition3D() + Vector3.up;
        }
    }
}