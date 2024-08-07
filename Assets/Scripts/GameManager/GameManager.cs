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
        [SerializeField] private Leaderboard leaderboardManager;
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
            // Room Settings
            IGameRoomManager.Instance.SetGameInstance(this,gameID);
            IGameRoomManager.Instance.RegisterGameActionListener(this);
            isRoomMaster = IGameRoomManager.Instance.IsRoomMaster();
            roomHost = isRoomMaster;
            if (!isRoomMaster)
            {
                TileAppearanceButton.Instance.HideButton();
            }
            m_gameSettings = await IGameRoomManager.Instance.GetGameRoomSettings<GameSettings>();
            this.SetGameSettings();

            // Buffer Data
            BufferData bufferData = null;
            if (isRoomMaster)
            {
                bufferData = GameManagerBufferData.Instance.GetDefaultBufferData();
                string masterName = IGameRoomManager.Instance.GetDisplayName();
                bufferData.masterName = masterName;
                bufferData.guestName = "";
                await IGameRoomManager.Instance.SetBufferRoomData(bufferData);
            }
            else
            {
                bufferData = await IGameRoomManager.Instance.GetBufferRoomData<BufferData>();
                string guestName = IGameRoomManager.Instance.GetDisplayName();
                bufferData.guestName = guestName;
                await IGameRoomManager.Instance.SetBufferRoomData(bufferData);
            }
            IGameRoomManager.Instance.ScatterActionToRoom("UpdateNameTags");

            if (bufferData != null)
            {
                GameManagerBufferData.Instance.SetBufferData(bufferData);
            }
            else
            {
                Debug.LogError("Buffer data is null");
                return;
            }

            ARChessGameSettings.Instance.SetBoardSkin(bufferData.boardAppearanceIndex);
            BoardManager.Instance.UpdatePieceCaptureState(bufferData.boardPieceState);

            // Persistant Data
            UserPointData[] leaderboard = await IGameRoomManager.Instance.GetLeaderBoard();
            UserPointData userPointData = await IGameRoomManager.Instance.GetCurrentUserPoints();
            leaderboardManager.SetLeaderboardData(leaderboard, userPointData);

            // if (isRoomMaster || !IGameRoomManager.Instance.IsMultiplayerRoom())
            // {
            //     BufferData bufferData = GameManagerBufferData.Instance.GetBufferData();
            //     await IGameRoomManager.Instance.SetBufferRoomData(bufferData);
            // }
            // else 
            // {
            //     BufferData bufferData = await IGameRoomManager.Instance.GetBufferRoomData<BufferData>();
            //     GameManagerBufferData.Instance.SetBufferData(bufferData);
            //     ARChessGameSettings.Instance.SetBoardSkin(bufferData.boardAppearanceIndex);
            //     BoardManager.Instance.UpdatePieceCaptureState(bufferData.boardPieceState);
            // }
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
            // Debug.Log("Is white player: " + whitePlayer);
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
            if (IGameRoomManager.Instance.IsMultiplayerRoom())
            {
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
            else
            {
                if (whitePlayer)
                {
                    whitePlayer = true;
                }
            }
        }

        // Switch room turn
        public void SwitchRoomTurn()
        {
            IGameRoomManager.Instance.ScatterActionToRoom("SwitchTurn");
        }

        public void EndRoomGame()
        {
            IGameRoomManager.Instance.ScatterActionToRoom("EndGame");
        }

        public async void ChangeRoomBoardSkin()
        {
            await IGameRoomManager.Instance.SetBufferRoomData(GameManagerBufferData.Instance.GetBufferData());
            IGameRoomManager.Instance.ScatterActionToRoom("ChangeBoardSkin");
        }

        public void OnActionReceived(string actionName)
        {
            // Debug.Log("Action received: " + actionName);
            Invoke(actionName, 0.1f);
        }

        // 
        public async void UpdateNameTags()
        {
            BufferData bufferData = await IGameRoomManager.Instance.GetBufferRoomData<BufferData>();
            GameManagerBufferData.Instance.SetBufferData(bufferData);
            if (roomHost)
            {
                NameTag.Instance.SetMasterName(bufferData.masterName);
                NameTag.Instance.SetGuestName(bufferData.guestName);
            } 
            else
            {
                NameTag.Instance.SetMasterName(bufferData.guestName);
                NameTag.Instance.SetGuestName(bufferData.masterName);
            }
        }

        // Update local turn and board state pieces position
        public void SwitchTurn()
        {
            BoardManager.Instance.SetWhiteTurn();

            if (!IGameRoomManager.Instance.IsMultiplayerRoom())
            {
                this.SetWhitePlayer(!this.whitePlayer);
            } 
            else
            { 
                if (this.IsMyTurn())
                {
                    IGameRoomManager.Instance.TakeOwnerShip();
                }
            }
        }

        public void EndGame()
        {
            ARChessGameSettings.Instance.SetGameStarted(false);
            if (this.IsMyTurn())
            {
                Debug.Log("You Lost");
                object updateUserInfo = -1;
                IGameRoomManager.Instance.SetPersistentData<object>(updateUserInfo);
            }
            else
            {
                Debug.Log("You Won");
                object updateUserInfo = 1;
                IGameRoomManager.Instance.SetPersistentData<object>(updateUserInfo);
            }
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