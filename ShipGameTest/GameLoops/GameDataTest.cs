using NUnit.Framework;
using Ships.GameLoop;
using ShipsGame.Core;

namespace ShipGameTest.GameLoops
{
    [TestFixture()]
    public class GameDataTest
    {
        
        [TestCase(Actor.PlayerTwo, Actor.PlayerOne)]
        [TestCase(Actor.PlayerOne, Actor.PlayerTwo)]
        public void SetNextTurn_changesplayer(Actor actor, Actor expectedActor)
        {
            var gameData = new GameData();
            gameData.CurrentPlayer = actor;
            gameData.SetNextPlayerTurn();
            Assert.That(gameData.CurrentPlayer, Is.EqualTo(expectedActor));
        }

        [TestCase(Actor.PlayerTwo, "P2Name")]
        [TestCase(Actor.PlayerOne, "P1Name")]
        public void GetCurrentPlayerName_changesplayer(Actor actor, string expectedName)
        {
            var gameData = new GameData();
            gameData.CurrentPlayer = actor;
            gameData.Player1Name = "P1Name";
            gameData.Player2Name = "P2Name";
            Assert.That(gameData.GetCurrentPlayerName(), Is.EqualTo(expectedName));
        }


        [TestCase(Actor.PlayerTwo, Actor.PlayerOne)]
        [TestCase(Actor.PlayerOne, Actor.PlayerTwo)]
        public void GetCurrentPlayerName_changesplayer(Actor actor, Actor inActiveActor)
        {
            var gameData = new GameData();
            gameData.CurrentPlayer = actor;
            gameData.Player1Name = "P1Name";
            gameData.Player2Name = "P2Name";
            gameData.Player1Board = new Board(Actor.PlayerOne);
            gameData.Player2Board = new Board(Actor.PlayerTwo);
            Assert.That(gameData.GetCurrentPlayerBoard().Owner, Is.EqualTo(actor));
            Assert.That(gameData.GetInActivePlayerBoard().Owner, Is.EqualTo(inActiveActor));
        }
    }
}