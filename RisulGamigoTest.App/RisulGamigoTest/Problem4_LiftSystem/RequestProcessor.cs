using System;
using System.Linq;

namespace RisulGamigoTest.Problem4_LiftSystem
{
    public class RequestProcessor
    {
        public int floorHeight = 10;
        public bool[] UpRequests ;
        public bool[] DownRequests;

        public RequestProcessor()
        {
            UpRequests = new bool[floorHeight];
            DownRequests = new bool[floorHeight];
        }
        
        public bool HasUpRequest => UpRequests.Count(a => a) > 0;
        public bool HasDownRequest => DownRequests.Count(a => a) > 0;

        public int GetNearestUpRequest(int currentFloor)
        {
            return GetNearestRequest(UpRequests, currentFloor);
        }

        public int GetNearestDownRequest(int currentFloor)
        {
            return GetNearestRequest(DownRequests, currentFloor);
        }

        public int GetNearestRequest(bool[] request, int currentFloor)
        {
            var hasRequestInUpperFloor = false;
            var hasRequestInLowerFloor = false;

            var upperFloorPointer = currentFloor + 1;
            var lowerFloorPointer = currentFloor - 1;
            while (upperFloorPointer < request.Length)
            {
                if (request[upperFloorPointer])
                {
                    hasRequestInUpperFloor = true;
                    break;
                }

                upperFloorPointer++;
            }

            while (lowerFloorPointer > 0)
            {
                if (request[lowerFloorPointer])
                {
                    hasRequestInLowerFloor = true;
                    break;
                }

                lowerFloorPointer--;
            }

            if (hasRequestInUpperFloor && hasRequestInLowerFloor)
            {
                var distanceFromCurrentToUpper = Math.Abs(currentFloor - upperFloorPointer);
                var distanceFromCurrentToLower = Math.Abs(currentFloor - lowerFloorPointer);

                if (distanceFromCurrentToLower < distanceFromCurrentToUpper)
                {
                    return lowerFloorPointer;
                }
                else
                {
                    return upperFloorPointer;
                }
            }
            else if (hasRequestInLowerFloor)
            {
                return lowerFloorPointer;
            }
            else if (hasRequestInUpperFloor)
            {
                return upperFloorPointer;
            }

            return currentFloor;
        }

        public void SetRequestStatus(Direction direction,int floor, bool state)
        {
            if (direction == Direction.Up) UpRequests[floor] = state;
            if (direction == Direction.Down) DownRequests[floor] = state;
        }

        public bool HasAnyRequestsInDirection(int motorCurrentFloor, Direction direction)
        {
            //if lift is going up, look for true index starting from current floor towards the top floor
            if (direction == Direction.Up)
            {
                while (motorCurrentFloor < UpRequests.Length)
                {
                    if (UpRequests[motorCurrentFloor]) return true;
                    motorCurrentFloor++;
                }
            }else if (direction == Direction.Down)
            {
                // if going down, look for down request from this floor towards 0
                while (motorCurrentFloor > 0)
                {
                    if (DownRequests[motorCurrentFloor]) return true;
                    motorCurrentFloor--;
                }
            }

            // no request to be processed, become idle
            return false;
        }

        // Requests from upper floors
        
        public int GetUpperFloorUpRequests(int currnetFloor)
        {
            for (int i = currnetFloor; i < UpRequests.Length; i++)
            {
                if (UpRequests[i]) return i;
            }

            return -1;
        }

        public int GetUpperFloorDownReqeust(int currentFloor)
        {
            for (int i = currentFloor; i < floorHeight; i++)
            {
                if (UpRequests[i]) return i;
            }

            return -1;
        }

        // Request from lower floor
        
        public int GetLowerFloorDownRequests(in int currentFloor)
        {
            for (int i = currentFloor; i >=0 ; i--)
            {
                if (DownRequests[i]) return i;
            }

            return -1;
        }

        public int GetLowerFloorUpRequests(int currentFloor)
        {
            for(var i = currentFloor; i>=0; i--)
            {
                if (UpRequests[i]) return i;
            }

            return -1;
        }
    }
}