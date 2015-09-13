using System;

namespace HomeWorkCasino
{
    class BlackjackDealer : AbstractDealer
    {
        private readonly Random _rand;
        private readonly char[] _suits = {'♠', '♣', '♥', '♦'};
        private readonly string[] _values = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};

        public BlackjackDealer()
        {
            _rand = new Random();
        }


        public override string ToString()
        {
            return "1";
        }

        private string GetCard(ref int sum)
        {
            var suit = _rand.Next(0, 4);
            var value = _rand.Next(0, 13);
            string card = _values[value] + _suits[suit];
            value++;
            if (value == 1)
            {
                if (sum <= 10)
                {
                    sum += 11;
                }
                else
                {
                    sum += 1;
                }
            }
            else
            {
                if (value > 10)
                {
                    sum += 10;
                }
                else
                {
                    sum += value;
                }
            }
            return card;
        }


        public override string Play(int bet, out int won)
        {
            String result = "";
            won = 0;
            Boolean moreCards = true;
            String playerCards;
            String dealerCards;
            Boolean busting = false;
            int playerSum = 0;
            int dealerSum = 0;

            playerCards = GetCard(ref playerSum);
            
            while ((moreCards)&&(playerSum < 21))
            {
                Boolean rightInput = false;
                playerCards += " " + GetCard(ref playerSum);
                Console.WriteLine("Your cards: " + playerCards);
                while ((!rightInput)&&(playerSum < 21))
                {
                    Console.WriteLine("Need More? [Y/N]");
                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "Y":
                            rightInput = true;
                            break;
                        case "N":
                            rightInput = true;
                            moreCards = false;
                            break;
                        default:
                            Console.WriteLine("Incorrect input.");
                            break;
                    }
                    Console.WriteLine();
                }
            }

            if (playerSum == 21)
            {
                Console.WriteLine("Blackjack!");
            }
            else
            {
                if (playerSum > 21)
                {
                    busting = true;
                    Console.WriteLine("Busting!");
                }
            }

            if (busting)
            {   
                Console.WriteLine("You loose!");
                dealerCards = "-";
                result = "L";
            }
            else
            {
                dealerCards = GetCard(ref dealerSum);
                while (dealerSum < 17)
                {
                    dealerCards += " " + GetCard(ref dealerSum);
                }
                Console.WriteLine("Dealer cards: " + dealerCards);
                if (dealerSum > 21)
                {
                    Console.WriteLine("You win!");
                    won = 2*bet;
                    result = "W";
                }
                else
                {
                    if (playerSum > dealerSum)
                    {
                        Console.WriteLine("You win!");
                        won = 2*bet;
                        result = "W";
                    }
                    else
                    {
                        Console.WriteLine("You loose!");
                        result = "L";
                    }
                }
            }
            result = "bet=" + bet + InnerDelimetr() +
                     "sum=" + playerSum + "/" + dealerSum + InnerDelimetr() +
                     "cards=" + "[" + playerCards + "/" + dealerCards + "]" + InnerDelimetr() +
                     "res=" + result;
            return result;
        }

        public override GameResultRecord Play(int bet, string username)
        {
            char result = ' ';
            int balanceChange = 0;
            Boolean moreCards = true;
            String playerCards;
            String dealerCards;
            Boolean busting = false;
            int playerSum = 0;
            int dealerSum = 0;

            playerCards = GetCard(ref playerSum);

            while ((moreCards) && (playerSum < 21))
            {
                Boolean rightInput = false;
                playerCards += " " + GetCard(ref playerSum);
                Console.WriteLine("Your cards: " + playerCards);
                while ((!rightInput) && (playerSum < 21))
                {
                    Console.WriteLine("Need More? [Y/N]");
                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "Y":
                            rightInput = true;
                            break;
                        case "N":
                            rightInput = true;
                            moreCards = false;
                            break;
                        default:
                            Console.WriteLine("Incorrect input.");
                            break;
                    }
                    Console.WriteLine();
                }
            }

            if (playerSum == 21)
            {
                Console.WriteLine("Blackjack!");
            }
            else
            {
                if (playerSum > 21)
                {
                    busting = true;
                    Console.WriteLine("Busting!");
                }
            }

            if (busting)
            {
                Console.WriteLine("You loose!");
                balanceChange = -1 * bet;
                result = 'L';
            }
            else
            {
                dealerCards = GetCard(ref dealerSum);
                while (dealerSum < 17)
                {
                    dealerCards += " " + GetCard(ref dealerSum);
                }
                Console.WriteLine("Dealer cards: " + dealerCards);
                if (dealerSum > 21)
                {
                    Console.WriteLine("You win!");
                    balanceChange = bet;
                    result = 'W';
                }
                else
                {
                    if (playerSum > dealerSum)
                    {
                        Console.WriteLine("You win!");
                        balanceChange = bet;
                        result = 'W';
                    }
                    else
                    {
                        Console.WriteLine("You loose!");
                        balanceChange = -1 * bet;
                        result = 'L';
                    }
                }
            }
            return new GameResultRecord(username, 1, result, balanceChange, String.Format("{0} {1}", playerSum, dealerSum));
        }
    }
}
