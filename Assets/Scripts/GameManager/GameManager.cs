using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    [Serializable]
    public class GameSettings
    {
        public GameSide side;
    }

    [Serializable]
    public class GameSide
    {
        public string black;
        public string white;
    }

    public static class SIDE
    {
        public const string MASTER = "master";
        public const string GUEST = "guest";
    }

    public class GameManager : MonoBehaviour, IGameInstance, IGameActionListener
    {
        public static GameManager Instance;
        private string m_playerID;
        private string[] m_gameSettings = new string[4]{"","","",""}; // 1: White Side, 2: Black Side, 3: Room Host, 4: Tile Skin
        private bool whitePlayer = true;
        private bool roomHost = true;
        private int skinIndex = 0;
        private GameSettings m_new_gameSettings = new GameSettings();



        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {
            #if SEENSIOGO
                IGameRoomManager.Instance.SetGameInstance(this,"XWVSJ2yCtyLlez9F47tu");
                IGameRoomManager.Instance.RegisterGameActionListener(this);
                m_gameSettings = await IGameRoomManager.Instance.GetGameRoomSettings<GameSettings>();
            #else
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
            #endif
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
            Debug.Log("Is white player: " + whitePlayer);
        }

        public void SetRoomHost(bool roomHost)
        {
            this.roomHost = roomHost;
        }

        public void SetRoomSkin(int skinIndex)
        {
            this.skinIndex = skinIndex;
        }

        public object GetGameSettings()
        {
            // Get player ID from room
            // m_playerID = GameRoomManager.Instance.GetPlayerID();
            // Set player side
            if (whitePlayer)
            {
                m_gameSettings[0] = m_playerID;
            }
            else
            {
                m_gameSettings[1] = m_playerID;
            }
            // Set room host
            m_gameSettings[2] = m_playerID;
            // Set board skin
            m_gameSettings[3] = skinIndex.ToString();
            return m_gameSettings;
        }

        public void SetGameSettings(object gameSettings)
        {
            // Retrieve setting from room
            m_gameSettings = gameSettings as string[];
            // Check if allowed to play as white
            if (m_gameSettings[0] == m_playerID)
            {
                if (!whitePlayer)
                {
                    whitePlayer = true;
                    BoardRotator.Instance.RotateBoard();
                } 
                Debug.Log("White player");
            }
            else
            {
                if (whitePlayer)
                {
                    whitePlayer = false;
                    BoardRotator.Instance.RotateBoard();
                }
                Debug.Log("Black player");
            } 

            // Set room host to show change skin option
            if (m_gameSettings[2] == m_playerID)
            {
                roomHost = true;
            }
            else
            {
                roomHost = false;
                TileAppearanceButton.Instance.gameObject.SetActive(false);
            }

            // Set room skin to corresponding screen
            if (m_gameSettings[3] != null)
            {
                skinIndex = int.Parse(m_gameSettings[3]);
                ARChessGameSettings.Instance.SetTileSkin(skinIndex);
            }
        }

        // Switch room turn
        public void SwitchRoomTurn()
        {
            #if SEENSIOGO
                IGameRoomManager.Instance.ScatterActionToRoom("SwitchTurn");
            #else
                this.SwitchTurn();
            #endif
        }

        public void OnActionReceived(string actionName)
        {
            Debug.Log("Action received: " + actionName);
            Invoke(actionName, 0.1f);
        }

        // Update local turn and board state pieces position
        public void SwitchTurn()
        {
            #if !SEENSIOGO
                this.SetWhitePlayer(!this.whitePlayer);
            #endif
            BoardManager.Instance.SetWhiteTurn();
            if (this.IsMyTurn())
            {
                IGameRoomManager.Instance.TakeOwnerShip();
            }
            BoardManager.Instance.UpdateBoardStatePiecesPosition();
        }

        public bool IsMyTurn()
        {
            return whitePlayer == BoardManager.Instance.GetWhiteTurn();
        }
    }
}