using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGuideManager : MonoBehaviour
{
    private Vector2Int position2D;
    private Vector3 position3D;
    private bool isShown;
    private Renderer renderer;

    void Awake()
    {
        this.SetPosition2D(new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.z));
        renderer = GetComponent<Renderer>();
        SetShown(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2Int GetPosition2D()
    {
        return position2D;
    }

    public bool IsShown()
    {
        return isShown;
    }

    public void SetPosition2D(Vector2Int position)
    {
        position2D = position;
    }

    public void SetShown(bool shown)
    {
        isShown = shown;
        if (renderer != null)
        {
            renderer.enabled = shown;
        }
    }

    public void SetColor(BoardManager.MoveType moveType)
    {
        if (renderer != null)
        {
            switch (moveType)
            {
                case BoardManager.MoveType.Allowed:
                    renderer.material.color = Color.green;
                    break;
                case BoardManager.MoveType.Capture:
                    renderer.material.color = Color.green;
                    break;
                case BoardManager.MoveType.Stay:
                    renderer.material.color = Color.yellow;
                    break;
                default:
                    renderer.material.color = Color.clear;
                    break;
            }
        }
    }
}