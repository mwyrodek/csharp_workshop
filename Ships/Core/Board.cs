using System;
using System.Collections.Generic;
using System.Linq;
using Ships.Action;
using Ships.UI;

namespace ShipsGame.Core
{
    public class Board
    {
        protected internal List<BoardCell> PlayerBoard;
        protected internal List<Ship> PlayerShips;
        public Actor Owner { get; }
        
        public Board(Actor actor)
        {
            PlayerBoard = new List<BoardCell>();
            PlayerShips = new List<Ship>();
            CreateBoard();
            PrepareShips();
        }

        private void PrepareShips()
        {
            foreach (ShipTypes shipType in Enum.GetValues(typeof(ShipTypes)))
            {
                for (var i = 0; i < Settings.GetShipTypeCountLimit(shipType); i++)
                {
                        PlayerShips.Add(new Ship(shipType));
                }
            }
        }

        private void CreateBoard()
        {
            var firstCell = CellID.CreateFirstCell();
           
            CreateBoard(firstCell);
        }

        private void CreateBoard(CellID id)
        {
            PlayerBoard.Add(new BoardCell(id));

            foreach (var cellstring in id.GetNeighbourIDs())
            {
                if (PlayerBoard.Any(bc => bc.Id == cellstring))
                {
                    continue;
                }
                CreateBoard(new CellID(cellstring));
            }
        }

        public ActionResult PlaceShip(Command tranlateCommand)
        {
            return PlaceShip(tranlateCommand.ShipType, tranlateCommand.Cell, tranlateCommand.Direction);
        }

        public ActionResult PlaceShip(ShipTypes ship, CellID id, Direction placingDirection)
        {
            var candidate = PlayerShips.FirstOrDefault(s => s.shipType == ship && !s.IsPlaced);
            if (candidate == null)
            {
                var faliuteMessege = $"All shipes of type: {ship.ToString()} were already placed";
                return new ActionResult(ActionStatus.Failure, faliuteMessege, true);
            }
            var actionResult = candidate.PlaceShip(id, placingDirection, ref PlayerBoard);

            return actionResult;
        }

        public ActionResult FireMissle(CellID cellId)
        {
            if (PlayerBoard.First(c => c.Id == cellId.Id).WasFired())
            {
                return new ActionResult(ActionStatus.Failure, $"{cellId.Id} was already shot at", true);
            }
            
            PlayerBoard.First(c => c.Id == cellId.Id).MarkAsFired();
            return new ActionResult(ActionStatus.Succes, $"{cellId.Id} Shot missed", false);
        }
    }
}