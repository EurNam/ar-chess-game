using UnityEngine;
using UnityEngine.UI;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class ChooseGameButton : MonoBehaviour
    {
        public static ChooseGameButton Instance;
        public UnityEngine.UI.Button toggleButton;
        public bool chessGame;
        public bool checkersGame;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            toggleButton.onClick.AddListener(ChangeGame);
        }

        public  void ChangeGame()
        {   
            if(chessGame)
            {
                GameManager.Instance.SetChessGame(true);
                GameManager.Instance.SetCheckersGame(false);
            }
            else
            {
                GameManager.Instance.SetChessGame(false);
                GameManager.Instance.SetCheckersGame(true);
            }
        }
    }
}