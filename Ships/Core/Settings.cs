using System;

namespace ShipsGame.Core
{

    //todo make it dependan on config files
    public static class Settings
    {
        /// <summary>
        /// Width is represented to user by Letters A B C D
        /// A is to left
        /// k is to right
        /// </summary>
        public const int BoardWidth = 11;
        /// <summary>
        /// Height is presented by numbers 1 2 3 4
        /// 9 is highest
        /// 1 is lowest
        /// </summary>
        public const int BoardHeight = 9;
        public static int GetShipSize(ShipTypes type)
        {
            switch (type)
            {
                case ShipTypes.Battleship:
                    return 4;
                case ShipTypes.Carrier:
                    return 5;
                case ShipTypes.Crusier:
                case ShipTypes.Submarine:
                    return 3;
                case ShipTypes.Destroyer:
                    return 2;
                default:
                    throw new ArgumentOutOfRangeException($"({type} is out of range)");
            }
        }

        public static int GetShipTypeCountLimit(ShipTypes type)
        {
            switch (type)
            {
                case ShipTypes.Battleship:
                case ShipTypes.Carrier:
                case ShipTypes.Crusier:
                case ShipTypes.Submarine:
                case ShipTypes.Destroyer:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException($"({type} is out of range)");
            }
        }
    }
}