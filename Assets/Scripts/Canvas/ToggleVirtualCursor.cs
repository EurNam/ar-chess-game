using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JKTechnologies.SeensioGo.GameEngine;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class ToggleVirtualCursor : MonoBehaviour
    {
        public UnityEngine.UI.Button toggleButton;
        public bool isToggle;
        public int points;

        private void Start()
        {
            toggleButton.onClick.AddListener(ToggleVirtualMouse);
            this.gameObject.SetActive(false);
        }

        private async void ToggleVirtualMouse()
        {
            if (VirtualMouse.Instance != null && isToggle)
            {
                VirtualMouse.Instance.ToggleVirtualMouse();
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