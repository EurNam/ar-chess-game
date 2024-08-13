using System.Collections.Concurrent;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace JKTechnologies.SeensioGo.GameEngine
{
    // Simulator: -> 2 room
    public class GameRoomManagerSimulator : MonoBehaviour, IGameRoomManager
    {
        public bool IsMultiplayer = false;
        public GameObject messagePopup;
        public TextMeshProUGUI messagePopupText;
        private IGameInstance m_gameInstance;
        private IGameDataListener m_gameDataListener;
        private IGameActionListener m_gameActionListener;
        private ConcurrentDictionary<int, IGameRPC> m_gameRPCActionTransfers = new ConcurrentDictionary<int, IGameRPC>();
        private string m_gameId;
        private object m_bufferData;
        private void Awake()
        {
            if(IGameRoomManager.Instance == null)
            {
                IGameRoomManager.Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }  
        #region COMMONs
        public bool IsMultiplayerRoom()
        {
            return IsMultiplayer;
        }

        public bool IsRoomMaster()
        {
            return true;
        } 

        public void TakeOwnerShip()
        {
            messagePopup.SetActive(true);
            messagePopupText.text = "Simulator: Take ownership";
        }

        public string GetDisplayName()
        {
            return "Simulator";
        }
        #endregion
        
        #region GAME SETTINGS
        public void SetGameInstance(IGameInstance gameManager, string gameId)
        {
            m_gameInstance = gameManager;
            
        }

        public async Task<T> GetGameRoomSettings<T>()
        {
            if(m_gameInstance == null)
            {
                messagePopup.SetActive(true);
                messagePopupText.text = "Simulator: Game Instance is null";
                return default;
            }
            else if(string.IsNullOrEmpty(m_gameId))
            {
                messagePopup.SetActive(true);
                messagePopupText.text = "Simulator: Game Id is null";
                return default;
            }
            await Task.Yield();
            return default;
        }
        #endregion
     
        #region LEADER BOARD
        public async Task<UserPointData[]> GetLeaderBoard()
        {
            await Task.Yield();
            return new UserPointData[0];
        }


        public async Task<UserPointData> GetCurrentUserPoints()
        {
            await Task.Yield();
            return new UserPointData();
        }

        public async Task<bool> UpdateUserPoints(int points)
        {
            await Task.Yield();
            return true;
        }
        #endregion

        #region PERSISTENT DATA
        public async Task<T> GetPersistentData<T>()
        {   
            await Task.Yield();
            return default;
        }
        public async Task<bool> SetPersistentData<T>(T persistentData)
        {
            await Task.Yield();
            return true;
        }
        #endregion

        #region BUFFER DATA
        public async Task<T> GetBufferRoomData<T>()
        {
            await Task.Yield();
            return (T)m_bufferData; 
        }
        public async Task<bool> SetBufferRoomData<T>(T bufferData)
        {
            m_bufferData = bufferData;
            await Task.Yield();
            return true;
        }

        public async Task<bool> DeleteBufferRoomData()
        {
            await Task.Yield();
            m_bufferData = null;
            return true;
        }
        #endregion

        #region DATA TRANSFER
        public void RegisterGameDataListener (IGameDataListener gameDataListener)
        {
            m_gameDataListener = gameDataListener;
        }
        public void ScatterDataToRoom(object data)
        {
            if(m_gameDataListener == null)
            {
                messagePopup.SetActive(true);
                messagePopupText.text = $"[GameRoomManagerSimulator] ScatterDataToRoom: m_gameDataListener not found";
                return;
            }
            m_gameDataListener.OnDataInRoomReceived(data);
        }
        #endregion
       
        #region ACTION TRANSFER
        public void RegisterGameActionListener(IGameActionListener gameActionListener)
        {
            m_gameActionListener = gameActionListener;
        }

        public void ScatterActionToRoom(string actionName)
        {
            if(m_gameActionListener == null)
            {
                messagePopup.SetActive(true);
                messagePopupText.text = $"[GameRoomManagerSimulator] ScatterActionToRoom: m_gameActionListener not found";
                return;
            }
            m_gameActionListener.OnActionReceived(actionName);
        }
        #endregion

        #region RPC
        public void RPC_RegisterToGameRoom(IGameRPC gameRPC)
        {
            if(m_gameRPCActionTransfers.ContainsKey(gameRPC.RPC_GetID()))
            {
                m_gameRPCActionTransfers[gameRPC.RPC_GetID()] = gameRPC;
            }
            else
            {
                m_gameRPCActionTransfers.TryAdd(gameRPC.RPC_GetID(), gameRPC);
            }
        }
        public void RPC_UnregisterToGameRoom(IGameRPC gameRPC)
        {
            m_gameRPCActionTransfers.TryRemove(gameRPC.RPC_GetID(), out _);
        }
        public void RPC_ScatterActionToRoom(IGameRPC  gameRPC, string actionName)
        {
            if(m_gameRPCActionTransfers.ContainsKey(gameRPC.RPC_GetID()))
            {
                m_gameRPCActionTransfers[gameRPC.RPC_GetID()].RPC_OnActionReceived(actionName);
            }
            else
            {
                messagePopup.SetActive(true);
                messagePopupText.text = $"[GameRoomManagerSimulator] RPC_ScatterActionToRoom: {gameRPC.RPC_GetID()} not found";
            }
        }
        #endregion
    }
}