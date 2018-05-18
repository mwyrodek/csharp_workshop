﻿using System;
 using System.Text;
 using System.Web;
 using System.Web.UI.WebControls;
 using Ships.UI;
 using ShipsGame.Core;

namespace Ships.GameLoop
{
    public class GameLoop
    {
        private GameState gameState = GameState.Setup;
        private string player1Name;
        private string player2Name;
        private Board player1Board;
        private Board player2Board;
        private Actor CurrentPlayer;
        
        public string Act(string readLine)
        {
            var message = new StringBuilder();
            if (IsBeginingOfGame(readLine))
            {
                return "Welcome to ships game please type first player name.\r\n";
            }

            if(IsNoInput(readLine))
            {
                message.Append("You need to type something.\r\n");
            }

            if (IsGaveUp(readLine))
            {
                SetNextPlayerTurn();
                gameState = GameState.FinnishUp;
            }

            
            switch (gameState)
            {
                    case GameState.Setup:
                        message.Append(Setup(readLine));
                        break;
                    case GameState.ShipPlacing:
                        message.Append(ShipPlacing(readLine.ToUpper()));
                        break;
                    case GameState.FinnishUp:
                        message.Append(GameWonState());
                        break;
                    case GameState.InProgress:
                        message.Append(TakeFireComand(readLine.ToUpper()));
                        break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (gameState)
            {
                case GameState.InProgress:
                    message.Append(BuildRepresentationCurrentPlayer());
                    break;
                case GameState.ShipPlacing:
                    message.Append(DisplayeShipsMap());
                    break;
            }
            message.Append("Reminder after Naming Players you can gave up by pressing <b>Q<b>");
            
            return message.ToString();
        }

        private bool IsGaveUp(string readLine)
        {
            return (readLine=="q"|| readLine =="Q")&&(gameState!=GameState.Setup);
        }

        private bool IsNoInput(string readLine)
        {
            return string.IsNullOrEmpty(readLine);
        }

        private bool IsBeginingOfGame(string readLine)
        {
            return string.IsNullOrEmpty(readLine) && string.IsNullOrEmpty(player1Name); 
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
            var formatTableString = $"<b>{command}</b>";

            if (string.IsNullOrEmpty(player1Name))
            {
                player1Name = formatTableString;
                player1Board = new Board(Actor.PlayerOne);
                return $"Player One name is {player1Name} \r\n" +
                       $"Enter Player Two name:\r\n";
            }
            
            if (string.IsNullOrEmpty(player2Name))
            {
                player2Name = formatTableString;
                player2Board = new Board(Actor.PlayerTwo);
                
                CurrentPlayer = Actor.PlayerOne;
                return $"Player two name is {player2Name} \r\n Send R for Random Ship Placing. Send anything else to enable manual setup";
            }

            if (command == "R" || command == "r")
            {
                gameState = GameState.RandomShips;
                var nextStepMessage = RandomShipPlacing(string.Empty);
                return $"RandomSetupEnabled \r\n{nextStepMessage}";
            }
            else
            {
                gameState = GameState.ShipPlacing;
                var nextStepMessage = ShipPlacing(string.Empty);
                return $"Manual setup enabled \r\n {nextStepMessage}";
            }
        }

        private string RandomShipPlacing(string empty)
        {
            player2Board.PlaceAllShipsAtRandom();
            player1Board.PlaceAllShipsAtRandom();
            gameState = GameState.InProgress;
            return TakeFireComand(string.Empty);
        }

        private string ShipPlacing(string command)
        {
            StringBuilder message = new StringBuilder();
            if (!string.IsNullOrEmpty(command))
            {
                if (!ValidateShipPlacementCommand(command))
                {
                    message.Append($"\"{command}\" is not valid input\r\n");
                }
                else
                {
                    var shipsMessage = PlaceShips(command);
                    message.Append(shipsMessage);
                }
            }

            if (IsAllShipPlaced())
            {
                gameState = GameState.InProgress;
                return TakeFireComand(string.Empty);

            }

            message.Append($"Enter {GetCurrentPlayerName()} Ship and its place:\r\n");
            message.Append(ShipPlacementHint());
            return message.ToString();
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

        private bool ValidateShipPlacementCommand(string command)
        {
            if (command.Length < 4)
            {
                return false;
            }
            return InputTranslatorHelper.IsShip(command[0]) &&
                   (InputTranslatorHelper.IsDirection(command[1]) && 
                    CellID.IsIdValid(command.Substring(2)));
        }

        private string ShipPlacementHint()
        {
            return $"To place ship sent ship ShipSymbol, Direction, and Starting Field \r\n  example BVA9\r\n" +
                   $"Ship Types and size \r\n" +
                   $"<b>B</b> - {ShipTypes.Battleship} {Settings.GetShipSize(ShipTypes.Battleship)}\r\n" +
                   $"<b>K</b> - {ShipTypes.Carrier} {Settings.GetShipSize(ShipTypes.Carrier)}\r\n" +
                   $"<b>C</b> - {ShipTypes.Crusier} {Settings.GetShipSize(ShipTypes.Crusier)}\r\n" +
                   $"<b>D</b> - {ShipTypes.Destroyer} {Settings.GetShipSize(ShipTypes.Destroyer)}\r\n" +
                   $"<b>S</b> - {ShipTypes.Submarine} {Settings.GetShipSize(ShipTypes.Submarine)}\r\n"+
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

        private string BuildRepresentationCurrentPlayer()
        {
            StringBuilder ships = new StringBuilder();

            ships.Append(DisplayeShipsMap());
            ships.Append($"<b>Target Board</b>: \t\n");
            ships.Append(DisplayTargetMap(GetInActivePlayerBoard()));
            return ships.ToString();
        }

        private string DisplayeShipsMap()
        {
            StringBuilder ships = new StringBuilder();
            ships.Append("<b>Your Board</b>: \t\n");
            ships.Append(DisplayShipsMap(GetCurrentPlayerBoard()));
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