using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKTechnologies.SeensioGo.ARChess.DataModel;

namespace JKTechnologies.SeensioGo.ARChess
{
    public class GameManagerBufferData: MonoBehaviour
    {
        public static GameManagerBufferData Instance;
        private BufferData bufferData = new BufferData();
        private UserPointData[] leaderboard;
        private UserPointData userPointData;

        private void Awake()
        {
            Instance = this;
            // bufferData.boardAppearanceIndex = 0;
            // bufferData.boardPieceState = new string[32];
            // for (int i = 0; i < 32; i++)
            // {
            //     bufferData.boardPieceState[i] = i.ToString();
            // }
        }


        #region Buffer Data
        public BufferData GetDefaultBufferData()
        {
            BufferData defaultBufferData = new BufferData();
            defaultBufferData.boardAppearanceIndex = 0;
            defaultBufferData.boardPieceState = new string[32];
            defaultBufferData.boardStoneState = new string[24];
            for (int i = 0; i < 32; i++)
            {
                defaultBufferData.boardPieceState[i] = (i + 1).ToString();
            }
            for (int i = 0; i < 24; i++)
            {
                defaultBufferData.boardStoneState[i] = (i + 1).ToString();
            }
            return defaultBufferData;
        }

        public void SetBufferData(BufferData bufferData)
        {
            this.bufferData = bufferData;
        }

        public BufferData GetBufferData()
        {
            return bufferData;
        }

        public void SetBufferSkinData(int boardAppearanceIndex)
        {
            bufferData.boardAppearanceIndex = boardAppearanceIndex;
        }

        public int GetBufferSkinData()
        {
            return bufferData.boardAppearanceIndex;
        }

        public void SetMasterName(string masterName)
        {
            bufferData.masterName = masterName;
        }

        public string GetMasterName()
        {
            return bufferData.masterName;
        }

        public void SetGuestName(string guestName)
        {
            bufferData.guestName = guestName;
        }

        public string GetGuestName()
        {
            return bufferData.guestName;
        }

        public void SetBufferPiecesData(string[] boardPieceState)
        {
            bufferData.boardPieceState = boardPieceState;
        }

        public void SetBufferStonesData(string[] boardStoneState)
        {
            bufferData.boardStoneState = boardStoneState;
        }

        public string[] GetBufferPiecesData()
        {
            return bufferData.boardPieceState;
        }

        public string[] GetBufferStonesData()
        {
            return bufferData.boardStoneState;
        }

        public void SetBufferPieceData(Piece piece, int index)
        {
            if (piece == null)
            {
                bufferData.boardPieceState[index] = "";
            }
            else
            {
                bufferData.boardPieceState[index] = piece.GetPieceIndex().ToString();
            }
        }

        public void SetBufferStoneData(Stone stone, int index)
        {
            int actualIndex = index - 39; // ID of the first stone is 40, -1 already done to index
            if (stone == null)
            {
                bufferData.boardStoneState[actualIndex] = "";
            }
            else
            {
                bufferData.boardStoneState[actualIndex] = stone.GetStoneIndex().ToString();
            }
        }
        #endregion

        #region Persistent Data
        public void SetLeaderboardData(UserPointData[] leaderboard)
        {
            this.leaderboard = leaderboard;
        }

        public UserPointData[] GetLeaderboardData()
        {
            return this.leaderboard;
        }

        public void SetUserPointData(UserPointData userPointData)
        {
            this.userPointData = userPointData;
        }

        public UserPointData GetUserPointData()
        {
            return this.userPointData;
        }
        #endregion
    }
}