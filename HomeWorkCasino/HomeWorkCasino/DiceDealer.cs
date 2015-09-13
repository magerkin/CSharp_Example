using System;

namespace HomeWorkCasino
{
    class DiceDealer : AbstractDealer
    {
        private readonly Random _rand;

        public DiceDealer()
        {
            _rand = new Random();
        }

        private string RetString(int bet, int sum, int d1, int d2, char res)
        {
            return ("bet=" + bet + InnerDelimetr() + "sum=" + sum + InnerDelimetr() + "dices=[" + d1 + " " + d2 + "]" +
                    InnerDelimetr() + "res=" + res);
        }

        private string getInfo(int d1, int d2, int sum)
        {
            return String.Format("{0} {1}", sum, d1+d2);
        }

        public override string ToString()
        {
            return "2";
        }

        public override string Play(int bet, out int won)
        {
            String result = "";
            won = 0;
            Boolean rightInput = false;
            while (!rightInput)
            {
                Console.WriteLine("Please, input sum of dices (from 2 to 12)");
                var inputString = Console.ReadLine();
                Console.WriteLine();
                int sum;
                var res = Int32.TryParse(inputString, out sum);
                if (res)
                {
                    if ((sum > 1) && (sum < 13))
                    {
                        rightInput = true;
                        int d1 = _rand.Next(1, 6);
                        int d2 = _rand.Next(1, 6);
                        Console.WriteLine("Dices: " + d1 + " " + d2);
                        if (d1 + d2 == sum)
                        {
                            Console.WriteLine("You win!");
                            won = 2*bet;
                            result = RetString(bet, sum, d1, d2, 'W');
                        } 
                        else
                        {   
                            Console.WriteLine("You Lose!");
                            result = RetString(bet, sum, d1, d2, 'L');
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect input. Do it again");                        
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect input. Do it again");    
                }
            }
            return result;
        }

        public override GameResultRecord Play(int bet, string username)
        {
            GameResultRecord result = null;
            int balanceChange = 0;
            Boolean rightInput = false;
            while (!rightInput)
            {
                Console.WriteLine("Please, input sum of dices (from 2 to 12)");
                var inputString = Console.ReadLine();
                Console.WriteLine();
                int sum;
                var res = Int32.TryParse(inputString, out sum);
                if (res)
                {
                    if ((sum > 1) && (sum < 13))
                    {
                        rightInput = true;
                        int d1 = _rand.Next(1, 6);
                        int d2 = _rand.Next(1, 6);
                        Console.WriteLine("Dices: " + d1 + " " + d2);
                        if (d1 + d2 == sum)
                        {
                            Console.WriteLine("You win!");
                            balanceChange = bet;
                            result = new GameResultRecord(username, 2, 'W', balanceChange, getInfo(d1, d2, sum));
                        }
                        else
                        {
                            Console.WriteLine("You Lose!");
                            balanceChange = -1*bet;
                            result = new GameResultRecord(username, 2, 'L', balanceChange, getInfo(d1, d2, sum));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect input. Do it again");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect input. Do it again");
                }
            }
            return result;
        }
    }
}
