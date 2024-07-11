using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookManager : PieceManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void GeneratePossibleMoves(Vector2Int position)
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        // Horizontal right
        for (int x = 0; x <= 8; x++)
        {
            if (position.x+x<=8){
                Vector2Int newPosition = new Vector2Int(position.x+x, position.y);
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newPosition)){
                        possibleMoves.Add(newPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            break;
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }

        // Horizontal left
        for (int x = 0; x <= 8; x++)
        {
            if (position.x-x>=1){
                Vector2Int newPosition = new Vector2Int(position.x-x, position.y);
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newPosition)){
                        possibleMoves.Add(newPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            break;
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }

        // Vertical up
        for (int y = 0; y <= 8; y++)
        {
            if (position.y+y<=8){
                Vector2Int newPosition = new Vector2Int(position.x, position.y+y);
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newPosition)){
                        possibleMoves.Add(newPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            break;
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }

        // Vertical down
        for (int y = 0; y <= 8; y++)
        {
            if (position.y-y>=1){
                Vector2Int newPosition = new Vector2Int(position.x, position.y-y);
                BoardManager.MoveType moveType = BoardManager.Instance.ValidMove(newPosition, this);
                if (moveType != BoardManager.MoveType.Invalid)
                {
                    if (!possibleMoves.Contains(newPosition)){
                        possibleMoves.Add(newPosition);
                        if (moveType == BoardManager.MoveType.Capture)
                        {
                            break;
                        }
                    }
                } else {
                    break;
                }
            } else {
                break;
            }
        }
        this.SetPossibleMoves(possibleMoves);
    }
}