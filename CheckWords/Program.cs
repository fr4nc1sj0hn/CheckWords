using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace CheckWords
{
    class Program
    {
        static int _numofwords;
        static string _targetword;
        static void Main(string[] args)
        {
            CommandLineApplication commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);

            commandLineApplication.Command("generate", (command) =>
            {
                command.Description = "This is more or less a cheat\n";
                command.ExtendedHelpText = "Usage: dotnet CheckWords.dll generate [listofletters] [length]";
                command.HelpOption("-?|-h|--help");

                var letterlist = command.Argument("[ListOfLetters]",
                                   "List of Letters.");
                var wordlength = command.Argument("[wordlength]",
                                   "Number of letters of the words to generate.");

                command.OnExecute(() =>
                {
                    _targetword = letterlist.Value;
                    Int32.TryParse(wordlength.Value, out _numofwords);
                    Console.WriteLine("Generating...");

                    GenerateWords();
                    
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();

                    return 0;
                });
            });
            commandLineApplication.Execute(args);
        }
        public static void GenerateWords()
        {
            var wordlist = GetWordsFromJSON();
            var correctwords = new List<string>();

            foreach (var word in wordlist)
            {
                if (ProcessWord(word) != "")
                {
                    correctwords.Add(word);
                }
            }
            Console.WriteLine("Allowed Words...");
            Console.WriteLine(".........................................");
            foreach (var word in correctwords)
            {
                Console.WriteLine(word);
            }

        }
        public static List<string> GetWordsFromJSON()
        {
            using (StreamReader r = new StreamReader("Data/words_dictionary.json"))
            {
                
                string json = r.ReadToEnd();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
                var words = dict.Where(kvp => kvp.Key.Length == _numofwords).Select(x => x.Key).ToList();
                return words;
            }
        }
        public static string ProcessWord(string word)
        {
            List<char> chars = new List<char>();
            List<char> allowedchars = new List<char>();
            chars.AddRange(word);
            allowedchars.AddRange(_targetword);

            foreach(var c in chars)
            {
                if(!allowedchars.Any(x => x == c)){
                    return "";
                }
                else
                {
                    allowedchars.Remove(c);
                }
            }
            return word;
            //if resultlist.SingleOrDefault(r => r.Id == 2);

        }
    }
}
