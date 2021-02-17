using System;
using RisulGamigoTest.Problem4_LiftSystem;

namespace RisulGamigoTest
{
    class Program
    {
        public static Random random = new Random();
        static void Main(string[] args)
        {
            
            var liftController = new LiftController();
            

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.A)
                {
                    liftController.SummonButtonPushed(3, Direction.Up);
                }

                if (Console.ReadKey().Key == ConsoleKey.B)
                {
                    liftController.SummonButtonPushed(9, Direction.Up);
                }

                if (Console.ReadKey().Key == ConsoleKey.R)
                {
                    var direction = Direction.Up;
                    if (random.Next(0, 100) > 50) direction = Direction.Down;
                    liftController.SummonButtonPushed(random.Next(0,9), direction);
                }

                if (Console.ReadKey().Key == ConsoleKey.C)
                {
                    liftController.FloorButtonPushed(random.Next(0, 9));
                }
            }
        }
    }
}
