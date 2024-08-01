using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class ARChessGameSettings : MonoBehaviour
    {
        public static ARChessGameSettings Instance;
        private bool whitePlayer;
        private bool gameStarted = false;
        private bool boardInitialized = false;
        // private bool botPlaying = false;
        private bool changeTileSkin = false;
        private int boardAppearanceIndex = 0;

        void Awake()
        {
            Instance = this;
            Debug.Log("ARChessGameSettings Awake");
        }

        public bool GetBoardInitialized()
        {
            return boardInitialized;
        }

        public bool GetChangeTileSkin()
        {
            return changeTileSkin;
        }

        public bool GetGameStarted()
        {
            return gameStarted;
        }

        public int GetBoardAppearanceIndex()
        {
            return boardAppearanceIndex;
        }

        public void SetWhitePlayer(bool whitePlayer)
        {
            this.whitePlayer = whitePlayer;
        }

        public void SetGameStarted(bool gameStarted)
        {
            this.gameStarted = gameStarted;
        }

        public void SetBoardInitialized(bool boardInitialized)
        {
            this.boardInitialized = boardInitialized;
        }

        public void SetChangeTileSkin(bool changeTileSkin)
        {
            this.changeTileSkin = changeTileSkin;
        }

        public void ResetGameSettings()
        {
            SetGameStarted(false);
            SetBoardInitialized(false);
            SetWhitePlayer(true);
        }

        public void ResetPlayerTurn()
        {
            ARChessGameSettings.Instance.SetWhitePlayer(true);
        }

        // public bool GetBotPlaying()
        // {
        //     return botPlaying;
        // }

        // public void SetBotPlayer(bool botPlaying)
        // {
        //     this.botPlaying = botPlaying;
        // }

        public void SetTileColor(int tileColorIndex, int player)
        {
            if (player == 1)
            {
                BoardManager.Instance.SetTileMaterial(tileColorIndex, player);
            } else {
                BoardManager.Instance.SetTileMaterial(tileColorIndex, player);
            }
        }

        public void SetBoardSkin(int tileAppearanceIndex)
        {
            if (tileAppearanceIndex == boardAppearanceIndex)
            {
                Debug.Log("Already using that skin");
                return;
            }
            changeTileSkin = true;
            boardInitialized = false;
            boardAppearanceIndex = tileAppearanceIndex;
            GameManager.Instance.ChangeBoardSkin();
        }
    }
}