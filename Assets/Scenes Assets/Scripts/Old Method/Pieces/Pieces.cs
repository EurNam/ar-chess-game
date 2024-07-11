// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public abstract class Pieces : MonoBehaviour
// {
//     public Pieces piece;
//     public Vector2Int position;
//     public List<Vector2Int> possibleMoves = new List<Vector2Int>();
//     public Board board;
//     public Color whiteColor = Color.white;
//     public Color blackColor = Color.black;

//     // FUNCTIONS

//     // Set color of pieces
//     public void SetColor()
//     {
//         if (position.y > 4)
//         {
//             GetComponent<Renderer>().material.color = blackColor;
//         }
//         else
//         {
//             GetComponent<Renderer>().material.color = whiteColor;
//         }
//     }
   
//    // Snap to position
//     public float SnapToHalf(float input)
//     {
//         return Mathf.Round(input);
//     }

//     // Get piece possible moves
//     public List<Vector2Int> GetPossibleMoves()
//     {
//         if (this.name.Contains("Rook"))
//         {
//             return VertiHoriMovement(this.position, this.board, 8);
//         }
//         return possibleMoves;
//     }
//     public List<Vector2Int> VertiHoriMovement(Vector2Int position, Board board, int steps)
//     {
//         List<Vector2Int> moves = new List<Vector2Int>();

//         // Horizontal right
//         for (int x = 0; x <= steps; x++)
//         {
//             if (position.x+x<=8){
//                 Vector2Int newPosition = new Vector2Int(position.x+x, position.y);
//                 if (board.CanMoveTo(newPosition, this)){
//                     if (!possibleMoves.Contains(newPosition)){
//                         possibleMoves.Add(newPosition);
//                     }
//                 } else {
//                     break;
//                 }
//             } else {
//                 break;
//             }
//         }

//         // Horizontal left
//         for (int x = 0; x <= steps; x++)
//         {
//             if (position.x-x>=1){
//                 Vector2Int newPosition = new Vector2Int(position.x-x, position.y);
//                 if (board.CanMoveTo(newPosition, this)){
//                     if (!possibleMoves.Contains(newPosition)){
//                         possibleMoves.Add(newPosition);
//                     }
//                 } else {
//                     break;
//                 }
//             } else {
//                 break;
//             }
//         }

//         // Vertical up
//         for (int y = 0; y <= steps; y++)
//         {
//             if (position.y+y<=8){
//                 Vector2Int newPosition = new Vector2Int(position.x, position.y+y);
//                 if (board.CanMoveTo(newPosition, this)){
//                     if (!possibleMoves.Contains(newPosition)){
//                         possibleMoves.Add(newPosition);
//                     }
//                 } else {
//                     break;
//                 }
//             } else {
//                 break;
//             }
//         }

//         // Vertical down
//         for (int y = 0; y <= steps; y++)
//         {
//             if (position.y-y>=1){
//                 Vector2Int newPosition = new Vector2Int(position.x, position.y-y);
//                 if (board.CanMoveTo(newPosition, this)){
//                     if (!possibleMoves.Contains(newPosition)){
//                         possibleMoves.Add(newPosition);
//                     }
//                 } else {
//                     break;
//                 }
//             } else {
//                 break;
//             }
//         }
//         return possibleMoves;
//     }
// }