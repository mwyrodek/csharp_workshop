﻿using System;
 using System.Text;
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
                return "Welcome to ships game please type first player name.\r\n";
            }

            if(string.IsNullOrEmpty(readLine) && string.IsNullOrEmpty(player1Name))
            {
                return "You need to type something.\r\n";
            }


            switch (gameState)
            {
                    case GameState.Setup:
                        return Setup(readLine);
                    case GameState.ShipPlacing:
                        return ShipPlacing(readLine.ToUpper());
                    case GameState.FinnishUp:
                        return GameWonState();
                    case GameState.InProgress:
                        return TakeFireComand(readLine.ToUpper());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string TakeFireComand(string command)
        {
            StringBuilder messege = new StringBuilder();
            if (CellID.IsIdValid(command))
            {
                var cellId = new CellID(command);
                var actionResult = GetCurrentPlayerBoard().FireMissle(cellId);
                


                if (IsAllEnemyShipSunk())
                {
                    gameState = GameState.FinnishUp;
                    return GameWonState();

                }

                if (!actionResult.AllowRepeat)
                {
                    SetNextPlayerTurn();
                }
                messege.Append(actionResult.Messege);
                messege.AppendLine();
            }
            else
            {
                messege.Append($"{command} is not valid command.\r\n");
            }
            messege.Append($"Player {GetCurrentPlayerName()} Turn: provide your target.\r\n");
            return messege.ToString();
        }

        private string GameWonState()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Congratulation Player {GetCurrentPlayerName()} Won!\r\n");
            stringBuilder.Append("Starting new game.\r\n");
            stringBuilder.Append("Please type first player name:\r\n");
            gameState = GameState.Setup;
            CurrentPlayer = Actor.PlayerOne;
            player1Name = string.Empty;
            player2Name = string.Empty;

            return stringBuilder.ToString();
        }

        private string Setup(string command)
        {
            if (string.IsNullOrEmpty(player1Name))
            {
                player1Name = command;
                player1Board = new Board(Actor.PlayerOne);
                return $"Player One name is {player1Name} \r\n" +
                       $"Enter Player Two name:\r\n";
            }
            
            if (string.IsNullOrEmpty(player2Name))
            {
                player2Name = command;
                player2Board = new Board(Actor.PlayerTwo);
                gameState = GameState.ShipPlacing;
                CurrentPlayer = Actor.PlayerOne;
                var nextStepMessage = ShipPlacing(string.Empty);
                return $"Player two name is {player2Name} \r\n {nextStepMessage}\r\n";
            } 
            
            throw new ArgumentException("You shouldn't be here.\r\n");
        }

        private string ShipPlacing(string command)
        {
            StringBuilder messege = new StringBuilder();
            if (!string.IsNullOrEmpty(command))
            {
                if (!ValideteShipPlacementCommand(command))
                {
                    messege.Append($"\"{command}\" is not valid input\r\n");
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
                messege.Append($"Enter {GetCurrentPlayerName()} Ship and its place:\r\n");
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
            
            //todo candidate for bug change it to current player
            return GetInActivePlayerBoard().IsAllShipSunk();
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
            return CurrentPlayer == Actor.PlayerOne ? player1Board : player2Board;
        }

        private Board GetInActivePlayerBoard()
        {
            return CurrentPlayer == Actor.PlayerOne ? player2Board : player1Board;
        }

        private bool ValideteShipPlacementCommand(string command)
        {
            return InputTranslatorHelper.IsShip(command[0]) &&
                   (InputTranslatorHelper.IsDirection(command[1]) && 
                    CellID.IsIdValid(command.Substring(2)));
        }
        private string ShipPlacementHint()
        {
            return $"To place ship sent ship ShipSymbol, Direction, and Starting Field \r\n  example BVA9\r\n" +
                   $"Ship Types and size \r\n" +
                   $"B - {ShipTypes.Battleship} {Settings.GetShipSize(ShipTypes.Battleship)}\r\n" +
                   $"K - {ShipTypes.Carrier} {Settings.GetShipSize(ShipTypes.Carrier)}\r\n" +
                   $"C - {ShipTypes.Crusier} {Settings.GetShipSize(ShipTypes.Crusier)}\r\n" +
                   $"D - {ShipTypes.Destroyer} {Settings.GetShipSize(ShipTypes.Destroyer)}\r\n" +
                   $"S - {ShipTypes.Submarine} {Settings.GetShipSize(ShipTypes.Submarine)}\r\n"+
                   $"Directions V - vertical UP => Down   H - Horizontal Left => Right\r\n";
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
                        throw new ArgumentOutOfRangeException($"Player {CurrentPlayer} is unknown.\r\n");
            }
        }
    }
}