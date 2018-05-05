using NUnit.Framework;
using Ships.GameLoop;

namespace ShipGameTest.GameLoops
{
    [TestFixture()]
    public class GameLoopTests
    {
        private GameLoop game;
        private string player1name;
        private string player2name;
        
        [OneTimeSetUp]
        public void Setup()
        {
          game = new Ships.GameLoop.GameLoop();
        }
        
        [Test]
        public void FreshSetup_AskForPlayerName()
        {
            Assert.That(game.Act(string.Empty), Is.EqualTo("Welcome to ships game please type first player name"));
        }
        
        [Test]
        public void SetFirstPlayerName_AskForSecondPlayerName()
        {
            player1name = "Arek";
            var act = game.Act(player1name);
            Assert.That(act, Contains.Substring(player1name));
            Assert.That(act, Contains.Substring("Enter Player Two name"));
        }
        
        [Test]
        public void SetSecondPlayerName_AskForSecondPlayerName()
        {
            player2name= "Czarek";
            var act = game.Act(player2name);
            Assert.That(act, Contains.Substring(player2name));
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }
        
        [Test]
        public void Player1PlaceShip_AskForSecondPlayerName()
        {
            player2name= "Czarek";
            var act = game.Act(player2name);
            Assert.That(act, Contains.Substring(player2name));
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }
        
        //player 1 place ship
        //player 2 place ship
        
        //place last ship change state to game
        //player 1 shot
        //player 2 shot
        //player 1 repeat
        //player 2 reapet
        //end game

    }
}