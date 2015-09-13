using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using CasinoAnalysis;

namespace HomeWorkCasino
{
    class Program
    {
        static void Main(string[] args)
        {
           // var casino = new Casino();
            //casino.Start(@".\log\Log.txt");
            User bobo = new User();
            bobo.Name = "Bobo";
            bobo.Balabce = 0;
            CasinoAnalytic casino = new CasinoAnalytic(@".\log\Log.txt");
            var v1 = casino.MostLuckyUser(GameCode.BJ);
            Console.WriteLine(v1.Name, "\n");
            var v2= casino.MaxLuckyUser();
            Console.WriteLine(v2.Name, "\n");
            Console.WriteLine("\n");
 
            var v3 = casino.UserDeposite(bobo,  DateTimeOffset.Parse("2015-04-13T01:00:37.4337237+03:00").DateTime);
            Console.WriteLine(v3.ToString());
            Console.WriteLine();
            var v4 = casino.ZeroBasedBalanceHistoryExchange(bobo);
            v4.All(x =>
            {
                Console.Write(x.ToString(), " ");
                return true;
            });
            var v5 = casino.NewUsersByMounths();
            var v6 = casino.NewUsersByMounths(GameCode.BJ);
        }
    }
}
