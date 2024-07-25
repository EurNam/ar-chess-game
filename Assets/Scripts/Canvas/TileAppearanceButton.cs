using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class TileAppearanceButton : MonoBehaviour
    {
        public static TileAppearanceButton Instance;
        public UnityEngine.UI.Button toggleButton;
        public bool skinButton;
        public int tileAppearanceIndex;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            toggleButton.onClick.AddListener(toggleColor);
            if (this.skinButton)
            {
                this.gameObject.SetActive(false);
            }
        }
        public void toggleColor()
        {   
            if (skinButton)
            {
                ARChessGameSettings.Instance.SetTileSkin(tileAppearanceIndex);
            }
        }
    }
}