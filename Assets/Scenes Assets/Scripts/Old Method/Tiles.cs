// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Tiles : MonoBehaviour
// {
//     public Vector2Int position;
//     public Pieces currentPiece;

//     public Color whiteColor = Color.white;
//     public Color blackColor = Color.black;

//     private void Start()
//     {
//         SetColor();
//     }

//     private void SetColor()
//     {
//         if ((position.x + position.y) % 2 == 0)
//         {
//             GetComponent<Renderer>().material.color = blackColor;
//         }
//         else
//         {
//             GetComponent<Renderer>().material.color = whiteColor;
//         }
//     }
// }
