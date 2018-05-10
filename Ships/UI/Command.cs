using ShipsGame.Core;

namespace Ships.UI
{
    public class Command
    {
        public CellID Cell;
        public Direction Direction;
        public ShipTypes ShipType;

        public Command(CellID cell, Direction direction, ShipTypes shipType)
        {
            Cell = cell;
            Direction = direction;
            ShipType = shipType;
        }
    }
}