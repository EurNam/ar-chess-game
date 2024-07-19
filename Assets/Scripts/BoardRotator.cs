using UnityEngine;
using System.Collections;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class BoardRotator : MonoBehaviour
    {
        public static BoardRotator Instance;
        public Transform pivot;
        public float animationDuration = 2f;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {

        }

        public void RotateBoard()
        {
            StartCoroutine(RotateBoardCoroutine());
        }

        private IEnumerator RotateBoardCoroutine()
        {
            Button.Instance.SetAnimationGoingOn(true);

            Vector3 originalPosition = pivot.position;
            Vector3 raisedPosition = originalPosition + Vector3.up * this.transform.localScale.y * 3;
            Quaternion originalRotation = pivot.rotation;
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, 180, 0);

            float elapsedTime = 0f;

            // Raise the board
            while (elapsedTime < animationDuration / 2)
            {
                pivot.position = Vector3.Lerp(originalPosition, raisedPosition, (elapsedTime / (animationDuration / 2)));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            pivot.position = raisedPosition;
            elapsedTime = 0f;

            // Rotate the board
            while (elapsedTime < animationDuration / 2)
            {
                pivot.rotation = Quaternion.Lerp(originalRotation, targetRotation, (elapsedTime / (animationDuration / 2)));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            pivot.rotation = targetRotation;
            elapsedTime = 0f;

            // Lower the board
            while (elapsedTime < animationDuration / 2)
            {
                pivot.position = Vector3.Lerp(raisedPosition, originalPosition, (elapsedTime / (animationDuration / 2)));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            pivot.position = originalPosition;
            
            Button.Instance.SetAnimationGoingOn(false); 
        }
    }
}