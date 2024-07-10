using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGuide : MonoBehaviour
{
    public Vector2Int position;
    public Pieces currentPiece;

    public Color validColor = Color.green;
    public Color captureColor = Color.yellow;

    private void Start()
    {
        SetColor();
    }

    private void SetColor()
    {
        GetComponent<Renderer>().material.color = validColor;
    }
}
