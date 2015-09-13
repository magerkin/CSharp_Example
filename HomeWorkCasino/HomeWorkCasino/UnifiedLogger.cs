using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkCasino
{
    class UnifiedLogger
    {

        private List<Record> _logger;
        private string _loggerPath;

        internal Record GetRecord(string input)
        {
            string[] separators = {"[", "]", " ", "DEPOSITE:", "BET:"};
            if (input.Contains("DEPOSITE"))
            {
                var elements = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                return new DepositeRecord(DateTimeOffset.Parse(elements[0]), elements[1], int.Parse(elements[2]));
            }
            else
            {
                if (input.Contains("BET"))
                {
                    var elements = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    return new BetRecord(DateTimeOffset.Parse(elements[0]), elements[1], int.Parse(elements[2]), int.Parse(elements[3]));
                }
                else
                {
                    var elements = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    return new GameResultRecord(DateTimeOffset.Parse(elements[0]), elements[1], int.Parse(elements[2]), elements[3].ToCharArray()[0],
                        int.Parse(elements[4]), elements[5].ToString() + " " + elements[6].ToString());
                }
            }
        }

        
        public UnifiedLogger(string loggerPath)
        {
            _loggerPath = loggerPath;
            string loggerDirectory = Path.GetDirectoryName(loggerPath);
            if (!Directory.Exists(loggerDirectory))
            {
                Directory.CreateDirectory(loggerDirectory);
            }
            if (File.Exists(loggerPath))
            {
                _logger = File.ReadAllLines(loggerPath).Select(input => GetRecord(input)).ToList();
            }
            else
            {
                using (File.CreateText(loggerPath))
                {
                }
                _logger = File.ReadAllLines(loggerPath).Select(input => GetRecord(input)).ToList();
            }
        }

        public int GetUserBalance(string username)
        {
            var balanceChange = _logger.Where(record => (record.Type == "Game result") && (record.Username == username))
                .Select(record => ((GameResultRecord)record).BalanceChange)
                .Sum();
            return (balanceChange + GetUserDeposites(username));
        }

        public int GetUserBalance(string username, DateTime toDate)
        {
            var balanceChange = _logger
                .Where(record => (record.Type == "Game result") && (record.Username == username) && (record.Date.DateTime <= toDate))
                .Select(record => ((GameResultRecord)record).BalanceChange)
                .Sum();
            return (balanceChange + GetUserDeposites(username, toDate));
        }

        public int GetUserDeposites(string username)
        {
            return _logger.Where(record => (record.Type == "Deposite") && (record.Username == username))
                .Select(record => ((DepositeRecord) record).Amount)
                .Sum();
        }

        public int GetUserDeposites(string username, DateTime toDate)
        {
            return _logger
                .Where(record => (record.Type == "Deposite") && (record.Username == username) && (record.Date.DateTime <= toDate))
                .Select(record => ((DepositeRecord)record).Amount)
                .Sum();
        }

        public IEnumerable<BetRecord> GetBets()
        {
            return _logger.Where(record => record.Type == "Bet").Select(bet => (BetRecord) bet);
        }

        public IEnumerable<DepositeRecord> GetDeposites()
        {
            return _logger.Where(record => record.Type == "Deposite")
                .Select(deposite => (DepositeRecord) deposite);
        }

        public IEnumerable<GameResultRecord> GetResults()
        {
            return _logger.Where(record => record.Type == "Game result")
                .Select(result => (GameResultRecord)result);
        } 

        public void AddNewBet(string username, int gamecode, int amount)
        {
            BetRecord newBet = new BetRecord(username, gamecode, amount);
            _logger.Add(newBet);
            using (StreamWriter file = new StreamWriter(_loggerPath, true))
            {
                file.WriteLine(newBet.ToString());
            }
        }

        public void AddNewDeposite(string username, int amount)
        {
            DepositeRecord newDeposite= new DepositeRecord(username, amount);
            _logger.Add(newDeposite);
            using (StreamWriter file = new StreamWriter(_loggerPath, true))
            {
                file.WriteLine(newDeposite.ToString());
            }
        }

        public void AddNewResult(string username, int gamecode, char status, int balanceChange, string gameInfo)
        {
            GameResultRecord newResult = new GameResultRecord(username, gamecode, status, balanceChange, gameInfo);
            _logger.Add(newResult);
            using (StreamWriter file = new StreamWriter(_loggerPath, true))
            {
                file.WriteLine(newResult.ToString());
            }
        }

        public void AddNewResult(GameResultRecord newResult)
        {
            _logger.Add(newResult);
            using (StreamWriter file = new StreamWriter(_loggerPath, true))
            {
                file.WriteLine(newResult.ToString());
            }
        }

    }
}
