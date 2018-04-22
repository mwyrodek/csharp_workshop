using NUnit.Framework;
using ShipsGame.Core;
using System.Collections.Generic;
using System;
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
        public void Setup_Board_Ship_Are_prepered_not_set()
        {

            var board = new TestBoard(Actor.PlayerOne);
            var count = board.GetBoard().Count;
            Assert.That(count, Is.EqualTo(99));
        }
        [Test()]
        public void Set_ship_succes()
        {
            
        }
        // place taken
        //no ship of this size avaible for setting
         

    }


    public class TestBoard : Board
    {
        public TestBoard(Actor actor) : base(actor)
        {
        }

        public Dictionary<string, BoardCell> GetBoard()
        {
            return PlayerBoard;
        }
    }
}
