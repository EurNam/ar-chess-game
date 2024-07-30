using System.Threading.Tasks;
using UnityEngine;

namespace JKTechnologies.SeensioGo.GameEngine
{
    public interface IGameRoomManager
    {
        public static IGameRoomManager Instance;
        public void SetGameInstance(IGameInstance gameManager, string gameId);
        public Task<T> GetGameRoomSettings<T>();
        public bool IsRoomMaster();
        
        public void ScatterDataToRoom(object data);
        public void RegisterGameDataListener (IGameDataListener gameDataListener);

        public void ScatterActionToRoom(string actionName); // -> IGameActionListen will receive this
        public void RegisterGameActionListener(IGameActionListener gameActionListener);
        public void RegisterRPCToGameRoom(IGameRPC gameRPC);
        public void UnregisterRPCToGameRoom(IGameRPC gameRPC);
        public void ScatterRPCActionToRoom(IGameRPC gameRPC, string actionName); // IGameRPC will receive this
        public void TakeOwnerShip();
    }

    public interface IGameInstance
    {
    }

    public interface IGameDataListener
    {
        public void OnDataInRoomReceived(object data);
    }
    public interface IGameActionListener
    {
        // SwitchTurn
        public void OnActionReceived(string actionName);
    }
    /// <summary>
    /// Interface for RPC, you need IGameRPC inheritance MonoBehaviour, if not, it will not work
    /// </summary>
    public interface IGameRPC 
    {
        public void OnRPCActionReceived(string actionName);
        // public int GetRPCID();
    }
}