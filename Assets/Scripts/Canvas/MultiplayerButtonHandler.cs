using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JKTechnologies.SeensioGo.GameEngine;
using JKTechnologies.SeensioGo.Scene;

namespace JKTechnologies.SeensioGo.ARCheess
{
    public class MultiplayerButtonHandler : MonoBehaviour
    {
        public static MultiplayerButtonHandler Instance;
        public UnityEngine.UI.Button toggleButton;
        public bool invitePlayerButton;
        public bool exitRoomButton;

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            toggleButton.onClick.AddListener(buttonFunction);
        }

        public void buttonFunction()
        {   
            // Skin change function
            if (invitePlayerButton)
            {
                Debug.Log("Invite player");
                ISeensioGoSceneUtilities.Instance?.InviteOtherUserToRoom();
            }

            if (exitRoomButton)
            {
                Debug.Log("Exit Room");
                ISeensioGoSceneUtilities.Instance?.ExitRoom();
            }
        }

        public void HideButton()
        {
            this.gameObject.SetActive(false);
        }
    }
}