using System;
using System.Collections.Generic;
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
            player1name = "P1";
            var act = game.Act(player1name);
            Assert.That(act, Contains.Substring(player1name));
            Assert.That(act, Contains.Substring("Enter Player Two name"));
        }

        [Test, Order(3)]
        public void SetSecondPlayerName_AskForPlayer1ShipName()
        {
            player2name= "P2";
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
            Assert.That(act, Contains.Substring("All ships of type: Carrier were already placed"));
            Assert.That(act, Contains.Substring($"Enter {player1name} Ship"));
        }

        [Test, Order(10)]
        public void Player1PlaceSecondShip_Correct()
        {
            var shipPlace = "BVB9";
            var act = game.Act(shipPlace);
            Assert.That(act, Contains.Substring($"Enter {player2name} Ship"));
        }
        
        [Test, Order(11)]
        public void PlacedALlShip_Correct()
                {
                    List<string> places = new List<string>
                    {
                        "BVB9",
                        "CVC9",
                        "CVC9",
                        "DVD9",
                        "DVD9",
                        "SVE9",
                        "SVE9"
                    };
                    var act = string.Empty;
                    foreach (var ship in places)
                    {
                        act = game.Act(ship);
                    }
        
                    Assert.That(act, Contains.Substring($"Player {player1name} Turn: provide your target"));
                }
                
        //player 1 shot miss
        [Test, Order(12)]
        public void FireAtFirstShip_Miss()
        {
            var Action = game.Act("A1");
            Assert.That(Action, Contains.Substring("Shot missed"));
            Assert.That(Action, Contains.Substring($"Player {player2name} Turn: provide your target"));
        }
        //player 2 shot miss
        [Test, Order(13)]
        public void FireAtFirstShip_MissReapet()
        {
            game.Act("A1");
            var Action = game.Act("A1");
            Assert.That(Action, Contains.Substring("A1 was already shot at"));
            Assert.That(Action, Contains.Substring($"Player {player1name} Turn: provide your target"));
        }
        
        [Test, Order(14)]
        public void FireAtFirstShip_Hit()
        {
            game.Act("D9");
            var Action = game.Act("D9");
            Assert.That(Action, Contains.Substring("Enemy Destroyer was Hit!"));
            Assert.That(Action, Contains.Substring($"Player {player1name} Turn: provide your target"));
        }

        [Test, Order(15)]
        public void FireAtFirstShip_Sunk()
        {
            game.Act("D8");
            var Action = game.Act("D8");
            Assert.That(Action, Contains.Substring("Enemy Destroyer was Sunk!"));
            Assert.That(Action, Contains.Substring($"Player {player1name} Turn: provide your target"));
        }
        
        [Test, Order(16)]
        public void GameWon()
        {
            var Action = ContiuneGame();
            Assert.That(Action, Contains.Substring("Congratulation Player P2 Won"));
            Assert.That(Action, Contains.Substring("Starting new game."));
            Assert.That(Action, Contains.Substring("Please type first player name"));
        }

        private string ContiuneGame()
        {
            List<char> width = new List<char>
            {
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'I',
                'J',
                'K'
            };
            for (int i = 1; i < 10; i++)
            {
                foreach (var positon in width)
                {
                    var Action = game.Act($"{positon}{i}");
                    if (Action.Contains("Won")) 
                    {
                        return Action;
                    }
                    Action = game.Act($"{positon}{i}");
                    if (Action.Contains("Won"))
                    {
                        return Action;
                    }
                }
            }

            return null;
        }
        // rest game
        
        //end game
        //new game and quit

    }
}