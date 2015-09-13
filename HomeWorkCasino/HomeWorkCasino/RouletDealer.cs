using System;

namespace HomeWorkCasino
{
    internal class RouletDealer : AbstractDealer
    {
        private readonly Random _rand;
        private readonly int[] _blacks = {2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35};
        private readonly int[] _reds = {1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36};

        public RouletDealer()
        {
            _rand = new Random();
        }

        public override string ToString()
        {
            return "3";
        }

        private string RollABall(out int cell)
        {
            string res = "";
            cell = _rand.Next(1, 37);
            if (Array.IndexOf(_blacks, cell) == -1)
            {
                res = "R";
                Console.WriteLine("It's " + cell + " Red");
            }
            else
            {
                res = "B";
                Console.WriteLine("It's " + cell + " Black");
            }
            
            return res;
        }

        public override string Play(int bet, out int won)
        {
            String result = "";
            won = 0;
            String gameRes = "";
            Boolean rightInput = false;
            String input = "";

            Console.Write("Cells:\nBlack: ");
            foreach (var item in _blacks) Console.Write(item + " ");
            Console.Write("\nRed:   ");
            foreach (var item in _reds) Console.Write(item + " ");
            Console.WriteLine();

            while (!rightInput)
            {
                Console.WriteLine("Please, choose a color (black or red) [B/R] or a cell [a number from 1 to 36]");
                input = Console.ReadLine();
                if ((input == "R") ||(input == "B"))
                {
                    rightInput = true;
                    int realCell;
                    var color = RollABall(out realCell);
                    gameRes = realCell + color;
                    if (color == input)
                    {
                        won = 2*bet;
                        Console.WriteLine("You win!");
                        result = "W";
                    }
                    else
                    {
                        Console.WriteLine("You loose!");
                        result = "L";
                    }
                }
                else
                {
                    int cell;
                    var res = Int32.TryParse(input, out cell);
                    if ((res)&&(cell >=1)&&(cell <= 36))
                    {
                        rightInput = true;
                        int realCell;
                        var color = RollABall(out realCell);
                        gameRes = realCell + color;
                        if (cell == realCell)
                        {
                            won = 4*bet;
                            Console.WriteLine("You win!");
                            result = "W";
                        }
                        else
                        {
                            Console.WriteLine("You loose!");
                            result = "L";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect input.");
                    }
                }
            }
            result = "bet=" + bet + InnerDelimetr() + "choiсe=" + input + InnerDelimetr() +
                     "cell=" + gameRes + InnerDelimetr() + "res=" + result;
            return result;
        }

        public override GameResultRecord Play(int bet, string username)
        {
            char result = ' ';
            int balanceChange = 0;
            String gameRes = "";
            Boolean rightInput = false;
            String input = "";

            Console.Write("Cells:\nBlack: ");
            foreach (var item in _blacks) Console.Write(item + " ");
            Console.Write("\nRed:   ");
            foreach (var item in _reds) Console.Write(item + " ");
            Console.WriteLine();

            while (!rightInput)
            {
                Console.WriteLine("Please, choose a color (black or red) [B/R] or a cell [a number from 1 to 36]");
                input = Console.ReadLine();
                if ((input == "R") || (input == "B"))
                {
                    rightInput = true;
                    int realCell;
                    var color = RollABall(out realCell);
                    gameRes = realCell.ToString();
                    if (color == input)
                    {
                        balanceChange = bet;
                        Console.WriteLine("You win!");
                        result = 'W';
                    }
                    else
                    {
                        Console.WriteLine("You loose!");
                        balanceChange = -1 * bet;
                        result = 'L';
                    }
                }
                else
                {
                    int cell;
                    var res = Int32.TryParse(input, out cell);
                    if ((res) && (cell >= 1) && (cell <= 36))
                    {
                        rightInput = true;
                        int realCell;
                        RollABall(out realCell);
                        gameRes = realCell.ToString();
                        if (cell == realCell)
                        {
                            balanceChange = 3 * bet;
                            Console.WriteLine("You win!");
                            result = 'W';
                        }
                        else
                        {
                            Console.WriteLine("You loose!");
                            balanceChange = -1 * bet;
                            result = 'L';
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect input.");
                    }
                }
            }
            return new GameResultRecord(username, 3, result, balanceChange, String.Format("{0} {1}", input.ToLower(), gameRes));
        }
    }
}
