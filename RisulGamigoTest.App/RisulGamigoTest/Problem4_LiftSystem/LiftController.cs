using System;
using System.Threading.Tasks;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class LiftController : IElevatorController
    {
        public event Action<int> ReachedSummonedFloor;
        public event Action<int> ReachedDestinationFloor;

        private RequestProcessor _processor;
        private IElevatorMotor _motor;

        public bool _isDoorOpen = false;

        public LiftController()
        {
            _processor = new RequestProcessor();
            _motor = new ElevatorMotor();
            _motor.CurrentDirection = Direction.Idle;
            Task.Factory.StartNew(Loop);
        }

        public async Task Loop()
        {
            while (true)
            {
                // if motor at idle, decide whether to go up or down
                if (_motor.CurrentDirection == Direction.Idle)
                {
                    GetDirectionFromRequestLookUp();
                }

                if (_motor.CurrentDirection == Direction.Up)
                {
                    var (destination, direction) = _processor.GetAnyRequestFromUpperFloor(_motor.CurrentFloor);
                    
                    // is there any up request in upper floor
                    if (destination != -1)
                    {
                        await MoveTowardsAsync(destination, direction).ConfigureAwait(false);
                    }
                    // if there is no request in upper floors, lets look down, and try to set direction Down
                    if (destination == -1)
                    {
                        var (targetFloor, _) = _processor.GetAnyRequestFromLowerFloor(_motor.CurrentFloor);
                        // if there is any request in lower floors, set the direction down
                        _motor.CurrentDirection = targetFloor != -1 ? Direction.Down : Direction.Idle;
                    }
                }

                if (_motor.CurrentDirection == Direction.Down)
                {
                    var (destination, direction) = _processor.GetAnyRequestFromLowerFloor(_motor.CurrentFloor);

                    if (destination != -1)
                    {
                        await MoveTowardsAsync(destination, direction).ConfigureAwait(false);
                    }
                    
                    // if no request in down direction, try set the direction up
                    var (targetFloor, _) = _processor.GetAnyRequestFromUpperFloor(_motor.CurrentFloor);
                    _motor.CurrentDirection = targetFloor != -1 ? Direction.Up : Direction.Idle;
                }

                if (!_isDoorOpen)
                    Console.WriteLine($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] " +
                                      $"- At Floor: {_motor.CurrentFloor}, Direction: {_motor.CurrentDirection}");

                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        private async Task MoveTowardsAsync(int destination, Direction direction)
        {
            if (_isDoorOpen) return;

            if (destination == _motor.CurrentFloor)
            {
                Console.WriteLine("Reached destination: Door Open.. Waiting For 5s");
                _isDoorOpen = true;
                await Task.Delay(5000).ConfigureAwait(false);
                _isDoorOpen = false;
                Console.WriteLine("Door Closed");
                _processor.SetRequestStatus(direction, _motor.CurrentFloor, false);
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
            var nearestRequest = _processor.GetNearestRequestInAnyDirection(_motor.CurrentFloor);
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
            if (direction == Direction.Up)
            {
                _processor.UpRequests[floor] = true;
            }
            else if (direction == Direction.Down)
            {
                _processor.DownRequests[floor] = true;
            }

            Console.WriteLine($"Request for floor {floor}-   - Direction: {direction}");
        }

        public void FloorButtonPushed(int floor)
        {
            Console.WriteLine($"Button pushed for {floor}");
            if (_motor.CurrentFloor < floor)
            {
                _processor.SetRequestStatus(Direction.Up, floor, true);
            }
            else
            {
                _processor.SetRequestStatus(Direction.Down, floor, true);
            }
        }
    }
}