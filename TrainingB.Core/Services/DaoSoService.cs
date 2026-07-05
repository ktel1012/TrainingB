using System.Collections.Generic;
using System.Linq;

namespace TrainingB.Core.Services
{
    /// <summary>
    /// Service for generating number permutations (Dao So)
    /// </summary>
    public static class DaoSoService
    {
        /// <summary>
        /// Generate 2-digit permutations from 2-digit input
        /// Example: "12" -> ["12", "21"]
        /// </summary>
        public static List<string> Dao2Trong2(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 2)
                return new List<string>();

            return GeneratePermutations(input, 2);
        }

        /// <summary>
        /// Generate 2-digit permutations from 3-digit input
        /// Example: "123" -> ["12", "13", "21", "23", "31", "32"]
        /// </summary>
        public static List<string> Dao2Trong3(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 3)
                return new List<string>();

            return GeneratePermutations(input, 2);
        }

        /// <summary>
        /// Generate 2-digit permutations from 4-digit input
        /// Example: "1234" -> ["12", "13", "14", "21", "23", "24", ...]
        /// </summary>
        public static List<string> Dao2Trong4(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 4)
                return new List<string>();

            return GeneratePermutations(input, 2);
        }

        /// <summary>
        /// Generate 3-digit permutations from 3-digit input
        /// Example: "123" -> ["123", "132", "213", "231", "312", "321"]
        /// </summary>
        public static List<string> Dao3Trong3(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 3)
                return new List<string>();

            return GeneratePermutations(input, 3);
        }

        /// <summary>
        /// Generate 3-digit permutations from 4-digit input
        /// Example: "1234" -> ["123", "124", "132", "134", ...]
        /// </summary>
        public static List<string> Dao3Trong4(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 4)
                return new List<string>();

            return GeneratePermutations(input, 3);
        }

        /// <summary>
        /// Generate 4-digit permutations from 4-digit input
        /// Example: "1234" -> all 24 permutations
        /// </summary>
        public static List<string> Dao4Trong4(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 4)
                return new List<string>();

            return GeneratePermutations(input, 4);
        }

        /// <summary>
        /// Core permutation generator
        /// </summary>
        private static List<string> GeneratePermutations(string source, int length)
        {
            var results = new HashSet<string>();
            GeneratePermutationsRecursive(source.ToCharArray(), length, "", results, new HashSet<int>());
            return results.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Recursive permutation generator with position tracking
        /// </summary>
        private static void GeneratePermutationsRecursive(
            char[] source, 
            int targetLength, 
            string current, 
            HashSet<string> results,
            HashSet<int> usedPositions)
        {
            if (current.Length == targetLength)
            {
                results.Add(current);
                return;
            }

            for (int i = 0; i < source.Length; i++)
            {
                if (usedPositions.Contains(i))
                    continue;

                usedPositions.Add(i);
                GeneratePermutationsRecursive(source, targetLength, current + source[i], results, usedPositions);
                usedPositions.Remove(i);
            }
        }

        /// <summary>
        /// Validate input number
        /// </summary>
        public static bool ValidateInput(string input, int expectedLength)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (input.Length != expectedLength)
                return false;

            // Check if all characters are digits
            return input.All(char.IsDigit);
        }
    }
}
