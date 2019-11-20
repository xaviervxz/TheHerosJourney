using CsvHelper;
using NeverendingStory.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace NeverendingStory.Functions
{
    using PeopleNames = Dictionary<PeopleNameOrigin, Dictionary<Sex, string[]>>;

    public static class LoadFromFile
    {
        public static FileData Data()
        {
            string GetDataFilePath(string fileName)
            {
                string jsonFileDirectory = Directory.GetCurrentDirectory();
#if DEBUG
                jsonFileDirectory = Directory.GetParent(jsonFileDirectory).Parent.FullName;
                //jsonFileDirectory = Path.Combine(Directory.GetParent(jsonFileDirectory).Parent.Parent.Parent.FullName, "NeverendingStory.Console");
#endif
                string filePath = Path.Combine(jsonFileDirectory, fileName);

                return filePath;
            }

            var fileData = new FileData();

            // **************************
            // LOADING PEOPLE NAMES
            // **************************

            // Get the file path of the JSON file.
            string peopleNamesFilePath = GetDataFilePath("CharacterData.json");

            // Read the JSON file.
            string peopleNamesFileContents = File.ReadAllText(peopleNamesFilePath);

            // Deserialize the JSON file.
            fileData.CharacterData = JsonConvert.DeserializeObject<PeopleNames>(peopleNamesFileContents);

            // **************************
            // LOADING LOCATION DATA
            // **************************

            // Get the file path of the JSON file.
            string locationDataFilePath = GetDataFilePath("LocationData.json");

            // Read the JSON file.
            string locationDataFileContents = File.ReadAllText(locationDataFilePath);

            // Deserialize the JSON file.
            fileData.LocationData = JsonConvert.DeserializeObject<LocationData>(locationDataFileContents);

            // **************************
            // LOADING SCENES
            // **************************

            // Get the file path of the Spreadsheet file.
            // TODO: Read from ODS file instead.
            string scenesFilePath = GetDataFilePath("Scenes.xlsx");

            using (var fileStream = new FileStream(scenesFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var excelPackage = new OfficeOpenXml.ExcelPackage())
            {
                excelPackage.Load(fileStream);

                if (excelPackage.Workbook.Worksheets.Count != 0)
                {
                    var worksheet = excelPackage.Workbook.Worksheets.First();

                    var headers = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(c => c.Text).ToArray();

                    List<Scene> scenes = new List<Scene>();

                    for (int row = 2; row <= worksheet.Dimension.Rows; row += 1)
                    {
                        var scene = new Scene();

                        for (int col = 1; col <= worksheet.Dimension.Columns; col += 1)
                        {
                            string value = worksheet.Cells[row, col].Value?.ToString() ?? "";
                            string header = headers[col - 1];

                            switch (header)
                            {
                                case "Identifier":
                                    scene.Identifier = value;
                                    break;
                                case "Conditions":
                                    scene.Conditions = value;
                                    break;
                                case "Message":
                                    scene.Message = value;
                                    break;
                                case "Choice1":
                                    scene.Choice1 = value;
                                    break;
                                case "Choice2":
                                    scene.Choice2 = value;
                                    break;
                                case "Outro1":
                                    scene.Outro1 = value;
                                    break;
                                case "Outro2":
                                    scene.Outro2 = value;
                                    break;
                            }
                        }

                        // For each scene we got, assign some calculated variables...

                        // The Stage
                        scene.Stage = Pick.StageFromCode(scene.Identifier);

                        // IsSubStage
                        char lastCharacter = scene.Identifier[scene.Identifier.Length - 1];
                        scene.IsSubStage = char.IsLetter(lastCharacter);

                        scenes.Add(scene);
                    }

                    fileData.Scenes = scenes.ToArray();
                }
            }

            return fileData;
        }
    }
}
