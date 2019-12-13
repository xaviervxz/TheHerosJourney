using CsvHelper;
using NeverendingStory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NeverendingStory.Functions
{
    using PeopleNames = Dictionary<PeopleNameOrigin, Dictionary<Sex, string[]>>;

    internal static class LoadFromFile
    {
        internal static FileData Data(Stream characterDataStream, Stream locationDataStream, Stream scenesStream)
        {
            /*static */string ReadAllText(Stream stream)
            {
                if (stream == null)
                {
                    throw new ArgumentNullException();
                }

                var lines = new List<string>();

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                string text = string.Join(Environment.NewLine, lines);

                return text;
            }

            var fileData = new FileData();

            // **************************
            // LOADING PEOPLE'S NAMES ETC.
            // **************************

            // Read the JSON file.
            string peopleNamesFileContents = ReadAllText(characterDataStream);

            // Deserialize the JSON file.
            fileData.CharacterData = JsonConvert.DeserializeObject<PeopleNames>(peopleNamesFileContents);

            // **************************
            // LOADING LOCATION NAMES, INDUSTRIES, LAYOUTS, ETC.
            // **************************

            // Read the JSON file.
            string locationDataFileContents = ReadAllText(locationDataStream);

            // Deserialize the JSON file.
            fileData.LocationData = JsonConvert.DeserializeObject<LocationData>(locationDataFileContents);

            // **************************
            // LOADING SCENES
            // **************************

            // Read the .csv file.
            // TODO: Read from an .ods file instead, or at least from an .xlsx file again.

            using (var streamReader = new StreamReader(scenesStream))
            using (var csv = new CsvReader(streamReader))
            {
                var scenes = csv.GetRecords<Scene>().ToArray();

                foreach (var scene in scenes)
                {
                    // For each scene we got, assign some calculated variables...

                    // The Stage
                    scene.Stage = Pick.StageFromCode(scene.Identifier);

                    // IsSubStage
                    char lastCharacter = scene.Identifier[scene.Identifier.Length - 1];
                    scene.IsSubStage = char.IsLetter(lastCharacter);
                }

                fileData.Scenes = scenes;
            }

            //using (var excelPackage = new OfficeOpenXml.ExcelPackage())
            //{
            //    excelPackage.Load(scenesStream);

            //    if (excelPackage.Workbook.Worksheets.Count != 0)
            //    {
            //        var worksheet = excelPackage.Workbook.Worksheets.First();

            //        var headers = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(c => c.Text).ToArray();

            //        List<Scene> scenes = new List<Scene>();

            //        for (int row = 2; row <= worksheet.Dimension.Rows; row += 1)
            //        {
            //            var scene = new Scene();

            //            for (int col = 1; col <= worksheet.Dimension.Columns; col += 1)
            //            {
            //                string value = worksheet.Cells[row, col].Value?.ToString() ?? "";
            //                string header = headers[col - 1];

            //                switch (header)
            //                {
            //                    case "Identifier":
            //                        scene.Identifier = value;
            //                        break;
            //                    case "Conditions":
            //                        scene.Conditions = value;
            //                        break;
            //                    case "Message":
            //                        scene.Message = value;
            //                        break;
            //                    case "Choice1":
            //                        scene.Choice1 = value;
            //                        break;
            //                    case "Choice2":
            //                        scene.Choice2 = value;
            //                        break;
            //                    case "Outro1":
            //                        scene.Outro1 = value;
            //                        break;
            //                    case "Outro2":
            //                        scene.Outro2 = value;
            //                        break;
            //                }
            //            }

            //            // For each scene we got, assign some calculated variables...

            //            // The Stage
            //            scene.Stage = Pick.StageFromCode(scene.Identifier);

            //            // IsSubStage
            //            char lastCharacter = scene.Identifier[scene.Identifier.Length - 1];
            //            scene.IsSubStage = char.IsLetter(lastCharacter);

            //            scenes.Add(scene);
            //        }

            //        fileData.Scenes = scenes.ToArray();
            //    }
            //}

            return fileData;
        }
    }
}
