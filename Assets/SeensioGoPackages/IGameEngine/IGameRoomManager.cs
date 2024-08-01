using System.Threading.Tasks;

namespace JKTechnologies.SeensioGo.GameEngine
{
    public interface IGameRoomManager
    {
        public static IGameRoomManager Instance;
        public void SetGameInstance(IGameInstance gameManager, string gameId);

        #region GAME SETTINGs
        public Task<T> GetGameRoomSettings<T>();
        #endregion
        
        #region COMMONs
        public bool IsRoomMaster();
        public string GetDisplayName();
        public void TakeOwnerShip();
        #endregion

        #region PERSISTENT DATAs
        public Task<T> GetPersistentData<T>();
        public Task<bool> SetPersistentData<T>(T data);
        #endregion

        #region BUFFER DATAs
        public Task<T> GetBufferRoomData<T>();
        public Task<bool> SetBufferRoomData<T>(T data);
        #endregion

        #region LEADERBOARDs
        public Task<UserPointData[]> GetLeaderBoard();
        public Task<bool> UpdateUserPoints(int points);
        public Task<UserPointData> GetCurrentUserPoints();
        #endregion

        #region SCATTER DATA
        public void RegisterGameDataListener (IGameDataListener gameDataListener);
        public void ScatterDataToRoom(object data);
        #endregion

        #region SCATTER ACTION
        public void RegisterGameActionListener(IGameActionListener gameActionListener);
        public void ScatterActionToRoom(string actionName); // -> IGameActionListen will receive this
        #endregion

        #region RPC
        public void RPC_RegisterToGameRoom(IGameRPC gameRPC);
        public void RPC_UnregisterToGameRoom(IGameRPC gameRPC);
        public void RPC_ScatterActionToRoom(IGameRPC gameRPC, string actionName); // IGameRPC will receive this
        #endregion
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
        public void RPC_OnActionReceived(string actionName);
        public int RPC_GetID();
    }
}