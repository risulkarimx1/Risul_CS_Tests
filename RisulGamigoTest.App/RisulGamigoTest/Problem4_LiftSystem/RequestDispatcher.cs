using System;
using System.Text;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class RequestDispatcher
    {
        private readonly int _floorHeight;
        private readonly bool[] _upRequests;
        private readonly bool[] _downRequests;
        private readonly bool[] _summonRequests;
        private StringBuilder _requestLog;

        public RequestDispatcher(int floorHeight)
        {
            _requestLog = new StringBuilder();
            _floorHeight = floorHeight;
            _upRequests = new bool[floorHeight];
            _downRequests = new bool[floorHeight];
            _summonRequests = new bool[floorHeight];
        }


        public StringBuilder DisplayUpRequestLog()
        {
            return DisplayRequestLog(_upRequests);
        }

        public StringBuilder DisplayDownRequestLog()
        {
            return DisplayRequestLog(_downRequests);
        }

        private StringBuilder DisplayRequestLog(bool[] request)
        {
            _requestLog.Clear();
            _requestLog.Append("[");
            for (int i = 0; i < _floorHeight; i++)
            {
                if (request[i])
                {
                    _requestLog.Append(i);
                    _requestLog.Append(",");
                }
            }

            if (_requestLog.Length > 1)
            {
                _requestLog.Remove(_requestLog.Length - 1, 1);
            }

            _requestLog.Append("]");
            return _requestLog;
        }

        public void SetRequestStatus(Direction direction, int floor, bool state)
        {
            if (direction == Direction.Up) _upRequests[floor] = state;
            if (direction == Direction.Down) _downRequests[floor] = state;
        }


        public (int, Direction) GetAnyRequestFromUpperFloor(int currentFloor)
        {
            var upRequest = GetUpperFloorUpRequests(currentFloor);
            if (upRequest != -1) return (upRequest, Direction.Up);
            var downRequest = GetUpperFloorDownRequest(currentFloor);
            if (downRequest != -1) return (downRequest, Direction.Down);

            return (-1, Direction.Idle);
        }

        public (int, Direction) GetAnyRequestFromLowerFloor(int currentFloor)
        {
            var downRequest = GetLowerFloorDownRequests(currentFloor);
            if (downRequest != -1) return (downRequest, Direction.Down);

            var upRequest = GetLowerFloorUpRequests(currentFloor);
            if (upRequest != -1) return (upRequest, Direction.Up);

            return (-1, Direction.Idle);
        }

        // Requests from upper floors

        private int GetUpperFloorUpRequests(int currentFloor)
        {
            for (int i = currentFloor; i < _upRequests.Length; i++)
            {
                if (_upRequests[i]) return i;
            }

            return -1;
        }

        private int GetUpperFloorDownRequest(int currentFloor)
        {
            for (int i = currentFloor; i < _floorHeight; i++)
            {
                if (_downRequests[i]) return i;
            }

            return -1;
        }

        // Request from lower currentFloor

        private int GetLowerFloorDownRequests(int currentFloor)
        {
            for (int i = currentFloor; i >= 0; i--)
            {
                if (_downRequests[i]) return i;
            }

            return -1;
        }

        private int GetLowerFloorUpRequests(int currentFloor)
        {
            for (var i = currentFloor; i >= 0; i--)
            {
                if (_upRequests[i]) return i;
            }

            return -1;
        }


        public int GetNearestRequestInAnyDirection(int currentFloor)
        {
            var upperFloorUpRequest = GetUpperFloorUpRequests(currentFloor);
            var upperFloorDownRequest = GetUpperFloorDownRequest(currentFloor);

            var lowerFloorUpRequest = GetLowerFloorUpRequests(currentFloor);
            var lowerFloorDownRequest = GetLowerFloorDownRequests(currentFloor);

            var destination = int.MaxValue;
            if (upperFloorUpRequest != -1)
            {
                destination = upperFloorUpRequest;
            }

            if (upperFloorDownRequest != -1)
            {
                destination = Math.Min(destination, upperFloorDownRequest);
            }

            if (lowerFloorUpRequest != -1)
            {
                destination = Math.Min(destination, lowerFloorUpRequest);
            }

            if (lowerFloorDownRequest != -1)
            {
                destination = Math.Min(destination, lowerFloorDownRequest);
            }

            if (destination == int.MaxValue) return -1;
            return destination;
        }

        public void SetSummonRequestStatus(int floor, bool status)
        {
            _summonRequests[floor] = status;
        }

        public bool GetSummonRequestStatus(in int floor)
        {
            return _summonRequests[floor];
        }
    }
}