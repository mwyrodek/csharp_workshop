using System.Text;
using ShipsGame.Core;

namespace Ships.GameLoop
{
    public class FirePhase
    {
        private static GameData gameData;
        public static string TakeFireComand(string command, ref GameData data)
        {
            gameData = data;
            StringBuilder messege = new StringBuilder();
            if (CellID.IsIdValid(command))
            {
                var cellId = new CellID(command);
                var actionResult = gameData.GetCurrentPlayerBoard().FireMissle(cellId);

                if (IsAllEnemyShipSunk())
                {
                    gameData.GameState = GameState.FinnishUp;
                    return CleanupPhase.GameWonState(ref gameData);
                }

                if (!actionResult.AllowRepeat)
                {
                    gameData.SetNextPlayerTurn();
                }
                messege.Append(actionResult.Messege);
                messege.AppendLine();
            }
            else if(!string.IsNullOrEmpty(command))
            {
                messege.Append($"{command} is not valid command.\r\n");
            }
            messege.Append($"Player {gameData.GetCurrentPlayerName()} Turn: provide your target.\r\n");
            return messege.ToString();
        }


        private static bool IsAllEnemyShipSunk()
        {

            //todo candidate for bug change it to current player
            return gameData.GetInActivePlayerBoard().IsAllShipSunk();
        }
    }
}