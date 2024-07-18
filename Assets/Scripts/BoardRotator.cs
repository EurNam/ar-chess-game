using UnityEngine;

public class BoardRotator : MonoBehaviour
{
    public static BoardRotator Instance;
    public Transform pivot;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {

    }

    public void RotateBoard()
    {
        pivot.Rotate(0, 180, 0);
    }
}