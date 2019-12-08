using NeverendingStory.Data;
using NeverendingStory.Functions;
using System;
using System.Collections.Generic;
using System.IO;

namespace NeverendingStory.Console
{
    internal class Program
    {
        private static readonly List<string> MessageLog = new List<string>();
        
        private static string ReadInput()
        {
            string input = System.Console.ReadLine();

            MessageLog.Add(input);

            return input;
        }

        /// <summary>
        ///     Writes the specified data, followed by the current line terminator, to the standard output stream, while wrapping lines that would otherwise break words.
        /// </summary>
        /// <param name="paragraph">The value to write.</param>
        /// <param name="tabSize">The value that indicates the column width of tab characters.</param>
        public static void WriteMessage(string paragraph)
        {
            MessageLog.Add(paragraph);

            const int tabSize = 8;
            string[] lines = paragraph
                .Replace("\t", new string(' ', tabSize))
                .Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string process = lines[i];
                List<string> wrapped = new List<string>();

                while (process.Length > System.Console.WindowWidth)
                {
                    int wrapAt = process.LastIndexOf(' ', Math.Min(System.Console.WindowWidth - 1, process.Length));
                    if (wrapAt <= 0)
                    {
                        break;
                    }

                    wrapped.Add(process.Substring(0, wrapAt));
                    process = process.Remove(0, wrapAt + 1);
                }

                foreach (string wrap in wrapped)
                {
                    if (wrap == "")
                    {
                        System.Console.ReadLine();
                    }
                    else
                    {
                        System.Console.WriteLine(wrap);
                    }
                }

                if (process == "")
                {
                    System.Console.ReadLine();
                }
                else
                {
                    System.Console.WriteLine(process);
                }
            }
        }

        public static void WriteDashes()
        {
            const string dashes = "--------";
            WriteMessage(dashes);
        }

        private static void Main(string[] args)
        {
            // RUN THE GAME.

            Run.Game(ReadInput, WriteMessage, WriteDashes);


            // WRITE THE STORY'S LOG TO A FILE.

            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "story_log.txt"), string.Join(Environment.NewLine, MessageLog));
        }
    }
}
