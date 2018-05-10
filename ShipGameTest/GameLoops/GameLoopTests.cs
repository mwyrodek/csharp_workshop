using System;
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
        
        [Test, Order(1)]
        public void FreshSetup_AskForPlayerName()
        {
            Assert.That(game.Act(string.Empty), Is.EqualTo("Welcome to ships game please type first player name"));
        }

        [Test, Order(2)]
        public void SetFirstPlayerName_AskForSecondPlayerName()
        {
            player1name = "Arek";
            var act = game.Act(player1name);
            Assert.That(act, Contains.Substring(player1name));
            Assert.That(act, Contains.Substring("Enter Player Two name"));
        }

        [Test, Order(3)]
        public void SetSecondPlayerName_AskForPlayer1ShipName()
        {
            player2name= "Czarek";
            var act = game.Act(player2name);
            Assert.That(act, Contains.Substring(player2name));
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }

        [Test, Order(4)]
        public void Player1PlaceShip_PutNotRealData()
        {
            var act = game.Act(player2name);
            Assert.That(act, Contains.Substring($"\"{player2name.ToUpper()}\" is not valid input"));
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }

        [Test, Order(5)]
        public void Player1PlaceShip_PutItInvalid()
        {
            var shipPlace = "KVA1";
            var act = game.Act(shipPlace);
            Assert.That(act, Contains.Substring("Ship can't be placed on A1"));
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }

        [Test, Order(6)]
        public void Player1PlaceShip_PutItCorrect()
        {
            var shipPlace = "KVA9";
            var act = game.Act(shipPlace);
            Assert.That(act, Contains.Substring($"Enter {player2name} Ship"));
        }

        [Test, Order(7)]
        public void Player2PlaceShip_PutItCorrect()
        {
            var shipPlace = "KVA9";
            var act = game.Act(shipPlace);
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }

        [Test, Order(8)]
        public void Player1PlaceShip_PlaceAlreadyTaken()
        {
            var shipPlace = "CVA9";
            var act = game.Act(shipPlace);
            Assert.That(act, Contains.Substring("Ship can\'t be placed on A9, A8, A7 becase A9 ,A8 ,A7 belongs to diffrent ship"));
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }

        [Test, Order(9)]
        public void Player1PlaceShip_ShipAlreadyUsed()
        {
            var shipPlace = "KVA9";
            var act = game.Act(shipPlace);
            Assert.That(act, Contains.Substring("All shipes of type: Carrier were already placed"));
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }

        [Test, Order(10)]
        public void Player1PlaceSecondShip_Correct()
        {
            var shipPlace = "KVA9";
            var act = game.Act(shipPlace);
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