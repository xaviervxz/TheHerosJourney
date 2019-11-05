using CsvHelper;
using NeverendingStory.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeverendingStory.Functions
{
    using Names = Dictionary<NameOrigin, Dictionary<Sex, string[]>>;

    public static class LoadFromFile
    {
        public static FileData Data(string names, string scenes)
        {
            var fileData = new FileData();

            fileData.Names = LoadFromFile.Names(names);

            fileData.Scenes = LoadFromFile.Scenes(scenes);

            return fileData;
        }

        private static Names Names(string fileName)
        {
            // Get the file path of the JSON file.
            string jsonFileDirectory = Directory.GetCurrentDirectory();
#if DEBUG
            jsonFileDirectory = Directory.GetParent(jsonFileDirectory).Parent.FullName;
#endif
            string filePath = Path.Combine(jsonFileDirectory, fileName + ".json");
            
            // Read the JSON file.
            string fileContents = File.ReadAllText(filePath);

            // Deserialize the JSON file.
            var names = JsonConvert.DeserializeObject<Names>(fileContents);

            return names;
        }

        private static Scene[] Scenes(string fileName)
        {
            // Get the file path of the CSV file.
            string jsonFileDirectory = Directory.GetCurrentDirectory();
#if DEBUG
            jsonFileDirectory = Directory.GetParent(jsonFileDirectory).Parent.FullName;
#endif
            string filePath = Path.Combine(jsonFileDirectory, fileName + ".csv");

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
