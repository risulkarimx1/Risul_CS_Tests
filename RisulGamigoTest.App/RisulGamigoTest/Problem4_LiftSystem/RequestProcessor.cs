using System;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class RequestProcessor
    {
        public int floorHeight = 10;
        public bool[] UpRequests;
        public bool[] DownRequests;

        public RequestProcessor()
        {
            UpRequests = new bool[floorHeight];
            DownRequests = new bool[floorHeight];
        }

        public void SetRequestStatus(Direction direction, int floor, bool state)
        {
            if (direction == Direction.Up) UpRequests[floor] = state;
            if (direction == Direction.Down) DownRequests[floor] = state;
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
            for (int i = currentFloor; i < UpRequests.Length; i++)
            {
                if (UpRequests[i]) return i;
            }

            return -1;
        }

        private int GetUpperFloorDownRequest(int currentFloor)
        {
            for (int i = currentFloor; i < floorHeight; i++)
            {
                if (UpRequests[i]) return i;
            }

            return -1;
        }

        // Request from lower currentFloor

        private int GetLowerFloorDownRequests(int currentFloor)
        {
            for (int i = currentFloor; i >= 0; i--)
            {
                if (DownRequests[i]) return i;
            }

            return -1;
        }

        private int GetLowerFloorUpRequests(int currentFloor)
        {
            for (var i = currentFloor; i >= 0; i--)
            {
                if (UpRequests[i]) return i;
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
    }
}