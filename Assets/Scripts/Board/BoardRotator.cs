using UnityEngine;
using System.Collections;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class BoardRotator : MonoBehaviour
    {
        public static BoardRotator Instance;
        public Transform pivot;
        public Tile scaling;
        public float animationDuration = 2f;
        private bool rotating = false;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {

        }

        public bool GetIsRotating()
        {
            return rotating;
        }

        public void RotateBoard()
        {
            StartCoroutine(RotateBoardCoroutine());
        }

        private IEnumerator RotateBoardCoroutine()
        {
            rotating = true;

            Vector3 originalPosition = pivot.position;
            Vector3 raisedPosition = originalPosition;
            Quaternion originalRotation = pivot.rotation;
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, 180, 0);

            float elapsedTime = 0f;
            pivot.position = raisedPosition;

            // Rotate the board
            while (elapsedTime < animationDuration / 2)
            {
                pivot.rotation = Quaternion.Lerp(originalRotation, targetRotation, (elapsedTime / (animationDuration / 2)));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            rotating = false;
        }
    }
}