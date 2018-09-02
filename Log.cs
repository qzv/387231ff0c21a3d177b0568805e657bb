using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace steam
{
    class Log
    {
        private string logPath;
        private int logQueueSize;
        private List<string> logQueue;

        public Log(string LogPath, int QueueSize)
        {
            logQueue = new List<string>();
            logPath = LogPath;
            logQueueSize = QueueSize;

            if (!File.Exists(logPath))
            {
                File.Create(logPath).Close();
                Console.WriteLine("Log file created");
                Thread.Sleep(250);
            }
        }

        public void Write(Config.Account account)
        {
            string logMessage = string.Format("{0}\n{1}\n{2}\n{3}\n",
                account.username,
                account.email,
           account.steamid,
           account.url);
            logQueue.Add(logMessage);
            FlushLog();
        }

        public void FlushLog(bool ignoreCondition = false)
        {
            if (ignoreCondition || logQueue.Count < logQueueSize)
                return;

            lock (logQueue)
            {
                using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (var str in logQueue)
                        {
                            sw.WriteLine(str);
                        }

                        logQueue.Clear();
                    }
                }
            }
        }
    }
}
