using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class StartButton : MonoBehaviour
    {
        public static StartButton Instance;
        public UnityEngine.UI.Button toggleButton;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            toggleButton.onClick.AddListener(StartGame);
        }

        public  void StartGame()
        {   
            ARChessGameSettings.Instance.SetGameStarted(true);
        }

        public void HideButton()
        {
            this.gameObject.SetActive(false);
        }
    }
}