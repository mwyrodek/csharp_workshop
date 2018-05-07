/*using NUnit.Framework;
using ShipsGame.Core;
using System;

namespace ShipGameTest
{
    [TestFixture()]
    public class BoardCellTests
    {
        [Test()]
        public void NewCell_Is_NotFired()
        {
            var boardCell = new BoardCell(new CellID("A1"));
            Assert.That(boardCell.WasFired(), Is.False, "Field by default are not fired upon");


        }

        [Test()]
        public void NewCell_Has_NoShip()
        {
            var boardCell = new BoardCell(new CellID("A1"));
            Assert.That(boardCell.IsPartOfTheShip(), Is.False, "Field by default are not part of the ship");

        }

        [Test()]
        public void Fire_AtCell_CellMarkedFired()
        {
            var boardCell = new BoardCell(new CellID("A1"));
            boardCell.MarkAsFired();
            Assert.That(boardCell.WasFired(), Is.True, "Field that was fired upon should be marked as fired");


        }

        [Test()]
        public void Fire_AtCellAlreadyFiredAt_ThrowError()
        {
            var boardCell = new BoardCell(new CellID("A1"));
            boardCell.MarkAsFired();
            var exception = Assert.Throws<InvalidOperationException>(
                delegate { boardCell.MarkAsFired(); }
            );


            Assert.That(exception.Message, Contains.Substring("This cell was already fired upon"));
        }


        [Test()]
        public void NewCell_AddShip_MarkAsHavingShip()
        {
            var boardCell = new BoardCell(new CellID("A1"));
            boardCell.MarkAsPartOfTheShip();
            Assert.That(boardCell.IsPartOfTheShip(), Is.True);

        }

        [Test()]
        public void CellWithShip_AddShip_ThrowError()
        {
            var boardCell = new BoardCell(new CellID("A1"));
            boardCell.MarkAsPartOfTheShip();
            var exception = Assert.Throws<InvalidOperationException>(
                delegate { boardCell.MarkAsPartOfTheShip(); }
            );


            Assert.That(exception.Message, Contains.Substring("Can't Add Ship to cell that already has ship"));
        }
    }
}*/