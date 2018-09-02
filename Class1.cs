using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SteamKit2;
using Newtonsoft.Json.Linq;

namespace steam
{
    //the important class
    class Class1
    {
        public static Config.UserInfo RetrieveInfo(string username, string password)
        {
            Class1.Username = username;
            Class1.Password = password;
            Class1.steamClient = new SteamClient(ProtocolType.Tcp);
            Class1.callbackManager = new CallbackManager(Class1.steamClient);
            Class1.steamUser = Class1.steamClient.GetHandler<SteamUser>();
            Class1.callbackManager.Subscribe<SteamClient.ConnectedCallback>(new Action<SteamClient.ConnectedCallback>(Class1.IsConnected));
            Class1.callbackManager.Subscribe<SteamClient.DisconnectedCallback>(new Action<SteamClient.DisconnectedCallback>(Class1.IsDisconnected));
            Class1.callbackManager.Subscribe<SteamUser.LoggedOnCallback>(new Action<SteamUser.LoggedOnCallback>(Class1.LogIn));
            Class1.callbackManager.Subscribe<SteamUser.LoggedOffCallback>(new Action<SteamUser.LoggedOffCallback>(Class1.LogOff));
            Class1.Running = true;
            
         // SteamDirectory
            SteamDirectory.Initialize(0u).Wait();
            Class1.steamClient.Connect(null);
            while (Class1.Running)
            {
                Class1.callbackManager.RunWaitCallbacks(TimeSpan.FromSeconds(3.0));
            }
            Config.UserInfo UserInfo = new Config.UserInfo();
            UserInfo.status = Class1.statusStr;
            UserInfo.steamid = Convert.ToString(Class1.steamid);
            Class1.steamClient.Disconnect();

            return UserInfo;
        }

        private static void IsConnected(SteamClient.ConnectedCallback callback)
        {
            //incase unable to connect  
            if (callback.Result != EResult.OK)
            {
                Class1.statusStr = "Con.Error";
                Class1.Running = false;
                return;
            }

            //try to log into the user
            Class1.steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = Class1.Username,
                Password = Class1.Password
            });
        }

        private static void IsDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Class1.Running = false;
        }

        private static void LogIn(SteamUser.LoggedOnCallback callback)
        {
            Class1.steamid = callback.ClientSteamID.ConvertToUInt64();
            if (callback.Result == EResult.OK)
            {
                Class1.statusStr = "Logged in";
                Class1.steamUser.LogOff();
                return;
            }
            if (callback.Result == EResult.AccountLogonDenied)
            {
                // if result = AccountLogonDenied then the account has steamguard on 
           
                Class1.statusStr = "Steamguard";
                Class1.Running = false;
                return;
            }
            Class1.statusStr = "Wrong password";
            Class1.Running = false;
        }

        private static void LogOff(SteamUser.LoggedOffCallback callback)
        {
           // log off from the account
        }

        static Class1()
        {
            Class1.statusStr = "ayylmao";
            Class1.steamid = 0UL;
        }

        private static SteamClient steamClient;
        private static CallbackManager callbackManager;
        private static SteamUser steamUser;
        private static bool Running;
        private static string Username;
        private static string Password;
        private static string statusStr;
        private static ulong steamid;
    }
}
