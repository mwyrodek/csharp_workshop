using System;
using ShipsGame.Core;

namespace Ships.GameLoop
{
    public class GameData
    {
        public GameState GameState = GameState.Setup;
        public string Player1Name;
        public string Player2Name;
        public Board Player1Board;
        public Board Player2Board;
        public Actor CurrentPlayer;

        public void SetNextPlayerTurn()
        {
            if (CurrentPlayer == Actor.PlayerOne)
            {
                CurrentPlayer = Actor.PlayerTwo;
                return;
            }

            CurrentPlayer = Actor.PlayerOne;
        }

        public Board GetCurrentPlayerBoard()
        {
            return CurrentPlayer == Actor.PlayerOne ? Player1Board : Player2Board;
        }

        public Board GetInActivePlayerBoard()
        {
            return CurrentPlayer == Actor.PlayerOne ? Player2Board : Player1Board;
        }

        public string GetCurrentPlayerName()
        {
            switch (CurrentPlayer)
            {
                case Actor.PlayerOne:
                    return Player1Name;
                case Actor.PlayerTwo:
                    return Player2Name;

                default:
                    throw new ArgumentOutOfRangeException($"Player {CurrentPlayer} is unknown.\r\n");
            }
        }
    }
}