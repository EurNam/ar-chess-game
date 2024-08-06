using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int boardIndex;
        public GameObject[] tilePrefabs;
        public Material[] tileMaterials;
        private int tileAppearanceIndex = 0;
        private Vector3 position3D;
        private bool isOccupied = false;
        private Piece piece = null;
        private Renderer tileRenderer;
        private GameObject tilePrefab;
        private Renderer moveGuideRenderer;
        private Transform moveGuideTransform;
        private Transform bloodTransform;
        private Transform deathTransform;
        private Vector3 originalDeathPosition;

        void Awake()
        {
            this.SetPosition3D(transform.position);
            this.tileRenderer = GetComponent<Renderer>();

            // Find the move guide using the MoveGuide script
            MoveGuide moveGuide = GetComponentInChildren<MoveGuide>();
            if (moveGuide != null)
            {
                moveGuideRenderer = moveGuide.GetComponent<Renderer>();
                moveGuideTransform = moveGuide.transform;
                // Set initial scale
                moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
            }

            SetMoveGuideShown(false);

            Transform bloodTransform = transform.Find("Blood");
            if (bloodTransform != null)
            {
                this.bloodTransform = bloodTransform;
                SetBloodShown(false);
            }

            Transform deathTransform = transform.Find("Death");
            if (deathTransform != null)
            {
                this.deathTransform = deathTransform;
                originalDeathPosition = deathTransform.localPosition;
                SetDeathShown(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Vector2Int GetBoardIndex()
        {
            return boardIndex;
        }

        public Vector3 GetPosition3D()
        {
            return transform.position;
        }

        public bool GetOccupiedState()
        {
            return isOccupied;
        }

        public Piece GetPiece()
        {
            return piece;
        }

        public void SetBoardIndex(Vector2Int boardIndex)
        {
            this.boardIndex = boardIndex;
        }

        public void SetPosition3D(Vector3 position)
        {
            position3D = position;
        }

        public void SetOccupied(bool occupied, Piece piece = null)
        {
            isOccupied = occupied;
            this.piece = piece;
        }

        public void SetTileMaterial(int materialIndex)
        {
            tileRenderer.material = tileMaterials[materialIndex];
        }

        public void SetMoveGuideShown(bool shown)
        {
            if (moveGuideRenderer != null)
            {
                moveGuideRenderer.enabled = shown;
            }
        }

        public void SetMoveGuideColor(BoardManager.MoveType moveType)
        {
            if (moveGuideRenderer != null)
            {
                switch (moveType)
                {
                    case BoardManager.MoveType.Allowed:
                        moveGuideRenderer.material.color = Color.green;
                        moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
                        break;
                    case BoardManager.MoveType.Capture:
                        moveGuideRenderer.material.color = Color.green;
                        moveGuideTransform.localScale = new Vector3(0.8f, moveGuideTransform.localScale.y, 0.8f);
                        break;
                    case BoardManager.MoveType.Check:
                        moveGuideRenderer.material.color = Color.red;
                        moveGuideTransform.localScale = new Vector3(1f, moveGuideTransform.localScale.y, 1f);
                        break;
                    case BoardManager.MoveType.Stay:
                        moveGuideRenderer.material.color = Color.yellow;
                        moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
                        break;
                    default:
                        moveGuideRenderer.material.color = Color.clear;
                        moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
                        break;
                }
            }
        }

        public void SetMoveGuideColor(Color color)
        {
            if (moveGuideRenderer != null)
            {
                moveGuideRenderer.material.color = color;
            }
        }

        public void SetBloodShown(bool shown)
        {
            if (bloodTransform != null)
            {
                bloodTransform.gameObject.SetActive(shown);
            }
        }

        public void ShowBloodTemporarily()
        {
            StartCoroutine(ShowBloodCoroutine());
        }

        private IEnumerator ShowBloodCoroutine()
        {
            SetBloodShown(true);
            yield return new WaitForSeconds(3f);
            SetBloodShown(false);
        }

        public void SetDeathShown(bool shown)
        {
            if (deathTransform != null)
            {
                deathTransform.gameObject.SetActive(shown);
            }
        }

        public void ShowDeathTemporarily()
        {
            StartCoroutine(ShowDeathCoroutine());
        }

        private IEnumerator ShowDeathCoroutine()
        {
            SetDeathShown(true);
            Vector3 targetPosition = originalDeathPosition + Vector3.up * 4f;
            float duration = 2f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                deathTransform.localPosition = Vector3.Lerp(originalDeathPosition, targetPosition, t);
                
                // Fade out effect
                CanvasGroup canvasGroup = deathTransform.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1 - t;
                }
                else
                {
                    Renderer renderer = deathTransform.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Color color = renderer.material.color;
                        color.a = 1 - t;
                        renderer.material.color = color;
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SetDeathShown(false);
            deathTransform.localPosition = originalDeathPosition;
            
            // Reset alpha
            CanvasGroup cg = deathTransform.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1;
            }
            else
            {
                Renderer r = deathTransform.GetComponent<Renderer>();
                if (r != null)
                {
                    Color c = r.material.color;
                    c.a = 1;
                    r.material.color = c;
                }
            }
        }

        public void ResetTile()
        {
            SetOccupied(false, null);
            SetMoveGuideShown(false);
        }

        public void ChangeTilePrefab()
        {
            int prefabIndex = ARChessGameSettings.Instance.GetBoardAppearanceIndex();
            if (prefabIndex < 0 || prefabIndex >= tilePrefabs.Length)
            {
                Debug.LogError("Invalid prefab index");
                return;
            }

            // Instantiate the new prefab
            GameObject newTile = Instantiate(tilePrefabs[prefabIndex], transform.position, transform.rotation, transform.parent);

            // Copy data from the current tile to the new tile
            Tile newTileComponent = newTile.GetComponent<Tile>();
            newTileComponent.gameObject.name = this.gameObject.name;
            newTileComponent.boardIndex = this.boardIndex;
            newTileComponent.tileAppearanceIndex = prefabIndex;

            // if (this.piece != null)
            // {
            //     this.piece.FindCurrentTile();
            // }

            if ((this.GetBoardIndex().x+this.GetBoardIndex().y)%2 == 0)
            {
                newTileComponent.SetTileMaterial(0);
            }
            else
            {
                newTileComponent.SetTileMaterial(1);
            }

            //  the current tile
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}