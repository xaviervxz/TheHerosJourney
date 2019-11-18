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
            // ----------------
            // DATA SET UP AND INTRODUCTION
            // ----------------

            // LOAD DATA FROM FILES AND CREATE EMPTY STORY
            var fileData = LoadFromFile.Data();
            var story = new Story();

            // PICK PLAYER'S NAME
            story.You = Pick.Character(Relationship.Self, story.Characters, fileData.PeopleNames);

            // DISPLAY INTRODUCTION
            WriteDashes();
            WriteMessage("Welcome to the Neverending Story!");
            WriteMessage("Type your name and press Enter.");
            story.You.Name = ReadInput();
            WriteDashes();

            WriteMessage("Male or female? (M/F)");
            string playerSex = ReadInput();
            if (playerSex == "M")
            {
                story.You.Sex = Sex.Male;
            }
            else
            {
                story.You.Sex = Sex.Female;
            }
            WriteDashes();

            // ASSIGN INSTINCT
            WriteMessage(story.You.Name + @", which one best describes what you want to do?
1) " + Instinct.ToAvoidNotice + @"
2) " + Instinct.ToReclaimWhatWasTaken);
            string instinct = ReadInput();
            switch (instinct)
            {
                case "1":
                default:
                    instinct = Instinct.ToAvoidNotice;
                    break;
                case "2":
                    instinct = Instinct.ToReclaimWhatWasTaken;
                    break;
            }
            WriteDashes();
            WriteMessage("Hello, " + story.You.Name + ", you want " + instinct.ToLower() + ".");
            WriteDashes();

            // ----------------
            // MAIN GAME LOOP
            // ----------------

            Scene currentScene = null;
            bool getNewScene = true;
            bool shownHelp = false;

            bool gameRunning = true;
            while (gameRunning)
            {
                // IF WE WERE JUST CHECKING INVENTORY OR SOMETHING,
                // DON'T PICK A NEW SCENE.
                if (getNewScene)
                {
                    // OTHERWISE, PICK A NEW SCENE.
                    currentScene = Pick.NextScene(fileData.Scenes, story);

                    // IF NO NEW SCENE IS FOUND, THE STORY IS OVER.
                    // WRITE OUT THE STORY TO A FILE AND SHOW "THE END"
                    if (currentScene == null)
                    {
                        WriteDashes();
                        WriteMessage("THE END");
                        WriteDashes();
                        WriteMessage("");
                        WriteMessage("(Press Enter to exit.)");

                        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "story_log.txt"), string.Join(Environment.NewLine, MessageLog));

                        ReadInput();

                        gameRunning = false;
                        continue;
                    }
                }
                else
                {
                    getNewScene = true;
                }

                // PROCESS AND DISPLAY MAIN MESSAGE
                string message = Process.Message(currentScene.Message, story, fileData.PeopleNames);
                WriteMessage(message);

                // IF THERE ARE NO CHOICES AVAILABLE IN THIS SCENE,
                // SKIP TO THE NEXT SCENE.
                if (string.IsNullOrWhiteSpace(currentScene.Choice1) || string.IsNullOrWhiteSpace(currentScene.Choice2))
                {
                    WriteMessage("");
                    continue;
                }

                // IF THERE ARE CHOICES AVAILABLE IN THIS SCENE,
                // PROCESS AND DISPLAY THEM.
                WriteDashes();
                string choice1 = Process.Message(currentScene.Choice1, story, fileData.PeopleNames);
                WriteMessage("1) " + choice1);
                string choice2 = Process.Message(currentScene.Choice2, story, fileData.PeopleNames);
                WriteMessage("2) " + choice2);

                if (!shownHelp)
                {
                    shownHelp = true;
                    WriteMessage("(Type \"help\" and hit Enter to learn how to play.)");
                }

                // ALLOW THE PLAYER TO MAKE A CHOICE
                string input = ReadInput();
                WriteDashes();

                // PROCESS THE PLAYER'S CHOICE
                if (input == "exit")
                {
                    gameRunning = false;

                    getNewScene = false;
                }
                else if (input == "help" || input == "?")
                {
                    WriteMessage(@"help or ? - show this help dialog
exit - exit the program
inventory or i - view your inventory (your collected items)
1 or 2 - choose an action");
                    WriteDashes();

                    getNewScene = false;
                }
                else if (input == "inventory" || input == "i")
                {
                    string inventoryMessage = Process.InventoryOf(story.You);

                    WriteMessage("You're carrying:");
                    WriteMessage(inventoryMessage);
                    WriteDashes();

                    getNewScene = false;
                }
                else if (input == "1")
                {
                    string rawOutro = currentScene.Outro1;
                    string outro = Process.Message(rawOutro, story, fileData.PeopleNames);

                    WriteMessage(outro);
                    WriteMessage("");

                    getNewScene = true;
                }
                else if (input == "2")
                {
                    string rawOutro = currentScene.Outro2;
                    string outro = Process.Message(rawOutro, story, fileData.PeopleNames);

                    WriteMessage(outro);
                    WriteMessage("");

                    getNewScene = true;
                }
                else
                {
                    WriteMessage("(Please enter \"1\" or \"2\" to choose an action.)");

                    getNewScene = false;
                }

                currentScene.Done = true;
            }
        }
    }
}
