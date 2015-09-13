using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkCasino
{
    class DepositeRecord : Record
    {
        public int Amount { get; private set; }

        public DepositeRecord(DateTimeOffset date, string username, int amount)
        {
            Type = "Deposite";
            Date = date;
            Username = username;
            Amount = amount;
        }

        public DepositeRecord(string username, int amount)
            : this(DateTimeOffset.Now, username, amount)
        {
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1} DEPOSITE:{2}", Date.ToString("O"), Username, Amount);
        }
    }
}
