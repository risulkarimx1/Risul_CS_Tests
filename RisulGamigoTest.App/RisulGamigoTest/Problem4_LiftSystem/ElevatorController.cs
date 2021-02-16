using System;
using System.Threading;
using System.Threading.Tasks;
namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class ElevatorController : IElevatorController, IDisposable
    {
        public event Action<int> ReachedSummonedFloor;
        public event Action<int> ReachedDestinationFloor;
        private IElevatorMotor _elevatorMotor;
        private readonly RequestDispatcher _requestDispatcher;
        private CancellationToken _cancellationToken;
        
        public ElevatorController(IElevatorMotor elevatorMotor, RequestDispatcher requestDispatcher, CancellationToken cancellationToken)
        {
            _elevatorMotor = elevatorMotor;
            _requestDispatcher = requestDispatcher;
            _cancellationToken = cancellationToken;
            _elevatorMotor.ReachedFloor += OnReachedFloor;
            Task.Factory.StartNew(Loop, cancellationToken);
        }

        private int _destinationFloor = 0;

        private void OnReachedFloor(int floor)
        {
            if (floor == _destinationFloor)
            {
                Console.WriteLine($"Reached floor: {_destinationFloor} - Opening Door");
            }
        }

        public async Task Loop()
        {
            while (_cancellationToken.IsCancellationRequested == false)
            {
                if (_requestDispatcher.HasUpRequest == false && _requestDispatcher.HasDownRequest == false)
                {
                    _elevatorMotor.CurrentDirection = Direction.Idle;
                }
                
                if (_elevatorMotor.CurrentDirection == Direction.Idle)
                {
                    var directionAndDestination = _requestDispatcher.GetDirectionAndDestination(_elevatorMotor.CurrentFloor);
                    _elevatorMotor.CurrentDirection = directionAndDestination.Item1;
                    _destinationFloor = directionAndDestination.Item2;
                }

                if (_elevatorMotor.CurrentDirection == Direction.Up || _elevatorMotor.CurrentDirection == Direction.Down )
                {
                    if (_elevatorMotor.CurrentFloor == _destinationFloor)
                    {
                        _requestDispatcher.SetRequestStatus(_destinationFloor, _elevatorMotor.CurrentDirection, false);
                        _elevatorMotor.CurrentDirection = Direction.Idle;
                    }
                    else
                    {
                        GoToFloor();
                    }
                }

                Console.WriteLine($"At Floor {_elevatorMotor.CurrentFloor} - Going {_elevatorMotor.CurrentDirection} - Time: {DateTime.Now.Minute}.{DateTime.Now.Second}");
                await Task.Delay(1000, _cancellationToken).ConfigureAwait(false);
            }
            
        }

        private void GoToFloor()
        {
            if (_elevatorMotor.CurrentFloor != _destinationFloor)
            {
                if (_elevatorMotor.CurrentFloor > _destinationFloor)
                {
                    _elevatorMotor.GoToFloor(_elevatorMotor.CurrentFloor - 1);
                }
                else if (_elevatorMotor.CurrentFloor < _destinationFloor)
                {
                    _elevatorMotor.GoToFloor(_elevatorMotor.CurrentFloor + 1);
                }
            }
        }

        public void SummonButtonPushed(int floor, Direction direction)
        {
            Console.WriteLine($"Received Request for floor {floor}, Direction: {direction}-------Current Floor {_elevatorMotor.CurrentFloor}--------");
            if (_elevatorMotor.CurrentFloor == floor)
            {
                Console.WriteLine("At the same floor... Opening Door");
            }
            else
            {
                _requestDispatcher.SetRequestStatus(floor, direction, true);
            }
            
        }

        public void FloorButtonPushed(int floor)
        {
        }

        public void Dispose()
        {
            _elevatorMotor.ReachedFloor -= OnReachedFloor;
        }
    }
}