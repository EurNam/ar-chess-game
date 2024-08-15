using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class NameTag : MonoBehaviour
    {
        public bool isUser;
        private string playerName;
        public TextMeshPro playerWins;
        public TextMeshPro playerLosses;
        private TextMeshPro text;
        void Awake()
        {
            text = this.gameObject.GetComponent<TextMeshPro>();
            text.text = playerName ?? "Player";
            playerWins.text = $"W:{Random.Range(0, 100)}";
            playerLosses.text = $"L:{Random.Range(0, 100)}";
            Debug.Log("NameTag: " + text.text);
        }

        public void SetNameTag(string name)
        {
            playerName = name;
            text.text = name;
        }

        public void SetMasterName(string name)
        {
            if (isUser)
            {
                playerName = name;
                text.text = playerName;
                playerWins.text = $"W:{Random.Range(0, 100)}";
                playerLosses.text = $"L:{Random.Range(0, 100)}";
            }
        }

        public void SetGuestName(string name)
        {
            if (!isUser)
            {
                playerName = name;
                text.text = playerName;
                playerWins.text = $"W:{Random.Range(0, 100)}";
                playerLosses.text = $"L:{Random.Range(0, 100)}";
            }
        }
    }
}