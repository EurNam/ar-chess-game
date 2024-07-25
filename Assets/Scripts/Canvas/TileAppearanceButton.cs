using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class TileAppearanceButton : MonoBehaviour
    {
        public UnityEngine.UI.Button toggleButton;
        public bool skinButton;
        public int tileAppearanceIndex;
        public int player;

        void Start()
        {
            toggleButton.onClick.AddListener(toggleColor);
            this.gameObject.SetActive(false);
        }
        public void toggleColor()
        {   
            if (skinButton)
            {
                ARChessGameSettings.Instance.SetTileSkin(tileAppearanceIndex);
            }
            else
            {
                ARChessGameSettings.Instance.SetTileColor(tileAppearanceIndex, player);
            }
        }
    }
}