using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ships.Action;

namespace ShipsGame.Core
{
    public class Ship
    {
        public ShipTypes shipType { get; }

        public bool IsPlaced { get; protected set; }

        private List<BoardCell> ShipPosition { get; }
        protected readonly int shipSize;
        public Ship(ShipTypes shipType)
        {
            this.shipType = shipType;
            shipSize = Settings.GetShipSize(shipType);
            ShipPosition = new List<BoardCell>();
        }

        /// <summary>
        /// Places ship has it goes either up-> dowm
        /// or left -> right
        /// </summary>
        /// <param name="startingPosition">Cell were to start</param>
        /// <param name="direction">Direction in which place ship</param>
        /// <param name="BoardCells">for markin them as used</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ActionResult PlaceShip(CellID startingPosition, Direction direction,ref List<BoardCell> BoardCells)
        {
            if (IsPlaced)
            {
                return new ActionResult(ActionStatus.Failure, "Ship is already on board", false);
            }
            
            //check if ship is in boarder;
            var planedShipPosition = GetPlanedShipPosition(startingPosition, direction);

            if (planedShipPosition.Count != shipSize)
            {
                var FailureMessage = $"Ship can't be placed on {string.Join(", ",planedShipPosition.Select(cell=>cell.Id).ToArray())}, * becase it is to long and goes beyond boarder";
                return new ActionResult(ActionStatus.Failure, FailureMessage, true);
            }

            //check if cell are free
            var takenCells = new List<String>();
            //todo find smother way to do it
            foreach (var cell in planedShipPosition)
            {
                if(BoardCells.Any(bc => bc.Id == cell.Id && bc.IsPartOfTheShip()))
                {
                    takenCells.Add(BoardCells.Where(bc => bc.Id == cell.Id).Select(bc => bc.Id).First());
                }
            }

            if (takenCells.Count > 0)
            {                  
                var FailureMessage = $"Ship can't be placed on {string.Join(", ",planedShipPosition.Select(cell=>cell.Id).ToArray())} becase {string.Join(" ,",takenCells.ToArray())} belongs to diffrent ship";
                return new ActionResult(ActionStatus.Failure, FailureMessage, true);
            }
           
            //Add cells to ship

            foreach (var cell in planedShipPosition)
            {
                var boardCell = BoardCells.First(bc => bc.Id == cell.Id);
                boardCell.MarkAsPartOfTheShip();
                ShipPosition.Add(boardCell);
            }
            //marks cells as taken
            IsPlaced = true;
            
            return new ActionResult(ActionStatus.Succes, string.Empty, false);
        }

        public ActionResult FireAtShip(CellID target)
        {
            if (ShipPosition.All(c => c.Id != target.Id))
            {
                return new ActionResult(ActionStatus.Failure, "Illegal operation - misses shoudn't come here", false);
            }
            var messege = String.Empty;
            ShipPosition.First(c=>c.Id == target.Id).MarkAsFired();
            if (ShipPosition.All(c => c.WasFired() == true))
            {
                messege = $"Enemy {shipType.ToString()} was Sunk!";
            }
            else
            {
                messege = $"Enemy {shipType.ToString()} was Hit!";
            }
            
            return  new ActionResult(ActionStatus.Succes, messege, false);
            
        }

        private List<CellID> GetPlanedShipPosition(CellID startingPosition, Direction direction)
        {
            switch (direction)
            {
                    case Direction.Vertical:
                        return GetPlanedShipPositionToDownword(startingPosition);
                        break;
                    case Direction.Horizontal:
                        return GetPlanedShipPositionToRight(startingPosition);
                        break;
                    default:
                        throw new InvalidEnumArgumentException();
            }
        }

        private List<CellID> GetPlanedShipPositionToRight(CellID startingPosition)
        {
            var cellIds = new List<CellID>();
            cellIds.Add(startingPosition);
            var nextCell = startingPosition.GetIdToRight();
            for (int i = 1; i < shipSize; i++)
            {
                var cellId = new CellID(nextCell);
                cellIds.Add(cellId);
                nextCell = cellId.GetIdToRight();
                if (string.IsNullOrEmpty(nextCell))
                {
                    break;
                }
            }

            return cellIds;
        }

        private List<CellID> GetPlanedShipPositionToDownword(CellID startingPosition)
        {
            var cellIds = new List<CellID>();
            cellIds.Add(startingPosition);
            var nextCell = startingPosition.GetIdBellow();
            for (int i = 1; i < shipSize; i++)
            {
                if (string.IsNullOrEmpty(nextCell))
                {
                    break;
                }
                var cellId = new CellID(nextCell);
                cellIds.Add(cellId);
                nextCell = cellId.GetIdBellow();
            }

            return cellIds;
        }
    }
}