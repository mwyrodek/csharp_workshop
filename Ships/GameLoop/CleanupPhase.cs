using System.Text;

namespace Ships.GameLoop
{
    public class CleanupPhase
    {
        public static string GameWonState(ref GameData data)
        {
            
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Congratulation Player {data.GetCurrentPlayerName()} Won!\r\n");
            stringBuilder.Append("Starting new game.\r\n");
            stringBuilder.Append("Please type first player name:\r\n");
            data=new GameData();
            return stringBuilder.ToString();
        }
    }
}