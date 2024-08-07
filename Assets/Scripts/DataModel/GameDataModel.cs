using System;

namespace JKTechnologies.SeensioGo.ARChess.DataModel
{
    public class PersistentData
    {
        public UserPointData[] leaderBoard;
        public UserPointData currentUser;
    }

    [Serializable]
    public class UserPointData
    {
        public long totalPoints;
        public string userId;
        public string displayName;
        public string photoUrl;
    }

    // public class UserGameDTO
    // {
    //     public int numberOfWins;
    //     public int numberOfLosses;
    // }
}