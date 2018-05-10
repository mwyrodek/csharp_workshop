
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ships.Action;
using ShipsGame.Core;

namespace ShipGameTest
{
    [TestFixture]
    public class ShipTests
    {
        private List<BoardCell> FakeBoard;
        [SetUp]
        public void Setup()
        {
            FakeBoard = new List<BoardCell>();
            var list = new List<string>()
            {
                "A1",
                "A2",
                "A3",
                "B1",
                "B2",
                "B3",
                "C1",
                "C2",
                "C3"
            };

            foreach (var cellid in list)
            {
                FakeBoard.Add(new BoardCell(new CellID(cellid)));
            }
        }
        
        [Test]
        public void CreateShip_sets_proper_size()
        {
            var ship = new TestShip(ShipTypes.Battleship);
            
            Assert.That(ship.GetShipSize(),Is.EqualTo(4));
        }

        [Test]
        public void ShipPlacedCorrectly_returns_succes()
        {
            var ship = new TestShip(ShipTypes.Destroyer);

            var actionResult = ship.PlaceShip(new CellID("B1"), Direction.Horizontal,ref FakeBoard);
            var actualShipPosition = FakeBoard.Where(ci => ci.IsPartOfTheShip()).Select(ci => ci.Id).ToList();
            var expectedShipPosition = new List<string>()
            {
                "B1",
                "C1"
            };
            Assert.That(actualShipPosition, Is.EqualTo(expectedShipPosition));
            Assert.That(ship.IsPlaced, Is.True);
            Assert.That(actionResult.Status, Is.EqualTo(ActionStatus.Succes));
        }
        
        [Test]
        public void ShipPlacedOnTakenSpace_returns_failure()
        {
            var ship = new TestShip(ShipTypes.Destroyer);
            FakeBoard.First(ci => ci.Id == "B2").MarkAsPartOfTheShip();
            var actionResult = ship.PlaceShip(new CellID("B3"), Direction.Vertical, ref FakeBoard);
  
 
            Assert.That(ship.IsPlaced, Is.False);
            Assert.That(actionResult.Status, Is.EqualTo(ActionStatus.Failure));
            Assert.That(actionResult.Messege, Is.EqualTo("Ship can't be placed on B3, B2 becase B2 belongs to diffrent ship \n"));
        }
        
        
        [Test]
        public void ShipGoesOutOfBoarder_returns_failure()
        {
            var ship = new TestShip(ShipTypes.Carrier);
            var actionResult = ship.PlaceShip(new CellID("B1"), Direction.Vertical, ref FakeBoard);
  
 
            Assert.That(ship.IsPlaced, Is.False);
            Assert.That(actionResult.Status, Is.EqualTo(ActionStatus.Failure));
            Assert.That(actionResult.Messege, Is.EqualTo("Ship can't be placed on B1, * becase it is to long and goes beyond boarder \n"));
        }

        [Test]
        public void ShipAlready_Placed()
        {
            var ship = new TestShip(ShipTypes.Destroyer);
            ship.MarkShipAsPlaced();
            var actionResult = ship.PlaceShip(new CellID("B1"), Direction.Horizontal, ref FakeBoard);

            Assert.That(ship.IsPlaced, Is.True);
            Assert.That(actionResult.Status, Is.EqualTo(ActionStatus.Failure));
            Assert.That(actionResult.Messege, Is.EqualTo("Ship is already on board\n"));
        }
       
        
        // fired ship hit
        [Test]
        public void FireAtShip_Miss()
        {
            var testShip = new TestShip(ShipTypes.Destroyer);
            testShip.PlaceShip(new CellID("B1"), Direction.Horizontal, ref FakeBoard);
            var actionResult = testShip.FireAtShip(new CellID("A1"));
            
            Assert.That(actionResult.AllowRepeat, Is.False);
            Assert.That(actionResult.Status, Is.EqualTo(ActionStatus.Failure));
            Assert.That(actionResult.Messege, Is.EqualTo("Illegal operation - misses shoudn't come here\n"));
        }

        [Test]
        public void FireAtShip_Hit()
        {
            var testShip = new TestShip(ShipTypes.Destroyer);
            testShip.PlaceShip(new CellID("B1"), Direction.Horizontal, ref FakeBoard);
            var actionResult = testShip.FireAtShip(new CellID("C1"));
            
            Assert.That(actionResult.AllowRepeat, Is.False);
            Assert.That(actionResult.Status, Is.EqualTo(ActionStatus.Succes));
            Assert.That(actionResult.Messege, Is.EqualTo("Enemy Destroyer was Hit!\n"));
            Assert.That(FakeBoard.First(cell => cell.Id == "C1").WasFired, Is.True);
        }

        [Test]
        public void FireAtShip_Sunk()
        {
            var testShip = new TestShip(ShipTypes.Destroyer);
            testShip.PlaceShip(new CellID("B1"), Direction.Horizontal, ref FakeBoard);
            testShip.FireAtShip(new CellID("C1"));
            var actionResult = testShip.FireAtShip(new CellID("B1"));
            
            Assert.That(actionResult.AllowRepeat, Is.False);
            Assert.That(actionResult.Status, Is.EqualTo(ActionStatus.Succes));
            Assert.That(actionResult.Messege, Is.EqualTo("Enemy Destroyer was Sunk!\n"));
            Assert.That(FakeBoard.First(cell => cell.Id == "B1").WasFired, Is.True);
        }

        
    }

    public class TestShip : Ship
    {
        public int GetShipSize()
        {
            return shipSize;
        }

        public TestShip(ShipTypes shipType) : base(shipType)
        {
        }

        public void MarkShipAsPlaced()
        {
            IsPlaced = true;
        }
    }
}
