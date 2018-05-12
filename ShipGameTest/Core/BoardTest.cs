using System;
using NUnit.Framework;
using ShipsGame.Core;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ships.Action;

namespace ShipGameTest
{
    [TestFixture()]
    public class BoardTest
    {
        [Test()]
        public void Setup_Board()
        {
            
            var board = new TestBoard(Actor.PlayerOne);
            var count = board.GetBoard().Count;
            Assert.That(count, Is.EqualTo(99)); 
        }

        [Test()]
        public void Validate_Count()
        {
            
            var board = new TestBoard(Actor.PlayerOne).GetBoard();
            
            Assert.That(board.Count(bc => bc.Id.Contains("A")), Is.EqualTo(9)); 
            Assert.That(board.Count(bc => bc.Id.Contains("D")), Is.EqualTo(9)); 
            Assert.That(board.Count(bc => bc.Id.Contains("K")), Is.EqualTo(9)); 
            Assert.That(board.Count(bc => bc.Id.Contains("1")), Is.EqualTo(11)); 
            Assert.That(board.Count(bc => bc.Id.Contains("5")), Is.EqualTo(11)); 
            Assert.That(board.Count(bc => bc.Id.Contains("9")), Is.EqualTo(11)); 
        }
        
        [Test()]
        public void Setup_Board_Ship_Are_prepered_not_set()
        {

            var board = new TestBoard(Actor.PlayerOne);
            var count = board.GetShips().Count;
            Assert.That(count, Is.EqualTo(5));
        }
        
        
        [Test()]
        public void Setup_Board_Owner_set_Correctly()
        {
            
            var board = new TestBoard(Actor.PlayerOne);

            Assert.That(board.Owner, Is.EqualTo(Actor.PlayerOne));
        }

        [Test]
        public void ShipProperlyPlaced_Returns_Succes()
        {
            
            var board = new TestBoard(Actor.PlayerOne);

            var result = board.PlaceShip(ShipTypes.Battleship, new CellID("A5"), Direction.Vertical);
            
            Assert.That(result.Status, Is.EqualTo(ActionStatus.Succes));
            var isShipPlaced = board.GetShips().Any(s => s.IsPlaced && s.shipType == ShipTypes.Battleship);
            Assert.That(isShipPlaced, Is.True, "Ship wasn't placed");
        }
        
        [Test]
        public void ShipCantBePlaced_Returns_Faliure()
        {
            
            var board = new TestBoard(Actor.PlayerOne);

            var result = board.PlaceShip(ShipTypes.Battleship, new CellID("A1"), Direction.Vertical);
            
            Assert.That(result.Status, Is.EqualTo(ActionStatus.Failure));
            var isShipPlaced = board.GetShips().Any(s => !s.IsPlaced && s.shipType == ShipTypes.Battleship);
            Assert.That(isShipPlaced, Is.True, "Ship was placed while it shoudn't");
        }      
        
        [Test]
        public void PlaceShip_NoMoreShips_Faliure()
        {
            
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Battleship, new CellID("A5"), Direction.Vertical);
            var result = board.PlaceShip(ShipTypes.Battleship, new CellID("A6"), Direction.Vertical);
            
            Assert.That(result.Status, Is.EqualTo(ActionStatus.Failure));
            Assert.That(result.Messege, Is.EqualTo("All shipes of type: Battleship were already placed"));
            Assert.That(result.AllowRepeat, Is.True);
        }

        [Test]
        public void FireAtShip_Miss_ReturnSuccess()
        {
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Battleship, new CellID("A5"), Direction.Vertical);
            ActionResult result = board.FireMissle(new CellID("C3"));
            
            Assert.That(result.Status, Is.EqualTo(ActionStatus.Succes));
            Assert.That(result.Messege, Is.EqualTo("C3 Shot missed"));
            Assert.That(result.AllowRepeat, Is.False);
        }
        
        [Test]
        public void FireAtShip_AlreadyHit_ReturnFailureRepaet()
        {
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Battleship, new CellID("A5"), Direction.Vertical);
            board.FireMissle(new CellID("C3"));
            
            ActionResult result = board.FireMissle(new CellID("C3"));
            Assert.That(result.Status, Is.EqualTo(ActionStatus.Failure));
            Assert.That(result.Messege, Is.EqualTo("C3 was already shot at"));
            Assert.That(result.AllowRepeat, Is.True);
        }
        
        [Test]
        public void FireAtShip_Hit_ReturnSuccess()
        {
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Battleship, new CellID("A5"), Direction.Vertical);
           
            ActionResult result = board.FireMissle(new CellID("A5"));
            Assert.That(result.Status, Is.EqualTo(ActionStatus.Succes));
            Assert.That(result.Messege, Contains.Substring("Enemy Battleship was Hit!"));
            Assert.That(result.AllowRepeat, Is.False);
        }
        
        [Test]
        public void FireAtShip_Hit_ShipSunkReturnSuccess()
        {
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Destroyer, new CellID("A5"), Direction.Vertical);
           
            board.FireMissle(new CellID("A5"));
            ActionResult result = board.FireMissle(new CellID("A4"));
            Assert.That(result.Status, Is.EqualTo(ActionStatus.Succes));
            Assert.That(result.Messege, Contains.Substring("Enemy Destroyer was Sunk!"));
            Assert.That(result.AllowRepeat, Is.False);
        }

        [Test]
        public void IsAllShipsPlaced_shipnotplaced_RetrunsFalse()
        {
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Battleship, new CellID("A5"), Direction.Vertical);

            bool isAllPlaced = board.IsAllPlaced();
            Assert.That(isAllPlaced, Is.False);
        }
        
        [Test]
        public void IsAllPlaced_allplaced_returnsTrue()
        {
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Battleship, new CellID("A5"), Direction.Vertical);
            board.GetShips().RemoveAll(s => s.IsPlaced == false);

            var isAllPlaced = board.IsAllPlaced();
            Assert.That(isAllPlaced, Is.True, "Not all ship were placed and yet answer was true");        }

        [Test]
        public void IsAllShipsSunk_shipstanding_returnsFalse()
        {
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Destroyer, new CellID("A5"), Direction.Vertical);
            board.PlaceShip(ShipTypes.Carrier, new CellID("B7"), Direction.Vertical);
            board.FireMissle(new CellID("A5"));
            board.FireMissle(new CellID("A4"));

            var isAllShipSunk = board.IsAllShipSunk();
            Assert.That(isAllShipSunk, Is.False);
        }

        [Test]
        public void IsAllShipsSunk_AllSunk_returnsTrue()
        {
            var board = new TestBoard(Actor.PlayerOne);
            board.PlaceShip(ShipTypes.Destroyer, new CellID("A5"), Direction.Vertical);
            board.FireMissle(new CellID("A5"));
            board.FireMissle(new CellID("A4"));
            board.GetShips().RemoveAll(s => s.IsPlaced == false);
            
            var isAllShipSunk = board.IsAllShipSunk();
            Assert.That(isAllShipSunk, Is.True);

        }
    }


    public class TestBoard : Board
    {
        public TestBoard(Actor actor) : base(actor)
        {
        }

        public List<BoardCell> GetBoard()
        {
            return PlayerBoard;
        }
        
        public List<Ship> GetShips()
        {
            return PlayerShips;
        }
    }
}
