using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Pieces
{
    public Vector2Int position;
    public Board board;
    public Color whiteColor = Color.white;
    public Color blackColor = Color.black;
    private List<Vector2Int> possibleMoves = new List<Vector2Int>();
    private Vector3 mousePosition;
    private Plane dragPlane;

    private void Start()
    {
        SetColor();
        possibleMoves = VertiHoriMovement(position, board, 8);
        dragPlane = new Plane(Vector3.up, transform.position);
        position = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
    }

    private void SetColor()
    {
        if (position.y>4)
        {
            GetComponent<Renderer>().material.color = blackColor;
        }
        else
        {
            GetComponent<Renderer>().material.color = whiteColor;
        }
    }

    private void Update()
    {
        // Handle drop down not registered by event mouse up
        if (Input.GetMouseButtonUp(0))
        {
            UpdatePositionAndMoves();
            if (transform.position.y != 1)
            {
                transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                board.ClearMoveGuides();
            }
        }
    }

    private void UpdatePositionAndMoves()
    {
        position = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
        possibleMoves = VertiHoriMovement(position, board, 8); 
    }

    private void NotifyBoardOfPossibleMoves()
    {
        if (board != null)
        {
            board.UpdateMoveGuides(possibleMoves);
        }
    }

    // public List<Vector2Int> VertiHoriMovement(Vector2Int position, int steps)
    // {
    //     List<Vector2Int> possibleMoves = new List<Vector2Int>();

    //     // Horizontal right
    //     for (int x = 0; x <= steps; x++)
    //     {
    //         if (position.x+x<=8){
    //             Vector2Int newPosition = new Vector2Int(position.x+x, position.y);
    //             possibleMoves.Add(newPosition);
    //         } else {
    //             break;
    //         }
    //     }

    //     // Horizontal left
    //     for (int x = 0; x <= steps; x++)
    //     {
    //         if (position.x-x>=1){
    //             Vector2Int newPosition = new Vector2Int(position.x-x, position.y);
    //             possibleMoves.Add(newPosition);
    //         } else {
    //             break;
    //         }
    //     }

    //     // Vertical up
    //     for (int y = 0; y <= steps; y++)
    //     {
    //         if (position.y+y<=8){
    //             Vector2Int newPosition = new Vector2Int(position.x, position.y+y);
    //             possibleMoves.Add(newPosition);
    //         } else {
    //             break;
    //         }
    //     }

    //     // Vertical down
    //     for (int y = 0; y <= steps; y++)
    //     {
    //         if (position.y-y>=1){
    //             Vector2Int newPosition = new Vector2Int(position.x, position.y-y);
    //             possibleMoves.Add(newPosition);
    //         } else {
    //             break;
    //         }
    //     }
    //     return possibleMoves;
    // }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private Vector3 beforeMovePosition;

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        beforeMovePosition = transform.position;
        NotifyBoardOfPossibleMoves();
    }


    private void OnMouseDrag()
    {   
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (dragPlane.Raycast(ray, out distance))
        {
            Vector3 newPosition = ray.GetPoint(distance);
            newPosition.x = SnapToHalf(newPosition.x);
            newPosition.z = SnapToHalf(newPosition.z);
            Vector2Int newPositionInt = new Vector2Int((int)newPosition.x, (int)newPosition.z);
            if (possibleMoves.Contains(newPositionInt))
            {
                transform.position = new Vector3(newPosition.x, 2, newPosition.z);
            }
        }
    }

    private void OnMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (dragPlane.Raycast(ray, out distance))
        {
            Vector3 newPosition = ray.GetPoint(distance);
            newPosition.x = SnapToHalf(newPosition.x);
            newPosition.z = SnapToHalf(newPosition.z);
            Vector2Int newPositionInt = new Vector2Int((int)newPosition.x, (int)newPosition.z);
            if (possibleMoves.Contains(newPositionInt))
            {
                transform.position = new Vector3(newPosition.x, 1, newPosition.z);
                board.MovePiece(position, newPositionInt, this);
            }
            else
            {
                transform.position = beforeMovePosition;
            }
        }
        else
        {
            transform.position = beforeMovePosition;
        }
        board.ClearMoveGuides();
    }

    private float SnapToHalf(float input)
    {
        return Mathf.Round(input);
    }
}
