using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class NameTag : MonoBehaviour
    {
        public static NameTag Instance;
        public bool isUser;
        private string playerName;
        private TextMeshPro text;
        void Awake()
        {
            Instance = this;
            text = this.gameObject.GetComponent<TextMeshPro>();
            text.text = playerName ?? "Player";
            Debug.Log("NameTag: " + text.text);
        }

        public void SetMasterName(string name)
        {
            if (isUser)
            {
                playerName = name;
                text.text = playerName;
            }
        }

        public void SetGuestName(string name)
        {
            if (!isUser)
            {
                playerName = name;
                text.text = playerName;
            }
        }
    }
}