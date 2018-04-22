using System;
using System.Runtime.InteropServices;

namespace ShipsGame.Core
{
    public class BoardCell
    {
        private bool fired;
        private bool partOfShip;
        public BoardCell(CellID id)
        {

        }

        public bool WasFired()
        {
            return fired;
        }

        public bool IsPartOfTheShip()
        {
            return partOfShip;
        }

        public void MarkAsPartOfTheShip()
        {
            if(partOfShip)
            {
                throw new InvalidOperationException("Can't Add Ship to cell that already has ship");
            }
            partOfShip = true;
        }

        public void MarkAsFired()
        {
            if (fired)
            {
                throw new InvalidOperationException("This cell was already fired upon");
            }
            fired = true;
        }
    }
}