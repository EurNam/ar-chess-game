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
        private bool botPlaying = false;
        private bool changeTileSkin = false;
        private int tileSkinIndex = 0;

        void Awake()
        {
            Instance = this;
            Debug.Log("ARChessGameSettings Awake");
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public bool GetWhitePlayer()
        {
            return whitePlayer;
        }

        public bool GetGameStarted()
        {
            return gameStarted;
        }

        public bool GetBoardInitialized()
        {
            return boardInitialized;
        }

        public bool GetChangeTileSkin()
        {
            return changeTileSkin;
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
            Button.Instance.ResetPosition();
        }

        public void ResetPlayerTurn()
        {
            ARChessGameSettings.Instance.SetWhitePlayer(true);
        }

        public bool GetBotPlaying()
        {
            return botPlaying;
        }

        public void SetBotPlayer(bool botPlaying)
        {
            this.botPlaying = botPlaying;
        }

        public void SetTileColor(int tileColorIndex, int player)
        {
            if (player == 1)
            {
                BoardManager.Instance.SetTileMaterial(tileColorIndex, player);
            } else {
                BoardManager.Instance.SetTileMaterial(tileColorIndex, player);
            }
        }

        public void SetTileSkin(int tileAppearanceIndex)
        {
            if (tileAppearanceIndex == tileSkinIndex)
            {
                Debug.Log("Already using that skin");
                return;
            }
            changeTileSkin = true;
            boardInitialized = false;
            BoardManager.Instance.SetTileSkin(tileAppearanceIndex);
            tileSkinIndex = tileAppearanceIndex;
        }
    }
}