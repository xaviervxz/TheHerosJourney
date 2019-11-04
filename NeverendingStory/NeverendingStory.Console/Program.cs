using NeverendingStory.Data;
using NeverendingStory.Functions;
using System;
using System.Collections.Generic;
using System.IO;

namespace NeverendingStory.Console
{
    internal class Program
    {
        private static string ReadInput()
        {
            return System.Console.ReadLine();
        }

        private static List<string> MessageLog = new List<string>();

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
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string process = lines[i];
                List<string> wrapped = new List<string>();

                while (process.Length > System.Console.WindowWidth)
                {
                    int wrapAt = process.LastIndexOf(' ', Math.Min(System.Console.WindowWidth - 1, process.Length));
                    if (wrapAt <= 0) break;

                    wrapped.Add(process.Substring(0, wrapAt));
                    process = process.Remove(0, wrapAt + 1);
                }

                foreach (string wrap in wrapped)
                {
                    System.Console.WriteLine(wrap);
                }

                System.Console.WriteLine(process);
            }
        }

        private static void Main(string[] args)
        {
            // Load names and scenes from files.
            var fileData = LoadFromFile.Data(names: "Names", scenes: "Scenes");

            // Create a story.
            var story = new Story();

            // Pick the player's name.
            story.You = Pick.Character(Relationship.Self, story.Characters, fileData.Names);

            const string dashes = "--------";

            WriteMessage(dashes);
            WriteMessage("Welcome to the Neverending Story, " + story.You.Name + "!");
            WriteMessage(dashes);

            bool gameRunning = true;
            bool skipScene = false;
            while (gameRunning)
            {
                string input;
                Scene currentScene = null;

                if (skipScene)
                {
                    skipScene = false;

                    input = ReadInput();
                    WriteMessage(dashes);
                }
                else
                {
                    // Find the next scene.
                    currentScene = Pick.NextScene(fileData.Scenes, story);

                    if (currentScene == null)
                    {
                        WriteMessage(dashes);
                        WriteMessage("THE END");
                        WriteMessage(dashes);

                        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "story_log.txt"), string.Join(Environment.NewLine, MessageLog));

                        break;
                    }

                    // Generate and display the scene message.
                    string message = Process.Message(currentScene.Message, story, fileData.Names);
                    WriteMessage(message);
                    WriteMessage(dashes);

                    // Generate and display the scene's options.
                    string choice1 = Process.Message(currentScene.Choice1, story, fileData.Names);
                    WriteMessage("1) " + choice1);
                    string choice2 = Process.Message(currentScene.Choice2, story, fileData.Names);
                    WriteMessage("2) " + choice2);

                    input = ReadInput();
                    WriteMessage(dashes);
                }

                if (input == "exit")
                {
                    gameRunning = false;
                }
                else if (input == "help" || input == "?")
                {
                    WriteMessage(@"help or ? - show this help dialog
exit - exit the program
inventory or i - view your inventory (your collected items)
1 or 2 - choose an action");
                    WriteMessage(dashes);
                }
                else if (input == "inventory" || input == "i")
                {
                    string inventoryMessage = Process.InventoryOf(story.You);

                    WriteMessage("You're carrying:");
                    WriteMessage(inventoryMessage);
                    WriteMessage(dashes);
                }
                else if (input == "1")
                {
                    string rawOutro = currentScene.Outro1;
                    string outro = Process.Message(rawOutro, story, fileData.Names);

                    WriteMessage(outro);
                    WriteMessage("");
                }
                else if (input == "2")
                {
                    string rawOutro = currentScene.Outro2;
                    string outro = Process.Message(rawOutro, story, fileData.Names);

                    WriteMessage(outro);
                    WriteMessage("");
                }
                else
                {
                    WriteMessage("(Please enter \"1\" or \"2\" to choose an action.)");
                }
            }
        }
    }
}
