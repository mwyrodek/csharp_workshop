using System.Text;
using Ships.UI;
using ShipsGame.Core;

namespace Ships.GameLoop
{
    public static class ShipPlacingPhase
    {
        private static GameData gameData;
        public static string RandomShipPlacing(ref GameData data)
        {
            gameData = data;
            gameData.Player2Board.PlaceAllShipsAtRandom();
            gameData.Player1Board.PlaceAllShipsAtRandom();
            gameData.GameState = GameState.InProgress;
            return FirePhase.TakeFireComand(string.Empty, ref gameData);
        }

        public static string PlaceShip(string command, ref GameData data)
        {
            gameData = data;
            StringBuilder message = new StringBuilder();
            if (!string.IsNullOrEmpty(command))
            {
                if (!ValidateShipPlacementCommand(command))
                {
                    message.Append($"\"{command}\" is not valid input\r\n");
                }
                else
                {
                    var shipsMessage = PlaceShips(command);
                    message.Append(shipsMessage);
                }
            }

            if (IsAllShipPlaced())
            {
                gameData.GameState = GameState.InProgress;
                return FirePhase.TakeFireComand(string.Empty, ref gameData);

            }

            message.Append($"Enter {gameData.GetCurrentPlayerName()} Ship and its place:\r\n");
            message.Append(ShipPlacementHint());
            return message.ToString();
        }

        private static bool IsAllShipPlaced()
        {
            return gameData.Player1Board.IsAllPlaced() && gameData.Player2Board.IsAllPlaced();
        }

        private static string PlaceShips(string command)
        {
            var board = gameData.GetCurrentPlayerBoard();
            var actionResult = board.PlaceShip(InputTranslatorHelper.TranlateCommand(command));
            if (!actionResult.AllowRepeat)
            {
                gameData.SetNextPlayerTurn();
            }

            return actionResult.Messege;

        }

        private static string ShipPlacementHint()
        {
            return $"To place ship sent ship ShipSymbol, Direction, and Starting Field \r\n  example BVA9\r\n" +
                   $"Ship Types and size \r\n" +
                   $"<b>B</b> - {ShipTypes.Battleship} {Settings.GetShipSize(ShipTypes.Battleship)}\r\n" +
                   $"<b>K</b> - {ShipTypes.Carrier} {Settings.GetShipSize(ShipTypes.Carrier)}\r\n" +
                   $"<b>C</b> - {ShipTypes.Crusier} {Settings.GetShipSize(ShipTypes.Crusier)}\r\n" +
                   $"<b>D</b> - {ShipTypes.Destroyer} {Settings.GetShipSize(ShipTypes.Destroyer)}\r\n" +
                   $"<b>S</b> - {ShipTypes.Submarine} {Settings.GetShipSize(ShipTypes.Submarine)}\r\n" +
                   $"Directions V - vertical UP => Down   H - Horizontal Left => Right\r\n";
        }
        private static bool ValidateShipPlacementCommand(string command)
        {
            if (command.Length < 4)
            {
                return false;
            }
            return InputTranslatorHelper.IsShip(command[0]) &&
                   (InputTranslatorHelper.IsDirection(command[1]) &&
                    CellID.IsIdValid(command.Substring(2)));
        }

    }
}