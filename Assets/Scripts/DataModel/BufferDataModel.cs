using System;

namespace JKTechnologies.SeensioGo.ARChess
{
    [Serializable]
    public class BufferData
    {
        public int boardAppearanceIndex;

        public string[] boardPieceState;

        public string masterName;
        public string guestName;
    }
}