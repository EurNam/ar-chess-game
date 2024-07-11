// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Board : MonoBehaviour
// {
//     public GameObject tilePrefab;
//     public GameObject rookPrefab;
//     public GameObject moveGuidePrefab;
//     public int boardSize = 8;
//     private float tileSpacing = 1.0f;
//     private char[] vertical = {'8','7','6','5','4','3','2','1'};
//     private char[] horizontal = {'a','b','c','d','e','f','g','h'};
//     private Dictionary<Vector2Int, GameObject> piecePositions = new Dictionary<Vector2Int, GameObject>();
//     private List<GameObject> moveGuides = new List<GameObject>();

//     // Start is called before the first frame update
//     void Start()
//     {
//         // GenerateBoard();
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     private void GenerateBoard()
//     {
//         // Tile generation
//         for (int x = 1; x <= boardSize; x++)
//         {
//             for (int y = 1; y <= boardSize; y++)
//             {
//                 Vector3 position = new Vector3(x * tileSpacing, 0, y * tileSpacing);
//                 GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity, transform);
//                 Tiles tile = tileObject.GetComponent<Tiles>();
//                 if (tile != null)
//                 {
//                     tile.position = new Vector2Int(x, y);
//                     tile.name = $"Tile {horizontal[x-1]}{vertical[8-y]}";
//                 }
//                 else
//                 {
//                     Debug.LogError("Tile prefab does not have a Tiles component attached.");
//                 }
//             }
//         }

//         // Rook generation
//         for (int x = 1; x <= 9; x+=7)
//         {
//             for (int y = 1; y <= 9; y+=7)
//             {
//                 Vector3 position = new Vector3(x * tileSpacing, 1, y * tileSpacing);
//                 GameObject rook = Instantiate(rookPrefab, position, Quaternion.identity, transform);
//                 rook.GetComponent<Rook>().board = this;
//                 rook.GetComponent<Rook>().position = new Vector2Int(x, y);
//                 rook.name = $"Rook {horizontal[x-1]}{vertical[8-y]}";
//                 piecePositions.Add(new Vector2Int(x, y), rook);
//             }
//         }
//     }

//     public void UpdateMoveGuides(List<Vector2Int> possibleMoves)
//     {
//         // Generate new move guides
//         foreach (Vector2Int move in possibleMoves)
//         {
//             Vector3 position = new Vector3(move.x * tileSpacing, 0, move.y * tileSpacing);
//             GameObject moveGuideObject = Instantiate(moveGuidePrefab, position, Quaternion.identity, transform);
//             moveGuides.Add(moveGuideObject);
//         }
//     }

//     public void ClearMoveGuides()
//     {
//         foreach (GameObject moveGuide in moveGuides)
//         {
//             Destroy(moveGuide);
//         }
//         moveGuides.Clear();
//     }

//     // Update later to enable capturing
//     public bool CanMoveTo(Vector2Int position, Pieces piece)
//     {
//         return !piecePositions.ContainsKey(position) || piecePositions[position].GetComponent<Pieces>() == piece;
//     }

//     public void MovePiece(Vector2Int oldPosition, Vector2Int newPosition, Pieces piece)
//     {
//         piecePositions.Remove(oldPosition);
//         piecePositions.Add(newPosition, piece.gameObject);
//         foreach (var kvp in piecePositions)
//         {
//             Pieces p = kvp.Value.GetComponent<Pieces>();
//             if (p != null)
//             {
//                 p.possibleMoves.Clear();
//                 p.possibleMoves = p.GetPossibleMoves();
//             }
//         }
//     }

//     public Dictionary<Vector2Int, GameObject> GetPiecePositions()
//     {
//         return piecePositions;
//     }
// }
