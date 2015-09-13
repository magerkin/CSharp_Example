using System;
using System.IO;
using System.Linq;

namespace HomeWorkCasino
{
    class Logger
    {
        private readonly string _logFilePath;
        private readonly string _logFileDir;
        private const char Delimiter = '|';

        public Logger(string username)
        {
            _logFileDir = @".\log";
            _logFilePath = Path.Combine(_logFileDir, (username + ".txt"));
            
            if (!File.Exists(_logFilePath))
            {
                if (!File.Exists(_logFileDir))
                {
                    Directory.CreateDirectory(_logFileDir);
                }
                Log("0" + Delimiter + "Balance=0");
            }
        }

        internal void Log(string message) {
            using (StreamWriter file = new StreamWriter(_logFilePath, true))
            {
                file.WriteLine(message);
            }
        }

        internal void Log(int newBalance)
        {
            Log("0" + Delimiter + "Balance=" + newBalance.ToString());
        }

        internal void Log(string gameNumber, string gameData, int newBalance)
        {
            Log(gameNumber + Delimiter + gameData + Delimiter + "Balance=" + newBalance); 
        }

        internal string GetLastString()
        {
          return File.ReadLines(_logFilePath).Last();          
        }

        internal int GetUsersBalance()
        {
            var lastActionString = GetLastString();
            var strBalance = lastActionString.Substring(lastActionString.LastIndexOf('=') + 1);
            return Convert.ToInt32(strBalance);
        }
    }
}
