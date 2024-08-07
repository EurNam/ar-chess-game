using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class StartButton : MonoBehaviour
    {
        public UnityEngine.UI.Button toggleButton;

        public bool isStart;
        public int points;

        void Start()
        {
            toggleButton.onClick.AddListener(StartGame);
        }

        public async void StartGame()
        {   
            if (isStart)
            {
                ARChessGameSettings.Instance.SetGameStarted(true);
                // GameRoomManager.Instance.StartGameRoom();
            } 
            else
            {
                await IGameRoomManager.Instance.UpdateUserPoints(points);
                UserPointData[] leaderboard = await IGameRoomManager.Instance.GetLeaderBoard();
                UserPointData userPointData = await IGameRoomManager.Instance.GetCurrentUserPoints();
                Leaderboard.Instance.SetLeaderboardData(leaderboard, userPointData);
            }
        }
    }
}