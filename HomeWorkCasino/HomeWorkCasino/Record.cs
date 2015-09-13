using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkCasino
{
    internal abstract class Record
    {
        public string Type { get; protected set; }
        public DateTimeOffset Date { get; protected set; }
        public string Username { get; protected set; }
    }
}
