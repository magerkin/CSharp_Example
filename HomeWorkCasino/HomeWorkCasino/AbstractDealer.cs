namespace HomeWorkCasino
{
    abstract class AbstractDealer
    {
        public string InnerDelimetr()
        {
            return ",";
        }

        public abstract string Play(int bet, out int won);

        public abstract GameResultRecord Play(int bet, string username);
    }

}
