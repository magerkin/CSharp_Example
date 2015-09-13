using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkCasino
{
    class GameResultRecord : Record
    {
        public int GameCode { get; private set; }
        public char Status { get; private set; }
        public int BalanceChange { get; private set; }
        public string GameInfo { get; private set; }

        public GameResultRecord(DateTimeOffset date, string username, int gamecode, char status, int balanceChange, string gameInfo)
        {
            Type = "Game result";
            Date = date;
            Username = username;
            GameCode = gamecode;
            Status = status;
            BalanceChange = balanceChange;
            GameInfo = gameInfo;
        }

        public GameResultRecord(string username, int gamecode, char status, int balanceChange, string gameInfo) :
            this(DateTimeOffset.Now, username, gamecode, status, balanceChange, gameInfo)
        {
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1} {2} {3} {4} {5}", Date.ToString("O"), Username, GameCode, Status, BalanceChange, GameInfo);
        }
    }
}
