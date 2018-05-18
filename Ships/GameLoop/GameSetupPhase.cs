using ShipsGame.Core;

namespace Ships.GameLoop
{
    public static class GameSetupPhase
    {
        public static string Setup(string command, ref GameData gameData)
        {
            var formatTableString = $"<b>{command}</b>";

            if (string.IsNullOrEmpty(gameData.Player1Name))
            {
                gameData.Player1Name = formatTableString;
                gameData.Player1Board = new Board(Actor.PlayerOne);
                return $"Player One name is {gameData.Player1Name} \r\n" +
                       $"Enter Player Two name:\r\n";
            }

            if (string.IsNullOrEmpty(gameData.Player2Name))
            {
                gameData.Player2Name = formatTableString;
                gameData.Player2Board = new Board(Actor.PlayerTwo);

                gameData.CurrentPlayer = Actor.PlayerOne;
                return $"Player two name is {gameData.Player2Name} \r\n Send R for Random Ship Placing. Send anything else to enable manual setup";
            }

            if (command == "R" || command == "r")
            {
                gameData.GameState = GameState.RandomShips;
                var nextStepMessage = ShipPlacingPhase.RandomShipPlacing(ref gameData);
                return $"RandomSetupEnabled \r\n{nextStepMessage}";
            }
            else
            {
                gameData.GameState = GameState.ShipPlacing;
                var nextStepMessage = ShipPlacingPhase.PlaceShip(string.Empty, ref gameData);
                return $"Manual setup enabled \r\n {nextStepMessage}";
            }
        }

    }
}