// using System;
// using System.Diagnostics;
// using System.Threading.Tasks;
// using JKTechnologies.Seensio;
// using Newtonsoft.Json;
// using UnityEngine;

// namespace JKTechnologies.SeensioGo.GameEngine
// {
//     public enum GunType
//     {
//         ShotGun = 0,
//         MiniGun = 1,
//         Sniper = 2
//     }
//     public interface ITrackingUserInfo
//     {
//         public static ITrackingUserInfo Instance;
//         public void OnEnterNewGame();
//         public void OnExitGame();
//         public void OnUseMagazines(int mount, GunType gunType);
//         public void OnEarnedFish();
//         public void OnEarnedGold(int gold);
//     }
// }