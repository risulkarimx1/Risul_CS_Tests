using System;

namespace RisulGamigoTest.Problem3_RoomGraph
{
    public class Room : IPathable
    {
        private readonly string _name;
        private const int _doorCount = 4;

        public Room North { get; private set; }
        public Room South { get; private set; }
        public Room East { get; private set; }
        public Room West { get; private set; }

        public string Name => _name;

        public Room(string name)
        {
            _name = name;
        }

        public void AddRoom(Room otherRoom, RoomDirection roomDirection)
        {
            if (otherRoom == this) throw new InvalidOperationException("Can't add a room to itself");

            switch (roomDirection)
            {
                case RoomDirection.North:
                    North = otherRoom;
                    break;
                case RoomDirection.South:
                    South = otherRoom;
                    break;
                case RoomDirection.East:
                    East = otherRoom;
                    break;
                case RoomDirection.West:
                    West = otherRoom;
                    break;
            }
        }

        public bool PathExistsTo(string endingRoomName)
        {
            if (_name == endingRoomName) return true;

            for (int i = 0; i < _doorCount; i++)
            {
                if (this[i] != null)
                {
                    return this[i].PathExistsTo(endingRoomName);
                }
            }

            return false;
        }

        public Room this[int index] =>
            index is 0 ? North :
            index is 1 ? South :
            index is 2 ? East : West;

        public override string ToString()
        {
            return Name;
        }
    }
}