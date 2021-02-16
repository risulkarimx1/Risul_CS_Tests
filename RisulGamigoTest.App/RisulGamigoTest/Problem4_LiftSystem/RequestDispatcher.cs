using System;
using System.Linq;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class RequestDispatcher
    {
        private readonly int _floorsCount;
        private bool[] _upRequests;
        private bool[] _downRequests;

        public RequestDispatcher(int floorsCount)
        {
            _floorsCount = floorsCount;
            _upRequests = new bool[floorsCount];
            _downRequests = new bool[floorsCount];
        }

        public void SetRequestStatus(int floor, Direction diretion, bool status)
        {
            if (floor >= _floorsCount)
            {
                throw new InvalidOperationException("Requested floor can't be more than total floor");
            }

            if (diretion == Direction.Up)
            {
                _upRequests[floor] = status;
            }
            else
            {
                _downRequests[floor] = status;
            }
        }

        public int GetUpRequestCount => _upRequests.Count(c => c);
        public int GetDownRequestCount => _downRequests.Count(c => c);
        public bool HasUpRequest => GetUpRequestCount > 0;
        public bool HasDownRequest => GetDownRequestCount > 0;

        public (Direction, int) GetDirectionAndDestination(int currentFloor)
        {
            var upRequest = GetDirectionAndDestination(_upRequests, currentFloor);
            var downRequest = GetDirectionAndDestination(_downRequests, currentFloor);
            
            if (upRequest.Item1 != Direction.Idle && downRequest.Item1 != Direction.Idle)
            {
                if (upRequest.Item2 < downRequest.Item2)
                {
                    return upRequest;
                }
                else
                {
                    return downRequest;
                }
            }else if (upRequest.Item1 != Direction.Idle)
            {
                return upRequest;
            }else if (downRequest.Item1 != Direction.Idle)
            {
                return downRequest;
            }

            return (Direction.Idle, currentFloor);
        }


        private (Direction, int) GetDirectionAndDestination(bool[] requests, int currentFloor)
        {
            var upPointer = currentFloor;
            var downPointer = currentFloor;

            var requestedInUpperFloor = false;
            var requestedInLowerFloor = false;
            
            while (upPointer < requests.Length)
            {
                if (requests[upPointer])
                {
                    requestedInUpperFloor = true;
                    break;
                }
                upPointer++;
            }

            while (downPointer > 0)
            {
                if (requests[downPointer])
                {
                    requestedInLowerFloor = true;
                    break;
                }
                downPointer--;
            }

            if (requestedInUpperFloor && requestedInLowerFloor)
            {
                if (Math.Abs(currentFloor - upPointer) < Math.Abs(currentFloor - downPointer))
                {
                    return (Direction.Up, upPointer);
                }
                else
                {
                    return (Direction.Down, downPointer);
                }
            }else if (requestedInUpperFloor)
            {
                return (Direction.Up, upPointer);
            }else if (requestedInLowerFloor)
            {
                return (Direction.Down, downPointer);
            }

            return (Direction.Idle, currentFloor);

        }
    }
}