using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class Button : MonoBehaviour, IPointerDownHandler
    {
        public static Button Instance;
        public bool isWhitePlayer;
        public Button otherButton;
        public Transform pivot;

        void Start()
        {
            Instance = this;
        }

        void Update()
        {

        }

        public void handleOnClick()
        {
            float buttonHeight = GetComponent<Renderer>().bounds.size.y;

            if (otherButton != null && otherButton.transform.position.y == transform.position.y) 
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - buttonHeight, transform.position.z);
                ARChessGameSettings.Instance.SetWhitePlayer(isWhitePlayer);
                ARChessGameSettings.Instance.SetGameStarted(true);
            } 
            else if (otherButton != null && otherButton.transform.position.y < transform.position.y)
            {
                otherButton.transform.position = new Vector3(otherButton.transform.position.x, otherButton.transform.position.y + buttonHeight, otherButton.transform.position.z);
                transform.position = new Vector3(transform.position.x, transform.position.y - buttonHeight, transform.position.z);
                ARChessGameSettings.Instance.SetWhitePlayer(isWhitePlayer);
            }

            if (ShouldRotateBoard())
            {
                BoardRotator.Instance.RotateBoard();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            handleOnClick();
        }

        public void OnMouseDown()
        {
            handleOnClick();
        }

        private bool ShouldRotateBoard()
        {
            float currentYRotation = pivot.eulerAngles.y;
            if ((isWhitePlayer && Mathf.Abs(Mathf.DeltaAngle(currentYRotation, 0f)) < 1f) ||
                (!isWhitePlayer && Mathf.Abs(Mathf.DeltaAngle(currentYRotation, 180f)) < 1f))
            {
                return false;
            }
            return true;
        }

        public void ResetPosition()
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            BoardManager.Instance.HideMoveGuides();
        }
    }
}