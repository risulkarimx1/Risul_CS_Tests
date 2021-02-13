using System;
using NUnit.Framework;
using RisulGamigoTest.Problem3_RoomGraph;

namespace RisulGamigoUnitTests
{
    [TestFixture]
    public class Problem3Tests
    {
        private Room room1 = new Room("room1");
        private Room room2 = new Room("room2");
        private Room room3 = new Room("room3");
        private Room room4 = new Room("room4");
        private Room room5 = new Room("room5");
        private Room room6 = new Room("room6");

        private Room room7 = new Room("room7");
        private Room room8 = new Room("room8");

        [SetUp]
        public void SetUp()
        {
            room1.AddRoom(room2, RoomDirection.East);
            room2.AddRoom(room3, RoomDirection.North);
            room3.AddRoom(room4, RoomDirection.North);
            room4.AddRoom(room5, RoomDirection.East);
            room5.AddRoom(room6, RoomDirection.South);

            room7.AddRoom(room8, RoomDirection.South);
        }

        [Test]
        public void RoomCannotConnectToItself()
        {
            Assert.Throws<InvalidOperationException>(() => room1.AddRoom(room1, RoomDirection.East));
        }

        [Test]
        public void Room1IsConnectedToRoom6()
        {
            var pathExists = room1.PathExistsTo("room6");
            Assert.IsTrue(pathExists);
        }

        [Test]
        public void Room1CantBeReachedFromRoom6()
        {
            var pathExists = room6.PathExistsTo("room1");
            Assert.IsFalse(pathExists);
        }

        [Test]
        public void Room2CantBeReachedFromRoom5()
        {
            var pathExists = room2.PathExistsTo("room5");
            Assert.IsTrue(pathExists);
        }

        [Test]
        public void Room7CantBeReachedFromRoom3()
        {
            var pathExists = room3.PathExistsTo("room7");
            Assert.IsFalse(pathExists);
        }

        [Test]
        public void Room7IsConnectedToRoom8()
        {
            var pathExists = room7.PathExistsTo("room8");
            Assert.IsTrue(pathExists);
        }
    }
}