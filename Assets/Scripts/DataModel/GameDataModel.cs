using System;

namespace JKTechnologies.SeensioGo.ARChess.DataModel
{
    public class PersistentData
    {
        public UserGameData[] leaderBoard;
        public UserGameData currentUser;
    }

    public class UserGameData
    {
        // public string displayName;
        public int numberOfWins;
        public int numberOfLosses;
        //public int numberOfDraws;
        //public int playerRanking;
    }

    // public class UserGameDTO
    // {
    //     public int numberOfWins;
    //     public int numberOfLosses;
    // }
}