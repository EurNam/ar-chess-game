using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Vector2Int position2D;
    private Vector3 position3D;
    private bool isOccupied = false;
    private PieceManager pieceManager = null;
    private Renderer moveGuideRenderer;
    private Transform moveGuideTransform;

    void Awake()
    {
        this.SetPosition2D(new Vector2Int((int)transform.position.x, (int)transform.position.z));
        this.SetPosition3D(transform.position);

        // Find the move guide using the MoveGuide script
        MoveGuide moveGuide = GetComponentInChildren<MoveGuide>();
        if (moveGuide != null)
        {
            moveGuideRenderer = moveGuide.GetComponent<Renderer>();
            moveGuideTransform = moveGuide.transform;
            // Set initial scale
            moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
        }

        SetMoveGuideShown(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int GetPosition2D()
    {
        return position2D;
    }

    public Vector3 GetPosition3D()
    {
        return position3D;
    }

    public bool GetOccupiedState()
    {
        return isOccupied;
    }

    public PieceManager GetPieceManager()
    {
        return pieceManager;
    }

    public void SetPosition2D(Vector2Int position)
    {
        position2D = position;
    }

    public void SetPosition3D(Vector3 position)
    {
        position3D = position;
    }

    public void SetOccupied(bool occupied, PieceManager piece = null)
    {
        isOccupied = occupied;
        pieceManager = piece;
    }

    public void SetMoveGuideShown(bool shown)
    {
        if (moveGuideRenderer != null)
        {
            moveGuideRenderer.enabled = shown;
        }
    }

    public void SetMoveGuideColor(BoardManager.MoveType moveType)
    {
        if (moveGuideRenderer != null)
        {
            switch (moveType)
            {
                case BoardManager.MoveType.Allowed:
                    moveGuideRenderer.material.color = Color.green;
                    moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
                    break;
                case BoardManager.MoveType.Capture:
                    moveGuideRenderer.material.color = Color.green;
                    moveGuideTransform.localScale = new Vector3(0.8f, moveGuideTransform.localScale.y, 0.8f);
                    break;
                case BoardManager.MoveType.Stay:
                    moveGuideRenderer.material.color = Color.yellow;
                    moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
                    break;
                default:
                    moveGuideRenderer.material.color = Color.clear;
                    moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
                    break;
            }
        }
    }
}