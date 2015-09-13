namespace HomeWorkCasino
{
    class User
    {
        private string _username;

        public User (string name, int money = 0)
        {
            _username = name;
            Money = money;
            
        }

        internal int Money { get; private set; }

        internal void AddMoney(int amount)
        {
            Money += amount;
        }

    }
}
