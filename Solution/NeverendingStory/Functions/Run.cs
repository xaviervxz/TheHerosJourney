using NeverendingStory.Data;
using System;
using System.IO;

namespace NeverendingStory.Functions
{
    public static class Run
    {
        public static void Game(Func<string> getPlayerInput, Action<string> addTextToStory, Action addGapToStory)
        {
            // ----------------
            // DATA SET UP AND INTRODUCTION
            // ----------------

            // LOAD DATA FROM FILES AND CREATE EMPTY STORY
            FileData fileData;
            try
            {
                fileData = LoadFromFile.Data();
            }
            catch (Exception exception)
            {
                if (exception is FileNotFoundException || exception is ArgumentNullException)
                {
                    addGapToStory();
                    addTextToStory("Sorry, the Neverending Story couldn't load because it can't find the files it needs.");
                    addTextToStory("First, make sure you're running the most current version.");
                    addTextToStory("Then, if you are and this still happens, contact the developer and tell him to fix it.");
                    addTextToStory("Thanks! <3");
                    addGapToStory();
                    addTextToStory("(Press Enter to exit the program.)");
                    getPlayerInput();

                    return;
                }

                throw;
            }

            var story = Pick.Story(fileData);

            //{
            //    // PICK A BUNCH OF LOCATION NAMES.
            //    for (int i = 0; i < 50; i += 1)
            //    {
            //        //var validLocationTypes = new[] { LocationType.Forest, LocationType.Swamp, LocationType.Spring, LocationType.Sea, LocationType.Mountain, LocationType.Plains, LocationType.River, LocationType.Lake, LocationType.Desert, LocationType.Bay, LocationType.Fortress };
            //        //var validLocationTypes = new[] { LocationType.Road };
            //        //var location = Pick.Location(validLocationTypes.Random(), new List<Location>(), fileData);
            //        //WriteMessage(location.NameWithThe);

            //        var location = Pick.Town(new List<Location>(), fileData);
            //        WriteMessage(location.MainFeature.RelativePosition);
            //    }
            //}

            // DISPLAY INTRODUCTION
            // PICK PLAYER'S NAME
            addGapToStory();
            addTextToStory("Welcome to the Neverending Story!");
#if DEBUG
            addGapToStory();
            story.You.Name = "Alex";
            story.You.Sex = Sex.Male;
#else
            writeMessage("Type your character name and press Enter.");
            {
                string characterName = ReadInput();
                if (!string.IsNullOrWhiteSpace(characterName))
                {
                    story.You.Name = characterName;
                }
            }
            writeDashes();

            writeMessage("Male or female? (M/F)");
            {
                string playerSex = ReadInput().ToLower();
                if (playerSex == "m")
                {
                    story.You.Sex = Sex.Male;
                }
                else
                {
                    story.You.Sex = Sex.Female;
                }
            }
            writeDashes();

            // ASSIGN INSTINCT
            //            writeMessage(story.You.Name + @", which one best describes what you want to do?
            //1) " + Instinct.ToAvoidNotice + @"
            //2) " + Instinct.ToReclaimWhatWasTaken);
            string instinct = "2";// readInput();
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
            //writeDashes();
            //writeMessage("Hello, " + story.You.Name + ", you want " + instinct.ToLower() + ".");
            //writeDashes();
#endif

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
                        addGapToStory();
                        addTextToStory("THE END");
                        addGapToStory();
                        addTextToStory("Your almanac of places and people you know:");
                        addTextToStory(Process.AlmanacFor(story));
                        addGapToStory();
                        addTextToStory("Your inventory:");
                        addTextToStory(Process.InventoryOf(story.You));
                        addGapToStory();
                        addTextToStory("(Press Enter to exit.)");

                        getPlayerInput();

                        gameRunning = false;
                        continue;
                    }
                    else
                    {
                        // PROCESS AND DISPLAY MAIN MESSAGE
                        string message = Process.Message(currentScene.Message, story, fileData);
                        addTextToStory(message);
                    }
                }
                else
                {
                    getNewScene = true;
                }

                // IF THERE ARE NO CHOICES AVAILABLE IN THIS SCENE,
                // SKIP TO THE NEXT SCENE.
                if (string.IsNullOrWhiteSpace(currentScene.Choice1) || string.IsNullOrWhiteSpace(currentScene.Choice2))
                {
                    addTextToStory("");
                    continue;
                }

                // IF THERE ARE CHOICES AVAILABLE IN THIS SCENE,
                // PROCESS AND DISPLAY THEM.
                addGapToStory();
                {
                    string choice1 = Process.Message(currentScene.Choice1, story, fileData);
                    addTextToStory("1) " + choice1);
                }
                {
                    string choice2 = Process.Message(currentScene.Choice2, story, fileData);
                    addTextToStory("2) " + choice2);
                }

                if (!shownHelp)
                {
                    shownHelp = true;
                    addTextToStory("(Type \"help\" and hit Enter to learn how to play.)");
                }

                // ALLOW THE PLAYER TO MAKE A CHOICE
                string input = getPlayerInput();
                addGapToStory();

                // PROCESS THE PLAYER'S CHOICE
                if (input == "exit")
                {
                    gameRunning = false;

                    getNewScene = false;
                }
                else if (input == "help" || input == "?")
                {
                    addTextToStory(@"help or ? - show this help dialog
i or inventory - see the items you're carrying
a or almanac - see the people and places you know
1 or 2 - make a choice
exit - exit the story");

                    getNewScene = false;
                }
                else if (input == "inventory" || input == "i")
                {
                    string inventoryMessage = Process.InventoryOf(story.You);

                    addTextToStory("You're carrying:");
                    addTextToStory(inventoryMessage);

                    getNewScene = false;
                }
                else if (input == "almanac" || input == "a")
                {
                    string almanacMessage = Process.AlmanacFor(story);

                    addTextToStory("Here are people you've met and places you've been or heard of:");
                    addTextToStory(almanacMessage);

                    getNewScene = false;
                }
                else if (input == "1")
                {
                    string rawOutro = currentScene.Outro1;
                    string outro = Process.Message(rawOutro, story, fileData);

                    addTextToStory(outro);
                    addTextToStory("");

                    getNewScene = true;
                }
                else if (input == "2")
                {
                    string rawOutro = currentScene.Outro2;
                    string outro = Process.Message(rawOutro, story, fileData);

                    addTextToStory(outro);
                    addTextToStory("");

                    getNewScene = true;
                }
                else
                {
                    addTextToStory("(Please enter \"1\" or \"2\" to make a choice.)");

                    getNewScene = false;
                }

                currentScene.Done = true;
            }
        }
    }
}
