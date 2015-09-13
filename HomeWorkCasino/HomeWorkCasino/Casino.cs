using System;

namespace HomeWorkCasino
{
    internal class Casino
    {
        private myUser _player;
        private UnifiedLogger _logger;
        private DiceDealer _diceDealer;
        private BlackjackDealer _blackjackDealer;
        private RouletDealer _rouletDealer;

        public void Start(string loggerPath)
        {
            Console.WriteLine("Welcome to the Casino!");
            Console.WriteLine("Please, name yourself:");
            var name = Console.ReadLine();
            _logger = new UnifiedLogger(loggerPath);
            _player = new myUser(name, _logger.GetUserBalance(name));
            _diceDealer = new DiceDealer();
            _blackjackDealer = new BlackjackDealer();
            _rouletDealer = new RouletDealer();
            Console.WriteLine();
            Console.WriteLine("Hello, " + name);
            Console.WriteLine("Your balance is " + _player.Money);
            ChooseAction();
        }


        private void ChooseAction()
        {
            Boolean rightInput;
            Boolean isPlaying = true;
            while (isPlaying)
            {
                Console.WriteLine();
                Console.WriteLine("Please, choose what do you want:");
                Console.WriteLine(" 0: Add more money \n 1: Play Blackjack \n 2: Play Dice \n 3: Play Roulet \n 4: Exit");
                rightInput = false;
                while (!rightInput)
                {
                    var inputString = Console.ReadLine();
                    Console.WriteLine();
                    int input;
                    var res = Int32.TryParse(inputString, out input);
                    if (res)
                    {
                        switch (input)
                        {
                            case 1:
                                if (_player.Money == 0)
                                {
                                    Console.WriteLine("Your balance is 0. Please, add money for play a game.");
                                }
                                else
                                {
                                    rightInput = true;
                                    PlayGame(_blackjackDealer);
                                }
                                break;
                            case 2:
                                if (_player.Money == 0)
                                {
                                    Console.WriteLine("Your balance is 0. Please, add money for play a game.");
                                }
                                else
                                {
                                    rightInput = true;
                                    PlayGame(_diceDealer);
                                }
                                break;
                            case 3:
                                if (_player.Money == 0)
                                {
                                    Console.WriteLine("Your balance is 0. Please, add money for play a game.");
                                }
                                else
                                {
                                    rightInput = true;
                                    PlayGame(_rouletDealer);
                                }
                                break;
                            case 4:
                                Console.WriteLine("Goodbye!");
                                rightInput = true;
                                isPlaying = false;
                                break;
                            case 0:
                                rightInput = true;
                                MoneyInput();
                                Console.WriteLine("Your balance is " + _player.Money);
                          //      ChooseAction();
                                break;
                            default:
                                Console.WriteLine("Please, write a correct number");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please, write a correct number");
                    }
                }
            }
        }

        private void MoneyInput()
        {
            Boolean rightInput = false;
            while (!rightInput)
            {
                Console.WriteLine("Please, input positive amount of money");
                var inputString = Console.ReadLine();
                Console.WriteLine();
                int input;
                var res = Int32.TryParse(inputString, out input);
                if (res)
                {
                    if (input >= 0)
                    {
                        _player.AddMoney(input);
                        _logger.AddNewDeposite(_player.Username, input);
                        rightInput = true;
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
        }

        private void PlayGame(AbstractDealer dealer)
        {
            Boolean playAgain = true;
            Boolean rightBetInput;
            Boolean rightAgainInput;
            int bet;
            GameResultRecord gameResult;

            while (playAgain)
            {
                rightBetInput = false;
                while (!rightBetInput)
                {
                    Console.WriteLine("Please, make a positive bet");
                    var inputString = Console.ReadLine();
                    Console.WriteLine();
                    var res = Int32.TryParse(inputString, out bet);
                    if (res)
                    {
                        if (bet >= 0) // zero bet for exit opportunity
                        {
                            if (bet <= _player.Money)
                            {
                                rightBetInput = true;
                                rightAgainInput = false;
                                _logger.AddNewBet(_player.Username, Int32.Parse(dealer.ToString()), bet);
                                gameResult = dealer.Play(bet,  _player.Username);
                                _player.AddMoney(gameResult.BalanceChange);
                                _logger.AddNewResult(gameResult);
                                Console.WriteLine("Your balance is " + _player.Money);
                                while (!rightAgainInput)
                                {
                                    Console.WriteLine("Do you want to play again? [Y/N]:");
                                    string answer = Console.ReadLine();
                                    Console.WriteLine();
                                    switch (answer)
                                    {
                                        case "Y":
                                            rightAgainInput = true;
                                            break;
                                        case "N":
                                            rightAgainInput = true;
                                            playAgain = false;
                                        //    ChooseAction();
                                            break;
                                        default:
                                            Console.WriteLine("Incorrect input. Do it again");
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Not enough money to make this bet. Please, do it again");
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
            }
        }
    }
}

