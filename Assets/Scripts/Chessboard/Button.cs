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
        public bool whitePlayerSelected = true;
        public Button otherButton;
        public Transform pivot;
        private bool animationGoingOn = false;
        private bool startGame = false;
        private bool clicked = false;

        void Awake()
        {
            Instance = this;
            
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public bool IsWhitePlayerSelected()
        {
            return whitePlayerSelected;
        }

        public void SetWhitePlayerSelected(bool value)
        {
            whitePlayerSelected = value;
        }

        public bool IsAnimationGoingOn()
        {
            return animationGoingOn;
        }

        public void SetAnimationGoingOn(bool value)
        {
            animationGoingOn = value;
        }

        public void SetStartGame(bool value)
        {
            startGame = value;
        }

        public void SetClicked(bool value)
        {
            clicked = value;
        }

        public void handleOnClick()
        {
            if (animationGoingOn) return;

            if (otherButton != null && !startGame && !clicked) 
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - this.transform.localScale.y, transform.localPosition.z);
                ARChessGameSettings.Instance.SetWhitePlayer(isWhitePlayer);
                ARChessGameSettings.Instance.SetGameStarted(true);
                startGame = true;
                otherButton.SetStartGame(true);
                clicked = true;
                if (!this.isWhitePlayer)
                {
                    BoardRotator.Instance.RotateBoard();
                }
            } 
            else if (otherButton != null && otherButton.transform.localPosition.y < transform.localPosition.y && !clicked)
            {
                otherButton.transform.localPosition = new Vector3(otherButton.transform.localPosition.x, otherButton.transform.localPosition.y + otherButton.transform.localScale.y, otherButton.transform.localPosition.z);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - this.transform.localScale.y, transform.localPosition.z);
                ARChessGameSettings.Instance.SetWhitePlayer(isWhitePlayer);
                clicked = true;
                otherButton.SetClicked(false);
                BoardRotator.Instance.RotateBoard();
            }
            // if (isWhitePlayer)
            // {
            //     GameManager.Instance.SetWhitePlayer(true);
            // }
            // else 
            // {
            //     GameManager.Instance.SetWhitePlayer(false);
            // }
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
            transform.localPosition = new Vector3(transform.localPosition.x, 1, transform.localPosition.z);
            BoardManager.Instance.HideMoveGuides();
        }
    }
}