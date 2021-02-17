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
}