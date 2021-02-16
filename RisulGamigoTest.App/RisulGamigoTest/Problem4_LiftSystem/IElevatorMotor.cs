using System;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public interface IElevatorMotor
    {
        Direction CurrentDirection { get; set; }
        int CurrentFloor { get; set; }
        event Action<int> ReachedFloor;
        void GoToFloor(int floor);
    }

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