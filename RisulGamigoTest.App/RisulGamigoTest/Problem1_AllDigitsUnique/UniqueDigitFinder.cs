namespace RisulGamigoTest.Problem1_AllDigitsUnique
{
    public class UniqueDigitFinder
    {
        public bool AllDigitsUnique(uint value)
        {
            if (value == 0) return true;
            var visited = new bool[10];
            while (value > 0)
            {
                var quotient = value % 10;
                if (visited[quotient] == false)
                {
                    visited[quotient] = true;
                }
                else
                {
                    return false;
                }

                value = value / 10;
            }
            
            return true;
        }
    }
}
