namespace steam
{
    public class Config
    {

        public class Settings
        {
            public int startId { get; set; }
            public int endId { get; set; }
            public int requestLimit { get; set; }
            public bool checkHotmail { get; set; }
        }

        public class UserInfo
        {
            public string status { get; set; }
            public string steamid { get; set; }
        }

        public class Account
        {
            public string username { get; set; }
            public string email { get; set; }
            public string steamid { get; set; }
            public string url { get; set; }
        }

        public class RootObject
        {
            public string url { get; set; }
            public string SteamId { get; set; }
            public bool CommunityBanned { get; set; }
            public bool Boolean_0 { get; set; }
            public int NumberOfVACBans { get; set; }
            public int DaysSinceLastBan { get; set; }
            public int NumberOfGameBans { get; set; }
            public string EconomyBan { get; set; }
        }
    }
}
