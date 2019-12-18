using NeverendingStory.Models;
using System;
using System.IO;
using System.Linq;

namespace NeverendingStory.Functions
{
    public static class Run
    {
        public static FileData LoadGameData(Stream characterDataStream, Stream locationDataStream, Stream scenesStream, Action showLoadGameFilesError)
        {
            // ----------------
            // DATA SET UP AND INTRODUCTION
            // ----------------

            // LOAD DATA FROM FILES AND CREATE EMPTY STORY
            FileData fileData;
            try
            {
                fileData = LoadFromFile.Data(characterDataStream, locationDataStream, scenesStream);
            }
            catch (Exception exception)
            {
                if (exception is FileNotFoundException || exception is ArgumentNullException)
                {
                    showLoadGameFilesError?.Invoke();

                    return null;
                }

                throw;
            }

            return fileData;
        }

        public static Story NewStory(FileData fileData, string storySeed, string[] reqSceneIds = null)
        {
            if (string.IsNullOrWhiteSpace(storySeed))
            {
                Pick.StorySeed = new Random();
            }
            else
            {
                int seed = storySeed.Sum(letter => letter);

                Pick.StorySeed = new Random(seed);
            }

            var story = Pick.Story(fileData);

            if (reqSceneIds != null)
            {
                Pick.ReqSceneIds = reqSceneIds;
            }

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

            return story;
        }

        public static Scene NewScene(FileData fileData, Story story, Action<string> addTextToStory)
        {
            var newScene = Pick.NextScene(fileData.Scenes, story);

            if (newScene != null)
            {
                // PROCESS AND DISPLAY MAIN MESSAGE
                string message = Process.Message(newScene.Message, story, fileData);
                addTextToStory(message);
            }

            return newScene;
        }

        public static void Outro1(FileData fileData, Story story, Scene currentScene, Action<string> addTextToStory)
        {
            Outro(1, fileData, story, currentScene, addTextToStory);
        }

        public static void Outro2(FileData fileData, Story story, Scene currentScene, Action<string> addTextToStory)
        {
            Outro(2, fileData, story, currentScene, addTextToStory);
        }

        private static void Outro(byte outroNum, FileData fileData, Story story, Scene currentScene, Action<string> addTextToStory)
        {
            if (currentScene == null)
            {
                //throw new ArgumentNullException(nameof(currentScene), "The currentScene is null, so we can't show an Outro.");
                return;
            }

            string rawOutro = outroNum == 1 ? currentScene.Outro1 : currentScene.Outro2;
            string outro = Process.Message(rawOutro, story, fileData);

            addTextToStory(outro);
            addTextToStory("");

            currentScene.Done = true;
        }

        public static bool PresentChoices(FileData fileData, Story story, Scene currentScene, Action<string, string> presentChoices, Action<string> addTextToStory)
        {
            if (currentScene == null)
            {
                return false;
            }

            // IF THERE ARE NO CHOICES AVAILABLE IN THIS SCENE,
            // SKIP TO THE NEXT SCENE.
            if (string.IsNullOrWhiteSpace(currentScene.Choice1) || string.IsNullOrWhiteSpace(currentScene.Choice2))
            {
                addTextToStory("");
                return false;
            }

            string choice1 = Process.Message(currentScene.Choice1, story, fileData);
            string choice2 = Process.Message(currentScene.Choice2, story, fileData);

            presentChoices(choice1, choice2);

            return true;
        }

        public static string NewName(FileData fileData, Sex sex)
        {
            string name = fileData.CharacterData[PeopleNameOrigin.Westron][sex].Random();

            return name;
        }
    }
}
