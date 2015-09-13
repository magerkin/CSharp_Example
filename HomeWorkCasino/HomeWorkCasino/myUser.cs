namespace HomeWorkCasino
{
    class myUser
    {
        private string _username;

        public myUser (string name, int money = 0)
        {
            _username = name;
            Money = money;
            
        }

        internal int Money { get; private set; }
        
        internal string Username {
            get { return _username; }
        }

        internal void AddMoney(int amount)
        {
            Money += amount;
        }

    }
}
