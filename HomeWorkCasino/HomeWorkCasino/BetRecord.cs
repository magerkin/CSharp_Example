using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkCasino
{
    class BetRecord : Record
    {
        public int GameCode { get; private set; }
        public int Amount { get; private set; }

        public BetRecord(DateTimeOffset date, string username, int gamecode, int amount)
        {
            Type = "Bet";
            Date = date;
            Username = username;
            GameCode = gamecode;
            Amount = amount;
        }

        public BetRecord(string username, int gamecode, int amount) :
            this(DateTimeOffset.Now, username, gamecode, amount)
        {
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1} {2} BET:{3}", Date.ToString("O"), Username, GameCode, Amount);
        }
    }
}
