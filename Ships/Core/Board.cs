using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Owner = actor;
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
                var faliuteMessege = $"All ships of type: {ship.ToString()} were already placed.\n";
                return new ActionResult(ActionStatus.Failure, faliuteMessege, true);
            }
            var actionResult = candidate.PlaceShip(id, placingDirection, ref PlayerBoard);

            return actionResult;
        }

        public ActionResult FireMissle(CellID cellId)
        {
            if (PlayerBoard.First(c => c.Id == cellId.Id).WasFired())
            {
                return new ActionResult(ActionStatus.Failure, $"{cellId.Id} was already shot at.\n", true);
            }

            var boardCell = PlayerBoard.First(c => c.Id == cellId.Id);
            if (boardCell.IsPartOfTheShip())
            {
                var first = PlayerShips.First(s => s.ShipPosition.Any(pos => pos.Id == cellId.Id));
                return first.FireAtShip(cellId);
            }

            boardCell.MarkAsFired();
            return new ActionResult(ActionStatus.Succes, $"{cellId.Id} Shot missed.\n", false);
            
        }

        public bool IsAllPlaced()
        {
            return PlayerShips.All(s => s.IsPlaced);
        }

        public bool IsAllShipSunk()
        {
            return PlayerShips.All(s => s.IsShipSunk());
        }

        /// <summary>
        /// Generate maps with makres of fire
        /// </summary>
        /// <returns></returns>
        public string HitBoardHtmlMap()
        {
            
            StringBuilder map = new StringBuilder();

            map.Append(TableHeader());

            for (int i = 9; i > 0; i--)
            {
                map.Append($"<tr><td>{i}</td>");
                var boardCells = PlayerBoard.Where(c => c.Id.Contains(i.ToString())).OrderBy(c=>c.Id);
                foreach (var boardCell in boardCells)
                {
                    if (boardCell.WasFired())
                    {
                        if (boardCell.IsPartOfTheShip())
                        {
                            var ship = PlayerShips.FirstOrDefault(c => c.ShipPosition.Any(id => id.Id == boardCell.Id));
                            var translateShipTypeToSymbol = InputTranslatorHelper.TranslateShipTypeToSymbol(ship.shipType);
                            map.Append($"<td>{translateShipTypeToSymbol}</td>");
                        }
                        else
                        {
                            map.Append($"<td>X</td>");
                        }
                    }
                    else
                    {
                        map.Append($"<td> </td>");
                    }
                    
                }

                map.Append($"<td>{i}</td></tr>");
            }
            map.Append(TableBottom());
            return map.ToString();
        }

        private string TableBottom()
        {
            StringBuilder TF = new StringBuilder();
            TF.Append("<tr>\n");
            TF.Append(GetCordinatesBar());
            TF.Append("</tr>\n");
            TF.Append("</table>");
            return TF.ToString();
        }

        private string TableHeader()
        {
            StringBuilder TH = new StringBuilder();
            TH.Append("<table>\n");
            TH.Append("<tr>\n");
            TH.Append(GetCordinatesBar());
            TH.Append("</tr>\n");
            return TH.ToString();
        }

        private string GetCordinatesBar()
        {
            StringBuilder Cordianters = new StringBuilder();
            Cordianters.Append("<td>-</td>");
            for (int i = 0; i < Settings.BoardWidth; i++)
            {
                char value = (char) ('A' + i);
                Cordianters.Append($"<td>{value}</td>");
            }
            Cordianters.Append("<td>-</td>");
            return Cordianters.ToString();
        }


        public string ShipsBoardHtmlMap()
        {

            StringBuilder map = new StringBuilder();

            map.Append(TableHeader());

            for (int i = 9; i > 0; i--)
            {
                map.Append($"<tr><td>{i}</td>");
                var boardCells = PlayerBoard.Where(c => c.Id.Contains(i.ToString())).OrderBy(c => c.Id);
                foreach (var boardCell in boardCells)
                {
                    if (boardCell.WasFired())
                    {
                        if (boardCell.IsPartOfTheShip())
                        {
                            var ship = PlayerShips.FirstOrDefault(c => c.ShipPosition.Any(id => id.Id == boardCell.Id));
                            var translateShipTypeToSymbol = InputTranslatorHelper.TranslateShipTypeToSymbol(ship.shipType);
                            map.Append($"<td>O</td>");
                        }
                        else
                        {
                            map.Append($"<td>X</td>");
                        }
                    }
                    else if(boardCell.IsPartOfTheShip())
                    {
                        var ship = PlayerShips.FirstOrDefault(c => c.ShipPosition.Any(id => id.Id == boardCell.Id));
                        var translateShipTypeToSymbol = InputTranslatorHelper.TranslateShipTypeToSymbol(ship.shipType);
                        map.Append($"<td>{translateShipTypeToSymbol}</td>");
                    }
                    else
                    {
                        map.Append($"<td> </td>");
                    }

                }

                map.Append($"<td>{i}</td></tr>");
            }
            map.Append(TableBottom());
            return map.ToString();
        }

        public void PlaceAllShipsAtRandom()
        {
            Random rand = new Random();
            while (PlayerShips.Any(s=>!s.IsPlaced))
            {
                var randomCelll = CellID.RandomCelll();
                var first = PlayerShips.First(s => !s.IsPlaced);
                Direction direct;
                if (rand.Next(2) > 0)
                {
                    direct = Direction.Horizontal;
                }
                else
                {
                    direct = Direction.Vertical;
                }

                first.PlaceShip(randomCelll, direct, ref PlayerBoard);
            }
        }
    }
}