using System;
using System.ComponentModel;
using System.Linq;
using ShipsGame.Core;

namespace Ships.UI
{
    public class InputTranslatorHelper
    {
        private static string ValidDirections = "VH";
        private static string ValidShips = "BKCDS";

        public static Direction TranslateToDirection(char input)
        {
            switch (input)
            {
                case 'H':
                    return Direction.Horizontal;
                case 'V':
                    return Direction.Vertical;
                default:
                    throw new ArgumentOutOfRangeException($"{input} is not valid");
            }
        }

        public static bool IsDirection(char symbol)
        {
            return ValidDirections.Contains(symbol);
        }

        public static bool IsShip(char symbol)
        {
            return ValidShips.Contains(symbol);
        }

        public static ShipTypes TranslateToShip(char input)
        {
            switch (input)
            {
                case 'B':
                    return ShipTypes.Battleship;
                case 'K':
                    return ShipTypes.Carrier;
                case 'C':
                    return ShipTypes.Crusier;
                case 'D':
                    return ShipTypes.Destroyer;
                case 'S':
                    return ShipTypes.Submarine;
                default:
                    throw new ArgumentOutOfRangeException($"{input} is not valid");
            }
        }

        public static char TranslateShipTypeToSymbol(ShipTypes ship)
        {
            switch (ship)
            {
                case ShipTypes.Battleship:
                    return 'B';
                case ShipTypes.Carrier:
                    return 'K';
                case ShipTypes.Crusier:
                    return 'C';
                case ShipTypes.Destroyer:
                    return 'D';
                case ShipTypes.Submarine:
                    return 'S';
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public static Command TranlateCommand(string command)
        {
            var translateToShip = TranslateToShip(command[0]);
            var translateToDirection = TranslateToDirection(command[1]);
            var cellId = new CellID(command.Substring(2));
            return new Command(cellId, translateToDirection, translateToShip);
        }
    }
}