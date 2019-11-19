using CsvHelper;
using NeverendingStory.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeverendingStory.Functions
{
    using PeopleNames = Dictionary<PeopleNameOrigin, Dictionary<Sex, string[]>>;

    public static class LoadFromFile
    {
        public static FileData Data()
        {
            var fileData = new FileData();

            fileData.PeopleNames = LoadFromFile.PeopleNames();
            fileData.LocationData = LoadFromFile.LocationData();

            fileData.Scenes = LoadFromFile.Scenes();

            return fileData;
        }

        private static string GetDataFilePath(string fileName)
        {
            string jsonFileDirectory = Directory.GetCurrentDirectory();
#if DEBUG
            jsonFileDirectory = Directory.GetParent(jsonFileDirectory).Parent.FullName;
            //jsonFileDirectory = Path.Combine(Directory.GetParent(jsonFileDirectory).Parent.Parent.Parent.FullName, "NeverendingStory.Console");
#endif
            string filePath = Path.Combine(jsonFileDirectory, fileName);

            return filePath;
        }

        private static PeopleNames PeopleNames()
        {
            // Get the file path of the JSON file.
            string filePath = GetDataFilePath("PeopleNames.json");

            // Read the JSON file.
            string fileContents = File.ReadAllText(filePath);

            // Deserialize the JSON file.
            var names = JsonConvert.DeserializeObject<PeopleNames>(fileContents);

            return names;
        }

        private static LocationData LocationData()
        {
            // Get the file path of the JSON file.
            string filePath = GetDataFilePath("LocationData.json");
            
            // Read the JSON file.
            string fileContents = File.ReadAllText(filePath);

            // Deserialize the JSON file.
            var locationData = JsonConvert.DeserializeObject<LocationData>(fileContents);

            return locationData;
        }

        private static Scene[] Scenes()
        {
            // Get the file path of the CSV file.
            string filePath = GetDataFilePath("Scenes.csv");

            // TODO: Read from ODS file instead.

            // Read and parse the CSV file.
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream))
            using (var csv = new CsvReader(streamReader))
            {
                // Get the scenes from the CSV.
                var scenes = csv.GetRecords<Scene>().ToArray();

                // For each scene we got, assign some calculated variables...
                foreach (var scene in scenes)
                {
                    // The Stage
                    scene.Stage = Pick.StageFromCode(scene.Identifier);

                    // IsSubStage
                    char lastCharacter = scene.Identifier[scene.Identifier.Length - 1];
                    scene.IsSubStage = char.IsLetter(lastCharacter);
                }

                return scenes;
            }
        }
    }
}
