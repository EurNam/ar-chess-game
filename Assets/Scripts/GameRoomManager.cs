using UnityEngine;

namespace JKTechnologies.SeensioGo.GameEngine
{
    public class GameRoomManager : MonoBehaviour
    {
        public static GameRoomManager Instance;
        public bool isMultiplayerMode = false;
        private object m_gameSettings;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }  

        public string GetPlayerID()
        {
            return "ID";
        }

        public void SetGameSettings(object gameSettings)
        {
            m_gameSettings = gameSettings;
        }

        public object GetGameSettings()
        {
            return m_gameSettings;
        }

        public void OnSetTurnAction(string action)
        {
            Debug.Log("OnSetTurnAction: " + action);
        }
    }
}