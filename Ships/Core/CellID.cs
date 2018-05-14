using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ShipsGame.Core
{
    public class CellID
    {
        public string Id { get; }
        private int height;
        private char width;

        public CellID(string id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Input was null.");
            }

            id = id.ToUpper();
            if (!CellID.IsIdValid(id))
            {
                //throw new ArgumentOutOfRangeException($"{id.ToString()} is not valid ID");
                id = "B3";
            }

            this.Id = id;
            var splitIdToElements = SplitIdToElements(id);
            width = splitIdToElements.Item1;
            height = splitIdToElements.Item2;
        }

        public static bool IsIdValid(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            if (!validateLenght(id))
            {
                return false;
            }
            if(!AreValuesInBound(id))
            {
                return false;
            }

            Regex regex = new Regex(@"[A-Z]\d+");
            Match match = regex.Match(id);
            if (match.Success)
            {
                return true;
            }

            return false;
        }

        private static bool AreValuesInBound(string id)
        {
            var splitIdToElements = SplitIdToElements(id);
            int minimumChar = (int)splitIdToElements.Item1 -'A';
            if ((minimumChar > Settings.BoardWidth - 1) || (minimumChar < 0))
            {
                return false;
            }
            
            
            if((splitIdToElements.Item2 > Settings.BoardHeight) || (splitIdToElements.Item2 <= 0))
            {
                return false;
            }

            return true;
        }

        private static bool validateLenght(string id)
        {
            return id.Length < 3 && id.Length >= 2;
        }

        public List<string> GetNeighbourIDs()
        {
            var list = new List<string>();
           
            var buildId = GetIdAbove();
            if (!string.IsNullOrEmpty(buildId))
            {
                list.Add(buildId);
            }

            buildId = GetIdToRight();
            if (!string.IsNullOrEmpty(buildId))
            {
                list.Add(buildId);
            }
            return list;
        }

        public string GetIdBellow()
        {
            var newHeigt = height - 1;
            var buildId = BuildId(width, newHeigt);
            return IsIdValid(buildId) ? buildId : null;
        }
        
        public string GetIdToRight()
        {
            var newWidth = width;
            var buildId = BuildId(++newWidth, height);
            return IsIdValid(buildId) ? buildId : null;
        }

        private string GetIdAbove()
        {
            var newHeight = height + 1;
            var buildId = BuildId(width, newHeight);
            return IsIdValid(buildId) ? buildId : null;
        }
        
        public static CellID CreateFirstCell()
        {
            return new CellID("A1");
        }

        private static Tuple<char, int> SplitIdToElements(string id)
        {
            var charArray = id.ToCharArray();
            var value = int.Parse(id.Substring(1));
            return Tuple.Create(charArray[0], value);
        }

        private string BuildId(char symbol, int value)
        {
            return $"{symbol}{value}";
        }
    }
}