using System;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public interface IElevatorController
    {
        void SummonButtonPushed(int floor, Direction direction);
        void FloorButtonPushed(int floor);
        event Action<int> ReachedSummonedFloor;
        event Action<int> ReachedDestinationFloor;
    }
}