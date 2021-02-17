using System;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class ElevatorMotor : IElevatorMotor
    {
        public Direction CurrentDirection { get; set; }
        public int CurrentFloor { get; set; }
        public event Action<int> ReachedFloor = delegate {};

        public ElevatorMotor()
        {
            CurrentDirection = Direction.Idle;
            CurrentFloor = 0;
        }

        public void GoToFloor(int floor)
        { 
            CurrentFloor = floor;
            ReachedFloor.Invoke(floor);
        }
    }
}