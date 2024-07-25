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
        private string[] m_gameSettings = new string[3]{"","",""}; // 1: White Side, 2: Black Side, 3: Room Host
        private bool whitePlayer;
        private bool roomHost;

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

        public bool GetRoomHost()
        {
            return roomHost;
        }

        public void SetWhitePlayer(bool whitePlayer)
        {
            this.whitePlayer = whitePlayer;
        }

        public void SetRoomHost(bool roomHost)
        {
            this.roomHost = roomHost;
        }

        public object GetGameSettings()
        {
            // Get player ID from room
            m_playerID = GameRoomManager.Instance.GetPlayerID();
            // Attempt to set room setting and play as white
            m_gameSettings[0] = m_playerID;
            // Set room host
            m_gameSettings[2] = m_playerID;
            return m_gameSettings;
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

            if (m_gameSettings[2] == m_playerID)
            {
                roomHost = true;
            }
            else
            {
                roomHost = false;
                TileAppearanceButton.Instance.gameObject.SetActive(false);
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

        public bool IsMyTurn()
        {
            return whitePlayer == BoardManager.Instance.GetWhiteTurn();
        }
    }
}