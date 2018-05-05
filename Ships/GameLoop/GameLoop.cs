﻿using System;
using System.Data;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
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
                        return ShipPlacing(readLine);
                    case GameState.FinnishUp:
                    case GameState.InProgress:
                        break;
            }
            throw new System.NotImplementedException();
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
                return $"Player two name is {player2Name} \n ";
            } 
            
            throw new ArgumentException("You shound be here");
        }

        private string ShipPlacing(string command)
        {
            StringBuilder messege = new StringBuilder();
            if (!string.IsNullOrEmpty(command))
            {
                throw new NotImplementedException();
            }

            messege.Append($"Enter {GetCurrentPlayerName()} ship and place:");
            messege.Append(ShipPlacementHint());
            return messege.ToString();
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