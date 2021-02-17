using System;
using System.Text;
using System.Threading.Tasks;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class ElevatorController : IElevatorController
    {
        public event Action<int> ReachedSummonedFloor = delegate{};
        public event Action<int> ReachedDestinationFloor = delegate{};

        private RequestDispatcher _dispatcher;
        private IElevatorMotor _motor;

        private bool _isDoorOpen;
        private StringBuilder _elevatorLog;

        public object _syncRoot;

        public ElevatorController(IElevatorMotor motor, int floorHeight)
        {
            _syncRoot = new object();
            _motor = motor;
            _elevatorLog = new StringBuilder();
            _dispatcher = new RequestDispatcher(floorHeight);
            _motor.CurrentDirection = Direction.Idle;
            Task.Factory.StartNew(Loop);
        }

        private async Task Loop()
        {
            while (true)
            {
                // if motor at idle, make a check if it can go up or down
                if (_motor.CurrentDirection == Direction.Idle)
                {
                    GetDirectionFromRequestLookUp();
                }

                if (_motor.CurrentDirection == Direction.Up)
                {
                    var (destination, direction) = _dispatcher.GetAnyRequestFromUpperFloor(_motor.CurrentFloor);

                    // is there any up request in upper floor
                    if (destination != -1)
                    {
                        await MoveTowardsAsync(destination, direction).ConfigureAwait(false);
                    }
                    // if there is no request in upper floors, lets look down, and try to set direction Down
                    else
                    {
                        var (targetFloor, _) = _dispatcher.GetAnyRequestFromLowerFloor(_motor.CurrentFloor);
                        // if there is any request in lower floors, set the direction down
                        _motor.CurrentDirection = targetFloor != -1 ? Direction.Down : Direction.Idle;
                    }
                }

                if (_motor.CurrentDirection == Direction.Down)
                {
                    var (destination, direction) = _dispatcher.GetAnyRequestFromLowerFloor(_motor.CurrentFloor);

                    if (destination != -1)
                    {
                        await MoveTowardsAsync(destination, direction).ConfigureAwait(false);
                    }
                    else
                    {
                        // if no request in down direction, try set the direction up
                        var (targetFloor, _) = _dispatcher.GetAnyRequestFromUpperFloor(_motor.CurrentFloor);
                        _motor.CurrentDirection = targetFloor != -1 ? Direction.Up : Direction.Idle;
                    }
                }

                if (!_isDoorOpen)
                { 
                    _elevatorLog.Clear();
                    _elevatorLog.Append($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] ");
                    _elevatorLog.Append($"- At Floor: {_motor.CurrentFloor}, Direction: {_motor.CurrentDirection}");
                    if (_motor.CurrentDirection != Direction.Idle)
                    {
                        if (_motor.CurrentDirection == Direction.Up)
                        {
                            _elevatorLog.Append($" | Will Stop At: {_dispatcher.DisplayUpRequestLog()}");
                        }
                        else
                        {
                            _elevatorLog.Append($" | Will Stop At: {_dispatcher.DisplayDownRequestLog()}");
                        }
                    }
                    
                    Console.WriteLine(_elevatorLog);
                }


                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        private async Task MoveTowardsAsync(int destination, Direction direction)
        {
            if (_isDoorOpen) return;

            if (destination == _motor.CurrentFloor)
            {
                Console.WriteLine($"<<<<<< Reached destination: {_motor.CurrentFloor} | Door Open.. Waiting For 5s");
                ReachedDestinationFloor.Invoke(_motor.CurrentFloor);
                if (_dispatcher.GetSummonRequestStatus(_motor.CurrentFloor))
                {
                    Console.WriteLine($"<<<<<< Reached Summoned Floor: {_motor.CurrentFloor} | Door Open.. Waiting For 5s");
                    ReachedSummonedFloor.Invoke(_motor.CurrentFloor);
                    _dispatcher.SetSummonRequestStatus(_motor.CurrentFloor, false);
                }
                lock (_syncRoot)
                {
                    _isDoorOpen = true;
                }

                await Task.Delay(5000).ConfigureAwait(false);
                lock (_syncRoot)
                {
                    _isDoorOpen = false;
                }

                Console.WriteLine("<<<<<<  Door Closed");
                _dispatcher.SetRequestStatus(direction, _motor.CurrentFloor, false);
            }
            else if (destination < _motor.CurrentFloor)
            {
                _motor.GoToFloor(_motor.CurrentFloor - 1);
            }
            else
            {
                _motor.GoToFloor(_motor.CurrentFloor + 1);
            }
        }

        private void GetDirectionFromRequestLookUp()
        {
            var nearestRequest = _dispatcher.GetNearestRequestInAnyDirection(_motor.CurrentFloor);
            if (nearestRequest == -1)
            {
                _motor.CurrentDirection = Direction.Idle;
            }
            else if (_motor.CurrentFloor < nearestRequest)
            {
                _motor.CurrentDirection = Direction.Up;
            }
            else
            {
                _motor.CurrentDirection = Direction.Down;
            }
        }

        public void SummonButtonPushed(int floor, Direction direction)
        {
            lock (_syncRoot)
            {
                _dispatcher.SetRequestStatus(direction, floor, true);
                _dispatcher.SetSummonRequestStatus(floor, true);
            }

            Console.WriteLine($"    >>>> SummonButtonPushed floor {floor}- Direction: {direction}");
        }

        public void FloorButtonPushed(int floor)
        {
            Console.WriteLine($"   >>>>> FloorButtonPushed {floor}");
            if (_motor.CurrentFloor < floor)
            {
                lock (_syncRoot)
                {
                    _dispatcher.SetRequestStatus(Direction.Up, floor, true);
                }
            }
            else
            {
                lock (_syncRoot)
                {
                    _dispatcher.SetRequestStatus(Direction.Down, floor, true);
                }
            }
        }
    }
}