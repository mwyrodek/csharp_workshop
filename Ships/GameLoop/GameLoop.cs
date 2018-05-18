using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Ships.UI;
using ShipsGame.Core;

namespace Ships.GameLoop
{
    public class GameLoop
    {
        GameData gameData = new GameData();

        public string Act(string readLine)
        {
            var message = new StringBuilder();
            if (IsBeginingOfGame(readLine))
            {
                return "Welcome to ships game please type first player name.\r\n";
            }

            if (IsNoInput(readLine))
            {
                message.Append("You need to type something.\r\n");
            }

            if (IsGaveUp(readLine))
            {
                gameData.SetNextPlayerTurn();
                gameData.GameState = GameState.FinnishUp;
            }


            switch (gameData.GameState)
            {
                case GameState.Setup:
                    message.Append(GameSetupPhase.Setup(readLine, ref gameData));
                    break;
                case GameState.ShipPlacing:
                    message.Append(ShipPlacingPhase.PlaceShip(readLine.ToUpper(),ref gameData));
                    break;
                case GameState.FinnishUp:
                    message.Append(CleanupPhase.GameWonState(ref gameData));
                    break;
                case GameState.InProgress:
                    message.Append(FirePhase.TakeFireComand(readLine.ToUpper(),ref gameData));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!gameData.IsWaitingForNextPlayer)
            {
                switch (gameData.GameState)
                {
                    case GameState.InProgress:
                        message.Append(BuildRepresentationCurrentPlayer());
                        break;
                    case GameState.ShipPlacing:
                        message.Append(DisplayeShipsMap());
                        break;
                }
            }

            message.Append("Reminder after Naming Players you can gave up by pressing <b>Q<b>");

            return message.ToString();
        }

        private bool IsGaveUp(string readLine)
        {
            return (readLine == "q" || readLine == "Q") && (gameData.GameState != GameState.Setup);
        }

        private bool IsNoInput(string readLine)
        {
            return string.IsNullOrEmpty(readLine);
        }

        private bool IsBeginingOfGame(string readLine)
        {
            return string.IsNullOrEmpty(readLine) && string.IsNullOrEmpty(gameData.Player1Name);
        }

        private string BuildRepresentationCurrentPlayer()
        {
            StringBuilder ships = new StringBuilder();

            ships.Append(DisplayeShipsMap());
            ships.Append($"<b>Target Board</b>: \t\n");
            ships.Append(DisplayTargetMap(gameData.GetInActivePlayerBoard()));
            return ships.ToString();
        }

        private string DisplayeShipsMap()
        {
            StringBuilder ships = new StringBuilder();
            ships.Append("<b>Your Board</b>: \t\n");
            ships.Append(DisplayShipsMap(gameData.GetCurrentPlayerBoard()));
            return ships.ToString();
        }

        private string DisplayTargetMap(Board PlayerBoard)
        {
            return PlayerBoard.HitBoardHtmlMap();
        }

        private string DisplayShipsMap(Board PlayerBoard)
        {
            return PlayerBoard.ShipsBoardHtmlMap();
        }
    }
}