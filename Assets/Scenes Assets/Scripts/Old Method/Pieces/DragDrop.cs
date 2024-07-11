// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DragDrop : MonoBehaviour
// {
//     private Vector3 mousePosition;
//     private List<Vector2Int> possibleMoves = new List<Vector2Int>();

//     private void Start()
//     {

//     }

//     private Vector3 GetMousePos()
//     {
//         return Camera.main.WorldToScreenPoint(transform.position);
//     }

//     private void OnMouseDown()
//     {
//         mousePosition = Input.mousePosition - GetMousePos();
//     }

//     private void OnMouseDrag()
//     {
//         Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
//         newPosition.x = SnapToHalf(newPosition.x);
//         newPosition.z = SnapToHalf(newPosition.z);
//         Vector2Int newPositionInt = new Vector2Int((int)newPosition.x, (int)newPosition.z);
//         if (possibleMoves.Contains(newPositionInt))
//         {
//             transform.position = new Vector3(newPosition.x, 2, newPosition.z);
//         }
//     }

//     private void OnMouseUp()
//     {
//         transform.position = new Vector3(transform.position.x, 1, transform.position.z);
//     }

//     public void SetPossibleMoves(List<Vector2Int> moves)
//     {
//         possibleMoves = new List<Vector2Int>(moves);
//     }

//     private float SnapToHalf(float input)
//     {
//         return Mathf.Round(input);
//     }
// }