using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class TileColorButton : MonoBehaviour
    {
        public UnityEngine.UI.Button toggleButton;
        public int tileColorIndex;
        public int player;

        void Start()
        {
            toggleButton.onClick.AddListener(toggleColor);
        }
        public void toggleColor()
        {   
            if (tileColorIndex == 4)
            {
                ARChessGameSettings.Instance.SetTileSkin();
            }
            else
            {
                ARChessGameSettings.Instance.SetTileColor(tileColorIndex, player);
            }
        }
    }
}