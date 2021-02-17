using System;
using RisulGamigoTest.Problem4_LiftSystem;

namespace RisulGamigoTest
{
    class Program
    {
        public static Random _random = new Random();
        public const int _floorCount = 11;
        static void Main(string[] args)
        {
            IElevatorMotor motor = new ElevatorMotor();
            IElevatorController controller = new ElevatorController(motor, _floorCount);
            TakeInput(controller);
        }

        private static void TakeInput(IElevatorController liftController)
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.S)
                {
                    var direction = Direction.Up;
                    if (_random.Next(0, 100) > 50) direction = Direction.Down;
                    liftController.SummonButtonPushed(_random.Next(0, _floorCount), direction);
                }

                if (Console.ReadKey().Key == ConsoleKey.F)
                {
                    liftController.FloorButtonPushed(_random.Next(0, _floorCount));
                }
            }
        }
    }
}