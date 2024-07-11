using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Vector2Int position2D;
    private Vector3 position3D;
    private bool isOccupied = false;
    private PieceManager pieceManager = null;

    // Start is called before the first frame update
    void Awake()
    {
        this.SetPosition2D(new Vector2Int((int)transform.position.x, (int)transform.position.z));
        this.SetPosition3D(transform.position);
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
}