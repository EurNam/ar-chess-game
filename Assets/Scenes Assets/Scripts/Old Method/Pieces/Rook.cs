// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Rook : Pieces
// {
//     private Vector3 mousePosition;
//     private Plane dragPlane;

//     private void Start()
//     {
//         this.SetColor();
//         possibleMoves = this.GetPossibleMoves();
//         dragPlane = new Plane(Vector3.up, transform.position);
//         position = new Vector2Int(Mathf.FloorToInt(transform.localPosition.x), Mathf.FloorToInt(transform.localPosition.z));
//     }

//     private void Update()
//     {
//         // Handle drop down not registered by event mouse up
//         if (Input.GetMouseButtonUp(0))
//         {
//             if (transform.localPosition.y != 1)
//             {
//                 UpdatePositionAndMoves();
//             }
//         }
//     }

//     private void UpdatePositionAndMoves()
//     {
//         Vector2Int newPosition = new Vector2Int(Mathf.FloorToInt(transform.localPosition.x), Mathf.FloorToInt(transform.localPosition.z));
//         if (newPosition != position)
//         {
//             Vector2Int oldPosition = position;
//             position = newPosition;
//             transform.localPosition = new Vector3(this.position.x, 1, this.position.y);
//             board.MovePiece(oldPosition, newPosition, this);
//             board.ClearMoveGuides();
//             Debug.Log("Position and moves updated");
//         }
//     }

//     private Vector3 GetMousePos()
//     {
//         return Camera.main.WorldToScreenPoint(transform.position);
//     }

//     private void OnMouseDown()
//     {
//         mousePosition = Input.mousePosition - GetMousePos();
//         board.UpdateMoveGuides(possibleMoves);
//     }

//     private void OnMouseDrag()
//     {
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         float distance;
//         if (dragPlane.Raycast(ray, out distance))
//         {
//             Vector3 newPosition = ray.GetPoint(distance);
//             newPosition.x = SnapToHalf(newPosition.x);
//             newPosition.z = SnapToHalf(newPosition.z);
//             Vector2Int newPositionInt = new Vector2Int((int)newPosition.x, (int)newPosition.z);
//             if (possibleMoves.Contains(newPositionInt))
//             {
//                 transform.localPosition = new Vector3(newPosition.x, 2, newPosition.z);
//             }
//         }
//     }

//     private void OnMouseUp()
//     {
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         float distance;
//         if (dragPlane.Raycast(ray, out distance))
//         {
//             Vector3 newPosition = ray.GetPoint(distance);
//             newPosition.x = SnapToHalf(newPosition.x);
//             newPosition.z = SnapToHalf(newPosition.z);
//             Vector2Int newPositionInt = new Vector2Int((int)newPosition.x, (int)newPosition.z);
//             if (possibleMoves.Contains(newPositionInt))
//             {
//                 Vector2Int oldPosition = position;
//                 position = newPositionInt;
//                 transform.localPosition = new Vector3(this.position.x, 1, this.position.y);
//                 board.MovePiece(oldPosition, newPositionInt, this);
//             }
//             else
//             {
//                 transform.localPosition = new Vector3(position.x, 1, position.y);
//             }
//         }
//         else
//         {
//             transform.localPosition = new Vector3(position.x, 1, position.y);
//         }
//         board.ClearMoveGuides();
//     }
// }