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

        public BufferData GetDefaultBufferData()
        {
            BufferData defaultBufferData = new BufferData();
            defaultBufferData.boardAppearanceIndex = 0;
            defaultBufferData.boardPieceState = new string[32];
            for (int i = 0; i < 32; i++)
            {
                defaultBufferData.boardPieceState[i] = (i + 1).ToString();
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

        public void SetBufferPiecesData(string[] boardPieceState)
        {
            bufferData.boardPieceState = boardPieceState;
        }

        public string[] GetBufferPiecesData()
        {
            return bufferData.boardPieceState;
        }

        public void SetBufferPieceData(Piece piece, int index)
        {
            if (piece == null)
            {
                bufferData.boardPieceState[index] = "";
            }
            else
            {
                bufferData.boardPieceState[index] = piece.name;
            }
        }
    }
}