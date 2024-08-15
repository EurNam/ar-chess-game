using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKTechnologies.SeensioGo.GameEngine;
using JKTechnologies.SeensioGo.ARPortal;
using UnityEngine.Events;
using Unity.VisualScripting;

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
        public UnityEngine.UI.Button startButton;
        public UnityEngine.UI.Button skinButton;
        public string gameID;
        public NameTag myName;
        public NameTag opponentName;
        public ARPortalController portalController;
        [SerializeField] private Leaderboard leaderboardManager;
        public GameObject[] gameTypes;
        public bool boardInitialized = false;
        public UnityEvent OnBoardInitialized = new UnityEvent();
        private string m_playerID;
        private string[] m_old_gameSettings = new string[4]{"","","",""}; // 1: White Side, 2: Black Side, 3: Room Host, 4: Tile Skin
        private bool whitePlayer = true;
        private bool roomHost = true;
        private int skinIndex = 0;
        private bool isRoomMaster = true; 
        private GameSettings m_gameSettings = new GameSettings();
        private bool chessGame = true;
        private bool checkersGame = false; 

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
                startButton.gameObject.SetActive(true);
                skinButton.gameObject.SetActive(true);

                bufferData = GameManagerBufferData.Instance.GetDefaultBufferData();
                string masterName = IGameRoomManager.Instance.GetDisplayName();
                bufferData.masterName = masterName;
                bufferData.guestName = "";
            }
            else
            {
                bufferData = await IGameRoomManager.Instance.GetBufferRoomData<BufferData>();
                if (bufferData.guestName != "")
                {
                    Debug.Log("Viewer");
                }
                else
                {
                    startButton.gameObject.SetActive(true);
                    skinButton.gameObject.SetActive(true);
                    string guestName = IGameRoomManager.Instance.GetDisplayName();
                    bufferData.guestName = guestName;
                }
            }
            if (bufferData != null)
            {
                GameManagerBufferData.Instance.SetBufferData(bufferData);
                await IGameRoomManager.Instance.SetBufferRoomData(bufferData);
                boardInitialized = true;
                OnBoardInitialized.Invoke();
            }
            else
            {
                Debug.LogError("Buffer data is null");
                return;
            }
            IGameRoomManager.Instance.ScatterActionToRoom("UpdateNameTags");

            ARChessGameSettings.Instance.SetBoardSkin(bufferData.boardAppearanceIndex);
            BoardManager.Instance.UpdatePieceCaptureState(bufferData.boardPieceState);

            // Persistant Data
            UserPointData[] leaderboard = await IGameRoomManager.Instance.GetLeaderBoard();
            UserPointData userPointData = await IGameRoomManager.Instance.GetCurrentUserPoints();
            myName.SetMasterName(userPointData.displayName);

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

        public bool GetChessGame()
        {
            return chessGame;
        }

        public bool GetCheckersGame()
        {
            return checkersGame;
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

        public void SetChessGame(bool chessGame)
        {
            this.chessGame = chessGame;
        }

        public void SetCheckersGame(bool checkersGame)
        {
            this.checkersGame = checkersGame;
        }

        public void SetRoomSkin(int skinIndex)
        {
            this.skinIndex = skinIndex;
        }

        public void SetGameSettings()
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

        public async void EndRoomGame()
        {
            await IGameRoomManager.Instance.UpdateUserPoints(1);
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
            if (roomHost)
            {
                // myName.SetMasterName(bufferData.masterName);
                opponentName.SetNameTag(bufferData.guestName);
            } 
            else
            {
                // myName.SetGuestName(bufferData.guestName);
                opponentName.SetNameTag(bufferData.masterName);
            }
        }

        // Update local turn and board state pieces position
        public void SwitchTurn()
        {
            if (chessGame)
            {
                BoardManager.Instance.SetWhiteTurn();
            }
            else if (checkersGame)
            {
                CheckersBoardManager.Instance.SetWhiteTurn();
            }

            if (!IGameRoomManager.Instance.IsMultiplayerRoom())
            {
                this.SetWhitePlayer(!this.whitePlayer);
            } 
            else
            { 
                if (this.IsMyTurn() && ARChessGameSettings.Instance.GetGameStarted())
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
            if (bufferData != null)
            {
                GameManagerBufferData.Instance.SetBufferSkinData(bufferData.boardAppearanceIndex);
                ARChessGameSettings.Instance.SetBoardAppearanceIndex(bufferData.boardAppearanceIndex);
            }
            else
            {
                ARChessGameSettings.Instance.SetBoardAppearanceIndex(this.skinIndex);
            }
            BoardManager.Instance.SetBoardSkin();
            CheckersBoardManager.Instance.SetBoardSkin();
        }

        public bool IsMyTurn()
        {
            return whitePlayer == BoardManager.Instance.GetWhiteTurn();
        }
    }
}