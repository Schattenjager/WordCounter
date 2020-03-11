using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace WordCounter
{
    class Program
    {
        /// <summary>
        /// Main method used as input source as per scope of the console application exercise.
        /// In the case of a cloud based service this application could be converted into a class used in a standard web API. 
        /// A controller would receive a basic JSON or XML object in the body of the POST or as query strings in a GET request with the values hard coded here. 
        /// These values could then be passed to the PrintWordCountInFile method and the response returned in a standard JSON or XML object.
        /// In order to make it cloud based such a service could be hosted in IIS anywhere or on a cloud platform such as Azure or AWS.
        /// </summary>
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            TextWriter textWriter = System.Console.Out;
            string fileName = "..//WarAndPeace.txt";
            int wordLenth = 6;
            int listCount = 50;
            PrintWordCountsInFile(fileName, textWriter, wordLenth, listCount);
            watch.Stop();
            PrintExecutionTime(watch, textWriter);
            Console.ReadLine();
        }

        /// <summary>
        /// Convenience method which prints the number of occurrences of each word in the given file
        /// Method to make code more generic. Can be packaged to take input from anywhere and only print words of specified length.
        /// </summary>
        public static void PrintWordCountsInFile(string fileName, TextWriter textWriter, int wordLength, int listCount)
        {
            var text = System.IO.File.ReadAllText(fileName);
            var words = SplitWords(text, wordLength);
            var counts = CountWordOccurrences(words);
            WriteWordCounts(counts, textWriter, listCount);
        }

        /// <summary>
        /// Splits the given text into individual words, stripping punctuation
        /// A word is defined by the regex @"[^\p{L}] which will also allow for special characters in the text"
        /// </summary>
        public static IEnumerable<string> SplitWords(string text, int wordLength = 0)
        {
            Regex nonLetters = new Regex(@"[^\p{L}]");
            return nonLetters.Split(text).Where(s => s.Length > wordLength);
        }

        /// <summary>
        /// Counts the number of occurrences of each word in the given enumerable
        /// </summary>
        public static IDictionary<string, int> CountWordOccurrences(IEnumerable<string> words)
        {
            return CountOccurrences(words, StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Prints word-counts to the given TextWriter
        /// TextWriter parameter means that code is more generic as it will write to any given destination.
        /// </summary>
        public static void WriteWordCounts(IDictionary<string, int> counts, TextWriter writer, int listCount)
        {
            writer.WriteLine($"The number of counts for top {listCount} words:");
            foreach (KeyValuePair<string, int> kvp in counts.OrderByDescending(key => key.Value).Take(listCount))
            {
                writer.WriteLine(kvp.Key.ToLower() + " " + kvp.Value); // print word in lower-case for consistency
            }
        }

        /// <summary>
        /// Counts the number of occurrences of each distinct item
        /// Use of interfaces and generic types makes code more generic allowing it to handle various kinds of collections (Enumerables) as needed.
        /// </summary>
        public static IDictionary<T, int> CountOccurrences<T>(IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            var counts = new Dictionary<T, int>(comparer);

            foreach (T t in items) //for loop might be faster than foreach here but would be less elegant.
            {
                int count;
                if (!counts.TryGetValue(t, out count))
                {
                    count = 0;
                }
                counts[t] = count + 1;
            }

            return counts;
        }

        /// <summary>
        /// Method to print execution time using given textwriter to make solution more generic.
        /// </summary>
        private static void PrintExecutionTime(Stopwatch watch, TextWriter textWriter)
        {
            textWriter.WriteLine();
            textWriter.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}