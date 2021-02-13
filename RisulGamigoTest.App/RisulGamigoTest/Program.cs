using System;
using RisulGamigoTest.Problem3_RoomGraph;

namespace RisulGamigoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var room1 = new Room("room1");
            var room2 = new Room("room2");
            var room3 = new Room("room3");
            var room4 = new Room("room4");
            var room5 = new Room("room5");
            var room6 = new Room("room6");

            room1.AddRoom(room2, RoomDirection.East);
            room2.AddRoom(room3, RoomDirection.North);
            room3.AddRoom(room4, RoomDirection.North);
            room4.AddRoom(room5, RoomDirection.East);
            room5.AddRoom(room6, RoomDirection.South);

            var pathExists = room1.PathExistsTo("room6");
            pathExists = room3.PathExistsTo("room1");
            
            Console.WriteLine();

        }
    }
}
