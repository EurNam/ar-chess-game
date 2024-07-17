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

        void Start()
        {
            Instance = this;
        }

        void Update()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (otherButton != null && otherButton.transform.position.y == transform.position.y) 
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                ARChessGameSettings.Instance.SetWhitePlayer(isWhitePlayer);
                ARChessGameSettings.Instance.SetGameStarted(true);
            } else if (otherButton != null && otherButton.transform.position.y < transform.position.y)
            {
                otherButton.transform.position = new Vector3(otherButton.transform.position.x, otherButton.transform.position.y + 1, otherButton.transform.position.z);
                transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                ARChessGameSettings.Instance.SetWhitePlayer(isWhitePlayer);
            }
        }

        public void ResetPosition()
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            BoardManager.Instance.HideMoveGuides();
        }
    }
}