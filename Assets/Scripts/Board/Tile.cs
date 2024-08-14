using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int boardIndex;
        public GameObject currentSkin;
        public GameObject[] tilePrefabs;
        public Material[] tileMaterials;
        private int tileAppearanceIndex = 0;
        private Vector3 position3D;
        private bool isOccupied = false;
        private Piece occupyingPiece = null;
        private bool isOccupiedByStone = false;
        private Stone occupyingStone = null;
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

        void Start()
        {
            if (tilePrefabs.Length == 0)
            {
                return;
            }

            // Create tile
            currentSkin = Instantiate(tilePrefabs[0], transform);
            currentSkin.transform.localPosition = Vector3.zero;
            currentSkin.transform.localRotation = Quaternion.identity;
            currentSkin.transform.localScale = Vector3.one;

            // Set color for tile
            if ((boardIndex.x + boardIndex.y) % 2 == 0)
            {
                this.SetTileMaterial(0);
            }
            else
            {
                this.SetTileMaterial(1);
            }

            // Find the move guide using the MoveGuide script
            MoveGuide moveGuide = currentSkin.GetComponentInChildren<MoveGuide>();
            if (moveGuide != null)
            {
                moveGuideRenderer = moveGuide.GetComponent<Renderer>();
                moveGuideTransform = moveGuide.transform;
                moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
            }
            SetMoveGuideShown(false);

            // Find components for animation
            Transform bloodTransform = currentSkin.transform.Find("Blood");
            if (bloodTransform != null)
            {
                this.bloodTransform = bloodTransform;
                SetBloodShown(false);
            }
            Transform deathTransform = currentSkin.transform.Find("Death");
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
            return occupyingPiece;
        }

        public bool GetOccupiedByStoneState()
        {
            return isOccupiedByStone;
        }

        public Stone GetStone()
        {
            return occupyingStone;
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
            occupyingPiece = piece;
        }

        public void SetOccupiedByStone(bool occupied, Stone stone = null)
        {
            isOccupiedByStone = occupied;
            occupyingStone = stone;
        }

        public void SetTileMaterial(int materialIndex)
        {
            Material newMaterial = tileMaterials[materialIndex];
            Renderer renderer = currentSkin.GetComponent<Renderer>();

            renderer.material = newMaterial;
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

        public void SetCheckersMoveGuideColor(CheckersBoardManager.CheckersMoveType moveType)
        {
            if (moveGuideRenderer != null)
            {
                switch (moveType)
                {
                    case CheckersBoardManager.CheckersMoveType.Allowed:
                        moveGuideRenderer.material.color = Color.green;
                        moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
                        break;
                    case CheckersBoardManager.CheckersMoveType.Capture:
                        moveGuideRenderer.material.color = Color.green;
                        moveGuideTransform.localScale = new Vector3(0.8f, moveGuideTransform.localScale.y, 0.8f);
                        break;
                    case CheckersBoardManager.CheckersMoveType.Stay:
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
            SetOccupiedByStone(false, null);
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

            // Destroy current skin
            if (currentSkin != null)
            {
                Destroy(currentSkin);
            }

            // Instantiate the new skin
            currentSkin = Instantiate(tilePrefabs[prefabIndex], transform);
            currentSkin.transform.localPosition = Vector3.zero;
            currentSkin.transform.localRotation = Quaternion.identity;
            currentSkin.transform.localScale = Vector3.one;

            // Set tile colors accordingly to new skins
            int blackColor = 0;
            int whiteColor = 1;
            if (prefabIndex == 1)
            {
                blackColor = 2;
                whiteColor = 3;
            }

            // Set color for tile
            if ((boardIndex.x + boardIndex.y) % 2 == 0)
            {
                this.SetTileMaterial(blackColor);
            }
            else
            {
                this.SetTileMaterial(whiteColor);
            }

            // Find the move guide using the MoveGuide script
            MoveGuide moveGuide = currentSkin.GetComponentInChildren<MoveGuide>();
            if (moveGuide != null)
            {
                moveGuideRenderer = moveGuide.GetComponent<Renderer>();
                moveGuideTransform = moveGuide.transform;
                moveGuideTransform.localScale = new Vector3(0.5f, moveGuideTransform.localScale.y, 0.5f);
            }
            SetMoveGuideShown(false);

            // Find components for animation
            Transform bloodTransform = currentSkin.transform.Find("Blood");
            if (bloodTransform != null)
            {
                this.bloodTransform = bloodTransform;
                SetBloodShown(false);
            }
            Transform deathTransform = currentSkin.transform.Find("Death");
            if (deathTransform != null)
            {
                this.deathTransform = deathTransform;
                originalDeathPosition = deathTransform.localPosition;
                SetDeathShown(false);
            }
        }
    }
}