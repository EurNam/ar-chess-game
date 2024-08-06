using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JKTechnologies.SeensioGo.GameEngine;
using JKTechnologies.SeensioGo.Scene;

namespace JKTechnologies.SeensioGo.ARCheess
{
    public class Leaderboard: MonoBehaviour
    {
        public static Leaderboard Instance;
        public GameObject[] ranks;
        private Image rank1Image;
        private Text rank1Name;
        private Text rank1Wins;
        public GameObject userInfo;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            for (int i = 0; i < ranks.Length; i++)
            {
                GameObject rank = ranks[i];
                
                Image rankImage = GetChildComponentByName<Image>(rank.transform, "Image");
                TextMeshProUGUI nameText = GetChildComponentByName<TextMeshProUGUI>(rank.transform, "Name");
                TextMeshProUGUI winsText = GetChildComponentByName<TextMeshProUGUI>(rank.transform, "Wins");

                if (rankImage != null)
                {
                    rankImage.color = Random.ColorHSV();
                }

                if (nameText != null)
                {
                    nameText.text = $"Player {i + 1}";
                }

                if (winsText != null)
                {
                    winsText.text = $"{Random.Range(0, 100)}";
                }
            }

            TextMeshProUGUI userRank = GetChildComponentByName<TextMeshProUGUI>(userInfo.transform, "Rank");
            Image userImage = GetChildComponentByName<Image>(userInfo.transform, "Image");
            TextMeshProUGUI userName = GetChildComponentByName<TextMeshProUGUI>(userInfo.transform, "Name");
            TextMeshProUGUI userWins = GetChildComponentByName<TextMeshProUGUI>(userInfo.transform, "Wins");

            if (userRank != null)
            {
                userRank.text = $"No. {Random.Range(0, 100)} ";
            }

            if (userImage != null)
            {
                // Set a random color for the rank image
                userImage.color = Random.ColorHSV();
            }

            if (userName != null)
            {
                userName.text = "Bendinh";
            }

            if (userWins != null)
            {
                userWins.text = $"{Random.Range(0, 100)}";
            }
        }

        private T GetChildComponentByName<T>(Transform parent, string name) where T : Component
        {
            Transform child = parent.Find(name);
            if (child != null)
            {
                return child.GetComponent<T>();
            }
            return null;
        }

    }
}