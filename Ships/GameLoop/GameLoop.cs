﻿using System;
 using System.Linq;
 using System.Text;
 using Ships.Action;
 using Ships.UI;
 using ShipsGame.Core;

namespace Ships.GameLoop
{
    public class GameLoop
    {
        private GameState gameState;
        private string player1Name;
        private string player2Name;
        private Board player1Board;
        private Board player2Board;
        private Actor CurrentPlayer;
        
        // start game
        // select ai 
        // place ships
        // ai places 
        // shot 
        // ai shots
        // check board state
        // ask for new imput
        
        
        //Q - kill game
        //
        
        //actions
        //state - menu /setup phase/ game 
        
        //menu 
        //    new game
        //    fast setup - streach goal
        //


        public GameLoop()
        {
            gameState = GameState.Setup;
        }

        public string Act(string readLine)
        {
            if (string.IsNullOrEmpty(readLine) && string.IsNullOrEmpty(player1Name))
            {
                return "Welcome to ships game please type first player name";
            }
            else if(string.IsNullOrEmpty(readLine) && string.IsNullOrEmpty(player1Name))
            {
                return "You need to type something";
            }

            
            switch (gameState)
            {
                    case GameState.Setup:
                        return Setup(readLine);
                    case GameState.ShipPlacing:
                        return ShipPlacing(readLine.ToUpper());
                    case GameState.FinnishUp:
                        break;
                    case GameState.InProgress:
                        return TakeFireComand(readLine.ToUpper());
                        break;
            }
            throw new System.NotImplementedException();
        }

        private string TakeFireComand(string command)
        {
            StringBuilder messege = new StringBuilder();
            if (CellID.IsIdValid(command))
            {
                var cellId = new CellID(command);
                var actionResult = GetCurrentPlayerBoard().FireMissle(cellId);
                
                if (!actionResult.AllowRepeat)
                {
                    SetNextPlayerTurn();
                }

                if (IsAllEnemyShipSunk())
                {
                    gameState = GameState.FinnishUp;
                    return GameWonState();

                }
                messege.Append(actionResult.Messege);
                messege.AppendLine();
            }
            else
            {
                messege.Append($"{command} is not valid command");
            }
            messege.Append($"Player {GetCurrentPlayerName()} Turn: provide your target");
            return messege.ToString();
        }

        private string GameWonState()
        {
            throw new NotImplementedException();
        }

        private string Setup(string command)
        {
            if (string.IsNullOrEmpty(player1Name))
            {
                player1Name = command;
                player1Board = new Board(Actor.PlayerOne);
                return $"Player One name is {player1Name} \n" +
                       $"Enter Player Two name";
            }
            
            if (string.IsNullOrEmpty(player2Name))
            {
                player2Name = command;
                player2Board = new Board(Actor.PlayerTwo);
                gameState = GameState.ShipPlacing;
                CurrentPlayer = Actor.PlayerOne;
                var nextStepMessage = ShipPlacing(string.Empty);
                return $"Player two name is {player2Name} \n {nextStepMessage}";
            } 
            
            throw new ArgumentException("You shouldn't be here");
        }

        private string ShipPlacing(string command)
        {
            StringBuilder messege = new StringBuilder();
            if (!string.IsNullOrEmpty(command))
            {
                if (!ValideteShipPlacementCommand(command))
                {
                    messege.Append($"\"{command}\" is not valid input\n");
                }
                else
                {
                    var shipsMessage = PlaceShips(command);
                    messege.Append(shipsMessage);
                }
            }

            if (IsAllShipPlaced())
            {
                gameState = GameState.InProgress;
                return TakeFireComand(string.Empty);

            }
            else
            {
                messege.Append($"Enter {GetCurrentPlayerName()} Ship and its place:");
                messege.Append(ShipPlacementHint());
                return messege.ToString();
            }
        }

        private bool IsAllShipPlaced()
        {
            return player1Board.IsAllPlaced() && player2Board.IsAllPlaced();
        }
        
        private bool IsAllEnemyShipSunk()
        {
            
            //todo candidate for bug
            return GetCurrentPlayerBoard().IsAllShipSunk();
        }

        private string PlaceShips(string command)
        {
            var board = GetCurrentPlayerBoard();
            var actionResult = board.PlaceShip(InputTranslatorHelper.TranlateCommand(command));
            if (!actionResult.AllowRepeat)
            {
                SetNextPlayerTurn();
            }

            return actionResult.Messege;

        }

        private void SetNextPlayerTurn()
        {
            if (CurrentPlayer == Actor.PlayerOne)
            {
                CurrentPlayer = Actor.PlayerTwo;
                    return;
            }

            CurrentPlayer = Actor.PlayerOne;
        }

        private Board GetCurrentPlayerBoard()
        {
            if (CurrentPlayer == Actor.PlayerOne)
            {
                return player1Board;
            }

            return player2Board;
        }

        private bool ValideteShipPlacementCommand(string command)
        {
            if(!InputTranslatorHelper.IsShip(command[0]))
            {
                return false;
            }
            if(!InputTranslatorHelper.IsDirection(command[1]))
            {
                return false;
            } 
            if (!CellID.IsIdValid(command.Substring(2)))
            {
                return false;
            }
            return true;
        }
        private string ShipPlacementHint()
        {
            return $"To place ship sent ship ShipSymbol, Direction, and Starting Field  example BVA9\n" +
                   $"Directions V - vertical UP => Down   H - Horizontal Left => Right\n" +
                   $"Ship Types and size" +
                   $"B - Battleship {Settings.GetShipSize(ShipTypes.Battleship)}\n" +
                   $"K - Battleship {Settings.GetShipSize(ShipTypes.Carrier)}\n" +
                   $"C - Battleship {Settings.GetShipSize(ShipTypes.Crusier)}\n" +
                   $"D - Battleship {Settings.GetShipSize(ShipTypes.Destroyer)}\n" +
                   $"S - Battleship {Settings.GetShipSize(ShipTypes.Submarine)}\n";
        }

        private string GetCurrentPlayerName()
        {
            switch (CurrentPlayer)
            {
                    case Actor.PlayerOne:
                        return player1Name;
                    case Actor.PlayerTwo:
                        return player2Name;
                    
                    default:
                        throw new ArgumentOutOfRangeException($"player {CurrentPlayer} is unknown");
            }
        }
    }
}