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
        public bool SwitchSidesButton;
        public int tileAppearanceIndex;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            toggleButton.onClick.AddListener(buttonFunction);
            if (this.skinButton)
            {
                this.gameObject.SetActive(false);
            }
        }
        public void buttonFunction()
        {   
            // Skin change function
            if (skinButton)
            {
                ARChessGameSettings.Instance.SetTileSkin(tileAppearanceIndex);
                GameManager.Instance.SetRoomSkin(tileAppearanceIndex);
            }

            if (SwitchSidesButton)
            {
                if (!BoardRotator.Instance.GetIsRotating())
                {
                    GameManager.Instance.SetWhitePlayer(!GameManager.Instance.GetWhitePlayer());
                    BoardRotator.Instance.RotateBoard();
                }
            }
        }
    }
}