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

        public LiftController()
        {
            _processor = new RequestProcessor();
            _motor = new ElevatorMotor();
            _motor.CurrentFloor = 5;
            _motor.CurrentDirection = Direction.Idle;
            Task.Factory.StartNew(() => Loop());
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
                    var destination = _processor.GetUpperFloorUpRequests(_motor.CurrentFloor);
                    // is there any up request in upper floor
                    if (destination != -1)
                    {
                        MoveTowards(destination, Direction.Up);
                    }
                    // if no up request in upper floor, is there any down request in upper floor
                    else
                    {
                        destination = _processor.GetUpperFloorDownRequest(_motor.CurrentFloor);
                        if (destination != -1)
                        {
                            MoveTowards(destination, Direction.Down);
                        }
                    }

                    // if there is no request in upper floors, lets look down, and try to set direction Down
                    if (destination == -1)
                    {
                        destination = _processor.GetLowerFloorDownRequests(_motor.CurrentFloor);
                        if (destination == -1)
                        {
                            destination = _processor.GetLowerFloorUpRequests(_motor.CurrentFloor);
                        }

                        // if there is any request in lower floors, set the direction down
                        if (destination != -1)
                        {
                            _motor.CurrentDirection = Direction.Down;
                        }
                        else // or just become idle
                        {
                            _motor.CurrentDirection = Direction.Idle;
                        }
                        
                    }
                }

                if (_motor.CurrentDirection == Direction.Down)
                {
                    var destination = _processor.GetLowerFloorDownRequests(_motor.CurrentFloor);
                    if (destination != -1)
                    {
                        MoveTowards(destination, Direction.Down);
                    }
                    else
                    {
                        destination = _processor.GetLowerFloorUpRequests(_motor.CurrentFloor);
                        if (destination != -1)
                        {
                            MoveTowards(destination, Direction.Up);
                        }
                    }
                    
                    // if no request in down direction, try set the direction up
                    if (destination == -1)
                    {
                        destination = _processor.GetUpperFloorUpRequests(_motor.CurrentFloor);
                        if (destination == -1)
                        {
                            destination = _processor.GetUpperFloorDownRequest(_motor.CurrentFloor);
                        }

                        if (destination != -1)
                        {
                            // has some request in up direction.. set direction up
                            _motor.CurrentDirection = Direction.Up;
                        }
                        else
                        {
                            _motor.CurrentDirection = Direction.Idle;
                        }
                    }
                }

                Console.WriteLine(
                    $"[{DateTime.Now.Minute}:{DateTime.Now.Second}] - At Floor: {_motor.CurrentFloor}, Direction: {_motor.CurrentDirection}");
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        public void MoveTowards(int destination, Direction direction)
        {
            if (destination == _motor.CurrentFloor)
            {
                Console.WriteLine("Reached destination");
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
            }else if (_motor.CurrentFloor < nearestRequest)
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
            
        }
    }
}