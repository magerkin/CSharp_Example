using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CasinoAnalysis;

namespace HomeWorkCasino
{
    internal class CasinoAnalytic : ICasinoAnalytic
    {
        private UnifiedLogger _logger;

        public CasinoAnalytic(string loggerPath)
        {
            _logger = new UnifiedLogger(loggerPath);
        }

        public GameCode GetGameCodeFromRecord(BetRecord bet)
        {
            GameCode res = 0;
            switch (bet.GameCode)
            {
                case 1:
                    res = GameCode.BJ;
                    break;
                case 2:
                    res = GameCode.Dice;
                    break;
                case 3:
                    res = GameCode.Roulet;
                    break;
            }
            return res;
        }

        public GameCode GetGameCodeFromRecord(GameResultRecord gameResult)
        {
            GameCode res = 0;
            switch (gameResult.GameCode)
            {
                case 1:
                    res = GameCode.BJ;
                    break;
                case 2:
                    res = GameCode.Dice;
                    break;
                case 3:
                    res = GameCode.Roulet;
                    break;
            }
            return res;
        }

        public GameResultStatus GetGameResultStatus(GameResultRecord gameResult)
        {
            GameResultStatus res = 0;
            switch (gameResult.Status)
            {
                case 'L':
                    res = GameResultStatus.Loose;
                    break;
                case 'W':
                    res = GameResultStatus.Win;
                    break;
            }
            return res;
        }

        public User GetUserFromRecord(Record record)
        {
            return new User { Name = record.Username, 
                Balabce = _logger.GetUserBalance(record.Username, record.Date.DateTime) };
        }

        public GameResults GetResultFromRecord(GameResultRecord gameResult)
        {
            GameResults result = null;
            switch (gameResult.GameCode)
            {
                case 1:
                    var scoresBJ = gameResult.GameInfo.Split(' ').Select(Int32.Parse).ToArray();
                    result = new BlackJackGameResults(gameResult.Date.DateTime, GetUserFromRecord(gameResult),
                        GetGameResultStatus(gameResult), gameResult.BalanceChange,
                        scoresBJ[0], scoresBJ[1]);
                    break;
                case 2:
                    var scoresDice = gameResult.GameInfo.Split(' ').Select(Int32.Parse).ToArray();
                    result = new DiceGameResults(gameResult.Date.DateTime, GetUserFromRecord(gameResult),
                        GetGameResultStatus(gameResult), gameResult.BalanceChange,
                        scoresDice[0], scoresDice[1]);
                    break;
                case 3:
                    var dataRoulet = gameResult.GameInfo.Split(' ').ToArray();
                    string userChoise = dataRoulet[0];
                    var cell = Int32.Parse(dataRoulet[1]);
                    result = new RouletGameResults(gameResult.Date.DateTime, GetUserFromRecord(gameResult),
                        GetGameResultStatus(gameResult), gameResult.BalanceChange,
                        userChoise, cell);
                    break;
            }
            return result;
        }

        public Bet GetBetFromRecord(BetRecord bet)
        {
            DateTime newDate = bet.Date.DateTime;
            User newUser = GetUserFromRecord(bet);
            GameCode newCode = GetGameCodeFromRecord(bet);
            Bet newBet = new Bet { Date = newDate, User = newUser, GameCode = newCode, Value = bet.Amount};
            return newBet;
        }

        public Deposite GetDepositeFromRecord(DepositeRecord deposite)
        {
            DateTime newDate = deposite.Date.DateTime;
            User newUser = GetUserFromRecord(deposite);
            Deposite newDeposite = new Deposite { User = newUser, Date = newDate, Value = deposite.Amount };
            return newDeposite;
        }



        public IEnumerable<Bet> Bets
        {
            get { return _logger.GetBets().Select(bet => GetBetFromRecord(bet)); }
        }

        public IEnumerable<Deposite> Deposites
        {
            get { return _logger.GetDeposites().Select(deposite => GetDepositeFromRecord(deposite)); }
        }

        public IEnumerable<GameResults> GamesResults
        {
            get { return _logger.GetResults().Select(result => GetResultFromRecord(result)); }
        }

        public Bet MaxBet()
        {
            return Bets.OrderByDescending(x => x.Value).First();
        }

        public Bet MaxBet(GameCode code)
        {
            return Bets.Where(x => x.GameCode == code).
                OrderByDescending(x => x.Value).First();
        }

        public int AverageDeposite()
        {
            var v = Deposites.Select(x => x.Value);
            return (int) Deposites.Select(x => x.Value).Average();
        }

        public int AverageDeposite(User user)
        {
            return (int) Deposites.Where(x => x.User.Name == user.Name).Select(x => x.Value).Average();
        }

        public IEnumerable<Deposite> TopDeposites(int count)
        {
            return Deposites.OrderByDescending(x => x.Value).Take(count);
        }

        public IEnumerable<User> RichestClients(int count)
        {
            return Deposites.GroupBy(x => x.User.Name).Select(x => new {Name = x.Key, user = x.First().User, AllMoney = x.Sum(y => y.Value)})
                .OrderByDescending(x => x.AllMoney).Take(count).Select(x => x.user);
        }

        // надеюсь, тут имелся в виду суммарный profit, а не за одну игру
        public GameCode MaxProfitGame(out int amount)
        {
            var res = GamesResults.Where(x => x.BalanceChange < 0).GroupBy(x => x.GameCode)
                .Select(x => new {gameCode = x.Key, profit = -1*x.Sum(y => y.BalanceChange)})
                .OrderByDescending(x => x.profit);
            amount = res.Select(x => x.profit).First();
            return res.Select(x => x.gameCode).First();
        }

        public User MostLuckyUser(GameCode game)
        {
            return GamesResults.Where(x => x.GameCode == game).GroupBy(x => x.User.Name)
                .Select(x => new
                {
                    user = x.First().User,
                    wons = x.Count(y => y.Status == GameResultStatus.Win),
                    losses = x.Count(y => y.Status == GameResultStatus.Loose)
                })
                .OrderByDescending(x => x.wons/x.losses).First().user;
        }

        public User MaxLuckyUser()
        {
            return GamesResults.GroupBy(x => x.User.Name)
                .Select(x => new
                {
                    user = x.First().User,
                    wons = x.Count(y => y.Status == GameResultStatus.Win),
                    losses = x.Count(y => y.Status == GameResultStatus.Loose)
                })
                .OrderByDescending(x => x.wons / x.losses).First().user;
        }

        public int UserDeposite(User user, DateTime to)
        {
            return _logger.GetUserDeposites(user.Name, to);
        }

        public IEnumerable<int> ZeroBasedBalanceHistoryExchange(User user)
        {
            List<int> res = new List<int> {0};
            foreach (var gameRes in GamesResults.Where(x => x.User.Name == user.Name).OrderBy(x => x.Date).ToList())
            {
                res.Add(res.Last() + gameRes.BalanceChange);
            }
            res.Remove(0);
            return res;
        }

        public IEnumerable<int> ZeroBasedBalanceHistoryExchange(User user, DateTime @from)
        {
            List<int> res = new List<int> {0};
            foreach (var gameRes in GamesResults.Where(x => (x.User.Name == user.Name) && (x.Date >= @from)).OrderBy(x => x.Date).ToList())
            {
                res.Add(res.Last() + gameRes.BalanceChange);
            }
            res.Remove(0);
            return res;
        }

        // не понял, как должна быть отсортирована. По первому дню, когда в данном месяце играли?
        // и почему ключ DateTime? Как я понимаю, это должен быть месяц, а data.Month -- это int.
        // Пока положу первую дату месяца как ключ.
        public Dictionary<DateTime, int> ProfitByMonths()
        {
            return GamesResults.Where(x => x.BalanceChange < 0).GroupBy(x => x.Date.Month)
                .Select(x => new
                {
                    month = x.Key, 
                    firstDate = x.OrderBy(y => y.Date).First().Date,
                    profit = -1*x.Sum(y => y.BalanceChange)
                }).OrderBy(x => x.firstDate).ToDictionary(x => x.firstDate, x => x.profit);
        }

        public Dictionary<DateTime, int> GamesCountByMounths()
        {
            return GamesResults.GroupBy(x => x.Date.Month)
                .Select(x => new
                {
                    firstDate = x.OrderBy(y => y.Date).First().Date,
                    count = x.Count()
                }).OrderBy(x => x.firstDate).ToDictionary(x => x.firstDate, x => x.count);
        }

        public Dictionary<DateTime, int> GamesCountByMounths(GameCode game)
        {
            return GamesResults.Where(x => x.GameCode == game).GroupBy(x => x.Date.Month)
                .Select(x => new
                {
                    firstDate = x.OrderBy(y => y.Date).First().Date,
                    count = x.Count()
                }).OrderBy(x => x.firstDate).ToDictionary(x => x.firstDate, x => x.count);
        }

        public Dictionary<DateTime, int> NewUsersByMounths()
        {
            return Deposites.GroupBy(x => x.User.Name)
                .Select(x => new {date = x.OrderBy(y => y.Date).First().Date})
                .GroupBy(x => x.date.Month)
                .Select(x => new {date = x.First().date, count = x.Count()})
                .ToDictionary(x => x.date, x => x.count);
        }

        public Dictionary<DateTime, int> NewUsersByMounths(GameCode game)
        {
            return GamesResults.Where(x => x.GameCode == game).GroupBy(x => x.User.Name)
                .Select(x => new { date = x.OrderBy(y => y.Date).First().Date })
                .GroupBy(x => x.date.Month)
                .Select(x => new { date = x.First().date, count = x.Count() })
                .ToDictionary(x => x.date, x => x.count);
        }
    }
}
