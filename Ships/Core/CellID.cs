using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ShipsGame.Core
{
    public class CellID
    {
        public string Id { get; }

        public CellID(string id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Input was null.");
            }

            id = id.ToUpper();
            if (!this.IsIdValid(id))
            {
                throw new ArgumentOutOfRangeException($"{id.ToString()} is not valid ID");
            };

            this.Id = id;
        }

        private bool IsIdValid(string id)
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

        private bool AreValuesInBound(string id)
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
            var splitIdToElements = SplitIdToElements(Id);
            var list = new List<string>();
            char newSymbol = splitIdToElements.Item1;
            
            int newValue = splitIdToElements.Item2 + 1;
            var buildId = BuildId(splitIdToElements.Item1, newValue);
            if (IsIdValid(buildId))
            {
                list.Add(buildId);
            }

            buildId = BuildId(++newSymbol, splitIdToElements.Item2);
            if (IsIdValid(buildId))
            {
                list.Add(buildId);
            }
            return list;
        }

        public static CellID CreateFirstCell()
        {
            throw new System.NotImplementedException();
        }

        private Tuple<char, int> SplitIdToElements(string id)
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