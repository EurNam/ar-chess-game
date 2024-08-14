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
        public bool SwitchBoardSkinButton;
        public int tileAppearanceIndex;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            toggleButton.onClick.AddListener(buttonFunction);
            // if (this.skinButton)
            // {
            //     this.gameObject.SetActive(false);
            // }
        }
        public void buttonFunction()
        {   
            // Skin change function
            if (skinButton)
            {
                GameManager.Instance.SetRoomSkin(tileAppearanceIndex);
                GameManagerBufferData.Instance.SetBufferSkinData(tileAppearanceIndex);
                ARChessGameSettings.Instance.SetBoardSkin(tileAppearanceIndex);
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

        public void HideButton()
        {
            this.gameObject.SetActive(false);
        }
    }
}