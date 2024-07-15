using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Piece piece;
    public bool isWhite;
    private Tile currentTile;
    private Tile nearestTile;
    private Vector3 mousePosition;
    private Plane dragPlane;
    private Tile[] tiles;
    private bool isDragging = false;
    private Vector3 new3DPosition;
    private bool firstMove = true;
    private bool isKing = false;
    private List<Vector2Int> possibleMoves = new List<Vector2Int>();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        FindCurrentTile();
        this.SetFirstMove(true);
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

    public Tile GetCurrentTile()
    {
        return currentTile;
    }

    public bool isKingPiece()
    {
        return isKing;
    }

    public bool isFirstMove()
    {
        return firstMove;
    }

    public bool colorWhite()
    {
        return isWhite;
    }

    public void SetPossibleMoves(List<Vector2Int> moves)
    {
        possibleMoves = moves;
    }

    public void SetCurrentTile(Tile tile)
    {
        currentTile = tile;
    }

    public void SetKing()
    {
        isKing = true;
    }

    public void SetFirstMove(bool move)
    {
        firstMove = move;
    }

    public void SetWhite(bool white)
    {
        isWhite = white;
    }

    protected virtual void GeneratePossibleMoves(Vector2Int currentBoardPosition)
    {
        possibleMoves.Clear();
    }

    private void FindCurrentTile()
    {
        tiles = FindObjectsOfType<Tile>();
        float minDistance = float.MaxValue;
        Tile startingNearestTile = null;

        foreach (Tile tile in tiles)
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
        Tile newNearestTile = FindNearestTile();
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

    protected virtual void OnMouseUp()
    {
        isDragging = false;
        SnapToNearestTile();
        BoardManager.Instance.HideMoveGuides(possibleMoves);
    }

    private void SnapToNearestTile()
    {
        // Check if the closest tile has a piece and if it is not the same piece
        if (nearestTile.GetPiece() != null && nearestTile.GetPiece() != this)
        {
            // If it does, destroy the piece and set the tile to be empty
            nearestTile.GetPiece().gameObject.SetActive(false);
            nearestTile.SetOccupied(false);
        }

        // Handle En Passant
        handleEnPassant(nearestTile);

        // Handle Rook in castling
        if (this.isKingPiece() && this.isFirstMove() && this.isCastle(nearestTile))
        {
            handleCastling(nearestTile);
            this.SetFirstMove(false);
        }

        // Move the piece to the new tile and update the board state
        if (nearestTile != currentTile){
            this.SetFirstMove(false);
            BoardManager.Instance.SetBoardIndexBeforeLastMove(currentTile.GetBoardIndex());
            BoardManager.Instance.SetPieceLastMoved(this);
            BoardManager.Instance.SetBoardIndexLastMove(nearestTile.GetBoardIndex());
        }

        // Update the board state before move
        transform.position = nearestTile.GetPosition3D() + Vector3.up;
        currentTile.SetOccupied(false);
        currentTile = nearestTile;
        currentTile.SetOccupied(true, this);
    }

    private Tile FindNearestTile()
    {
        float minDistance = float.MaxValue;
        Tile nearestTile = null;

        // Find the closest tile to the mouse position to snap the piece to
        foreach (Tile tile in tiles)
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

    private bool isCastle(Tile nearestTile)
    {
        // Check if the new move is a castle
        List<Vector2Int> castleMoves = new List<Vector2Int>();
        castleMoves.Add(new Vector2Int(7, 1));
        castleMoves.Add(new Vector2Int(3, 1));
        castleMoves.Add(new Vector2Int(7, 8));
        castleMoves.Add(new Vector2Int(3, 8));
        return castleMoves.Contains(nearestTile.GetBoardIndex());
    }

    private void handleCastling(Tile kingNewTile)
    {
        Vector2Int kingPosition = kingNewTile.GetBoardIndex();
        int rookCol = (kingPosition.x == 3) ? 1 : 8; // Determine which rook to move based on king's new position
        int rookNewCol = (kingPosition.x == 3) ? 4 : 6; // New position for the rook

        Tile rookTile = BoardManager.Instance.GetTile(new Vector2Int(rookCol, kingPosition.y));
        if (rookTile.GetPiece() is Rook rook)
        {
            // Move rook to castling position
            Tile newRookTile = BoardManager.Instance.GetTile(new Vector2Int(rookNewCol, kingPosition.y));
            rook.transform.position = newRookTile.GetPosition3D() + Vector3.up;
            rookTile.SetOccupied(false);
            newRookTile.SetOccupied(true, rook);
            rook.SetCurrentTile(newRookTile);
        }
    }

    private void handleEnPassant(Tile nearestTile)
    {
        // Check for En Passant capture
        if (!this.isFirstMove())
        {
            // If last piece moved is a pawn
            if (BoardManager.Instance.GetPieceLastMoved().GetType() == typeof(Pawn))
            {
                // If that pawn moved two tiles forward
                if (Mathf.Abs(BoardManager.Instance.GetBoardIndexLastMove().y - BoardManager.Instance.GetBoardIndexBeforeLastMove().y) == 2)
                {
                    // If this pawn is in the same row as that pawn that moved two tiles forward right before
                    if (this.GetCurrentTile().GetBoardIndex().y == BoardManager.Instance.GetBoardIndexLastMove().y)
                    {
                        // If this pawn is next to the pawn that moved two tiles forward right before
                        if (Mathf.Abs(this.GetCurrentTile().GetBoardIndex().x - BoardManager.Instance.GetBoardIndexLastMove().x) == 1)
                        {
                            Debug.Log("En passant capture");

                            int yChange = 1;
                            if (BoardManager.Instance.GetBoardIndexLastMove().y > BoardManager.Instance.GetBoardIndexBeforeLastMove().y)
                            {
                                yChange = -1;
                            }

                            if (nearestTile.GetBoardIndex() == new Vector2Int(BoardManager.Instance.GetBoardIndexLastMove().x, BoardManager.Instance.GetBoardIndexLastMove().y+yChange))
                            {
                                BoardManager.Instance.GetTile(BoardManager.Instance.GetBoardIndexLastMove()).GetPiece().gameObject.SetActive(false);
                                BoardManager.Instance.GetTile(BoardManager.Instance.GetBoardIndexLastMove()).SetOccupied(false);
                            }
                        }
                    }
                }
            }
        }
    }
}