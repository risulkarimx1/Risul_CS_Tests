using System;
using RisulGamigoTest.Problem4_LiftSystem;

namespace RisulGamigoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var elevatorManager = new ElevatorManager();
            
            
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.A)
                {
                    elevatorManager.SimulateOperation(3, Direction.Up);
                }

                if (Console.ReadKey().Key == ConsoleKey.B)
                {
                    elevatorManager.SimulateOperation(9, Direction.Up);
                }

                else if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    elevatorManager.Dispose();
                }
            }
        }
    }
}
