using System;
using System.Runtime.InteropServices;

namespace ShipsGame.Core
{
    public class BoardCell
    {
        private bool fired;
        private bool partOfShip;
        private CellID cellId;

        public string Id
        {
            get { return cellId.Id; }
        }
        public BoardCell(CellID cellId)
        {
            this.cellId = cellId;
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