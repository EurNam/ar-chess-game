using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGuideManager : MonoBehaviour
{
    private Vector2Int position2D;
    private Vector3 position3D;
    private bool isShown;

    // Start is called before the first frame update
    void Start()
    {
        this.SetPosition2D(new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.z));
        this.gameObject.SetActive(isShown);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.SetActive(isShown);
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
    }
}
