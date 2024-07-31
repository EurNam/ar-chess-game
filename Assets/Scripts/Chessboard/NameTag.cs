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
        private TextMeshPro text;
        void Awake()
        {
            text = this.gameObject.GetComponent<TextMeshPro>();
            text.text = playerName ?? "Player";
            Debug.Log("NameTag: " + text.text);
        }

        public void SetName(string name)
        {
            playerName = name;
            text.text = playerName;
        }
    }
}