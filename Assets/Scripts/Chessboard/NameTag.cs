using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class NameTag : MonoBehaviour
    {
        private string playerName;
        private TextMeshPro text;
        void Awake()
        {
            text = this.gameObject.GetComponent<TextMeshPro>();
            text.text = playerName ?? "Player";
            Debug.Log("NameTag: " + text.text);
        }
    }
}