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
    }
}