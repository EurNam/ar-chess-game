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
        public GameSide side = new GameSide();
    }

    [Serializable]
    public class GameSide
    {
        public string black; // "master" || "guest"
        public string white; // "master" || "guest"
    }

    public static class SIDE
    {
        public const string MASTER = "master";
        public const string GUEST = "guest";
    }

    public class GameManager : MonoBehaviour, IGameInstance, IGameActionListener
    {
        public static GameManager Instance;
        public string gameID;
        private string m_playerID;
        private string[] m_old_gameSettings = new string[4]{"","","",""}; // 1: White Side, 2: Black Side, 3: Room Host, 4: Tile Skin
        private bool whitePlayer = true;
        private bool roomHost = true;
        private int skinIndex = 0;
        private bool isRoomMaster = true;
        private GameSettings m_gameSettings = new GameSettings();



        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private async void Start()
        {
            #if SEENSIOGO
                IGameRoomManager.Instance.SetGameInstance(this,gameID);
                IGameRoomManager.Instance.RegisterGameActionListener(this);
                isRoomMaster = IGameRoomManager.Instance.IsRoomMaster();
                if (!isRoomMaster)
                {
                    TileAppearanceButton.Instance.HideButton();
                }
                m_gameSettings = await IGameRoomManager.Instance.GetGameRoomSettings<GameSettings>();
                this.SetGameSettings();

                BufferData bufferData = await IGameRoomManager.Instance.GetBufferDataToRoom<BufferData>();
                if (bufferData == null)
                {
                    bufferData = GameManagerBufferData.Instance.GetBufferData();
                    IGameRoomManager.Instance.SetBufferRoomData(bufferData);
                }
                else 
                {
                    GameManagerBufferData.Instance.SetBufferData(bufferData);
                    ARChessGameSettings.Instance.SetBoardSkin(bufferData.boardAppearanceIndex);
                    BoardManager.Instance.UpdatePieceCaptureState(bufferData.boardPieceState);
                }

            #else
                // m_playerID = "White";
                // m_gameSettings[0] = m_playerID;
                // if (m_gameSettings[0] != null)
                // {
                //     whitePlayer = true;
                // }
                // else 
                // {
                //     whitePlayer = false;
                // }
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

        public async void SetGameSettings()
        {
            // Retrieve setting from room
            if ((m_gameSettings.side.white == "master") == isRoomMaster)
            {
                if (!whitePlayer)
                {
                    whitePlayer = true;
                    BoardRotator.Instance.RotateBoard();
                    IGameRoomManager.Instance.TakeOwnerShip();
                } 
            }   
            else
            {
                if (whitePlayer)
                {
                    whitePlayer = false;
                    BoardRotator.Instance.RotateBoard();
                }
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

        public void ChangeRoomBoardSkin()
        {
            #if SEENSIOGO
                IGameRoomManager.Instance.SetBufferRoomData(GameManagerBufferData.Instance.GetBufferData());
                IGameRoomManager.Instance.ScatterActionToRoom("ChangeBoardSkin");
            #else
                this.ChangeBoardSkin();
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
            BoardManager.Instance.SetWhiteTurn();

            #if !SEENSIOGO
                this.SetWhitePlayer(!this.whitePlayer);
            #else
                if (this.IsMyTurn())
                {
                    IGameRoomManager.Instance.TakeOwnerShip();
                }
            #endif
        }

        public async void ChangeBoardSkin()
        {
            BufferData bufferData = await IGameRoomManager.Instance.GetBufferRoomData<BufferData>();
            GameManagerBufferData.Instance.SetBufferSkinData(bufferData.boardAppearanceIndex);
            ARChessGameSettings.Instance.SetBoardAppearanceIndex(bufferData.boardAppearanceIndex);
            BoardManager.Instance.SetBoardSkin();
        }

        public bool IsMyTurn()
        {
            return whitePlayer == BoardManager.Instance.GetWhiteTurn();
        }
    }
}