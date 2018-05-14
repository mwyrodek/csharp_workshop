using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ships.UI;
using ShipsGame.Core;

namespace Ships.StubsForTestingViaUI
{
    public class ShipReadForTest
    {
        private List<BoardCell> FakeBoard;
        private Ship ship;
        public ShipReadForTest()
        {
            FakeBoard = new List<BoardCell>();
            var list = new List<string>()
            {
                "A1",
                "A2",
                "A3",
                "A4",
                "A5",
                "B1",
                "B2",
                "B3",
                "B4",
                "B5",
                "C1",
                "C2",
                "C3",
                "C4",
                "C5",
                "D1",
                "D2",
                "D3",
                "D4",
                "D5",
                "E1",
                "E2",
                "E3",
                "E4",
                "E5"
            };

            foreach (var cellid in list)
            {
                FakeBoard.Add(new BoardCell(new CellID(cellid)));
            }
            ship = new Ship(ShipTypes.Battleship);
        }


        public string ActOnShip(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return String.Empty;
            }
            if (command.Length > 3)
            {
                var tranlateCommand = InputTranslatorHelper.TranlateCommand(command);
                 ship = new Ship(tranlateCommand.ShipType);
                return ship.PlaceShip(tranlateCommand.Cell, tranlateCommand.Direction, ref FakeBoard).Messege;
            }
            else
            {
                return  ship.FireAtShip(new CellID(command)).Messege;
            }
        }
    }
}
