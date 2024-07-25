using UnityEngine;
using JKTechnologies.SeensioGo.ARChess;

namespace JKTechnologies.SeensioGo.GameEngine
{
    public class GameRoomManager : MonoBehaviour
    {
        public static GameRoomManager Instance;
        public bool isMultiplayerMode = false;
        private IGameManager m_gameManager;
        private object m_gameSettings;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            Debug.Log("Hello");
        }  

        #region EXTERNAL INTERFACES (BOTH EXTERNAL AND INTERNAL USE)
        /* 
            This is the external interface for the game room manager.
            This is only used for seensio go external use: ARChess, Seafood, etc.
        */

        public bool IsMultiplayerMode()
        {
            return default;
        }

        public bool IsGameRoomMaster()
        {
            return default;
        }

        public string GetPlayerID()
        {
            return "ID";
        }

        public void SetGameManager(IGameManager gameManager)
        {
            m_gameManager = gameManager;
        }

        public void SetGameRoomSettings(object gameSettings)
        {
            m_gameSettings = gameSettings;
        }


        public object GetRoomGameSettings()
        {
            return m_gameSettings;
        }

        public void SwitchRoomTurn()
        {
            // TODO: call switch turn on all players in this game room
            GameManager.Instance.SwitchTurn();
            // m_gameManager.SwitchTurn();
        }        
        #endregion
    }
}