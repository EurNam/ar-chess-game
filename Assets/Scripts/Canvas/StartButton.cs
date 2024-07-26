using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class StartButton : MonoBehaviour
    {
        public UnityEngine.UI.Button toggleButton;

        void Start()
        {
            toggleButton.onClick.AddListener(StartGame);
        }

        public void StartGame()
        {   
            ARChessGameSettings.Instance.SetGameStarted(true);
            GameRoomManager.Instance.StartGameRoom();
        }
    }
}