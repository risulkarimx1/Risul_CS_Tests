using System;
using System.Threading;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class ElevatorManager: IDisposable
    {
        private IElevatorController _elevatorController;
        private CancellationTokenSource _cancellationTokenSource;
        public ElevatorManager()
        {
            // var elevatorMotor = new ElevatorMotor();
            // var dispatcher = new RequestDispatcher(10);
            // _cancellationTokenSource = new CancellationTokenSource();
            // _elevatorController = new ElevatorController(elevatorMotor, dispatcher, _cancellationTokenSource.Token);
            _elevatorController = new LiftController();
        }
        
        public void SimulateOperation(int floor , Direction direction)
        {
            _elevatorController.SummonButtonPushed(floor, direction);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
    
}
