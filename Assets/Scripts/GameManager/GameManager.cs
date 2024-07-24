using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public static GameManager Instance;
        private string m_playerID;
        private string[] m_gameSettings = new string[2]{"",""};
        private bool whitePlayer;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {
            if (GameRoomManager.Instance.IsMultiplayerMode())
            {
                GameRoomManager.Instance.SetGameManager(this);
                // Get ID from room
                m_playerID = GameRoomManager.Instance.GetPlayerID();
                // Attempt to set room setting and play as white
                m_gameSettings[0] = m_playerID;
                GameRoomManager.Instance.SetGameRoomSettings(m_gameSettings);
                // Retrieve setting from room
            }
            else
            {
                m_playerID = "White";
                m_gameSettings[0] = m_playerID;
                if (m_gameSettings[0] != null)
                {
                    whitePlayer = true;
                }
                else 
                {
                    whitePlayer = false;
                }
            }
        }

        public bool GetWhitePlayer()
        {
            return whitePlayer;
        }

        public void SetWhitePlayer(bool whitePlayer)
        {
            this.whitePlayer = whitePlayer;
        }

        public void SetGameSettings(object gameSettings)
        {
            // Retrieve setting from room
            m_gameSettings = gameSettings as string[];
            // Check if allowed to play as white
            if (m_gameSettings[0] == m_playerID)
            {
                // Play as white
                whitePlayer = true;
            }
            else
            {
                // Play as black
                whitePlayer = false;
            } 
        }
        public void SwitchRoomTurn()
        {
            GameRoomManager.Instance.SwitchRoomTurn();
        }

        public void SwitchTurn()
        {
            BoardManager.Instance.SetWhiteTurn();
        }
    }
}