using TheHerosJourney.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;

namespace TheHerosJourney.Functions
{
    public static class LoadFromFile
    {
        /*static */
        public static string ReadAllText(Stream stream)
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
            stream.Dispose();

            string text = string.Join(Environment.NewLine, lines);

            return text;
        }

        public static Dictionary<Sex, string[]> CharacterData(Stream characterDataStream)
        {

            // **************************
            // LOADING PEOPLE'S NAMES ETC.
            // **************************

            // Read the JSON file.
            string peopleNamesFileContents = ReadAllText(characterDataStream);

            // Deserialize the JSON file.
            return JsonConvert.DeserializeObject<Dictionary<Sex, string[]>>(peopleNamesFileContents);

        }

        public static LocationData LocationData(Stream locationDataStream)
        {
            // **************************
            // LOADING LOCATION NAMES, INDUSTRIES, LAYOUTS, ETC.
            // **************************

            // Read the JSON file.
            string locationDataFileContents = ReadAllText(locationDataStream);

            // Deserialize the JSON file.
            return JsonConvert.DeserializeObject<LocationData>(locationDataFileContents);

        }

        public static Scene[] SceneData(Stream scenesStream)
        {
            // **************************
            // LOADING SCENES
            // **************************

            // Read the .csv file.
            // TODO: Read from an .ods file instead, or at least from an .xlsx file again.

            // *************************
            // CSVREADER CODE BELOW
            // *************************

            // LOAD SCENES
            using (var streamReader = new StreamReader(scenesStream))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    Scene[] scenes = csv.GetRecords<Scene>().ToArray();

                    foreach (var scene in scenes)
                    {
                        // For each scene we got, assign some calculated variables...

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

        public static Adventure[] AdventureData(Stream adventuresStream)
        {

            // LOAD ADVENTURES
            using (var streamReader = new StreamReader(adventuresStream))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    Adventure[] adventures = csv.GetRecords<Adventure>().ToArray();

                    foreach (var adventure in adventures)
                    {
                        // For each scene we got, assign some calculated variables...

                        // The Stage
                        adventure.RequiredSceneIds = adventure.RawRequiredScenes.Split(',');

                        // IsSubStage
                        var parsedTransitions = adventure.RawTransitions
                            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(transition => transition.Split(':'))
                            .ToArray();
                        adventure.Transitions = parsedTransitions
                            .ToDictionary(
                                transition => Pick.StageFromCode(transition.First()).Value,
                                transition => transition.Skip(1).ToArray()
                            );
                    }

                    return adventures;
                }
            }

        }
        internal static FileData Data(Stream characterDataStream, Stream locationDataStream, Stream scenesStream, Stream adventuresStream)
        {
            

            var fileData = new FileData();

            // **************************
            // LOADING PEOPLE'S NAMES ETC.
            // **************************

            // Read the JSON file.
            string peopleNamesFileContents = ReadAllText(characterDataStream);

            // Deserialize the JSON file.
            fileData.CharacterData = JsonConvert.DeserializeObject<Dictionary<Sex, string[]>>(peopleNamesFileContents);

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

            // *************************
            // CSVREADER CODE BELOW
            // *************************

            // LOAD SCENES
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

            // LOAD ADVENTURES
            using (var streamReader = new StreamReader(adventuresStream))
            using (var csv = new CsvReader(streamReader))
            {
                var adventures = csv.GetRecords<Adventure>().ToArray();

                foreach (var adventure in adventures)
                {
                    // For each scene we got, assign some calculated variables...

                    // The Stage
                    adventure.RequiredSceneIds = adventure.RawRequiredScenes.Split(',');

                    // IsSubStage
                    var parsedTransitions = adventure.RawTransitions
                        .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(transition => transition.Split(':'))
                        .ToArray();
                    adventure.Transitions = parsedTransitions
                        .ToDictionary(
                            transition => Pick.StageFromCode(transition.First()).Value,
                            transition => transition.Skip(1).ToArray()
                        );
                }

                fileData.Adventures = adventures;
            }


            // *************************
            // EPPLUS (XLSX) CODE BELOW
            // *************************

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


            // *************************
            // MANUAL ODS FILE PARSING CODE BELOW
            // *************************

            //var scenesOdsFile = new ZipArchive(scenesStream, ZipArchiveMode.Read, false);

            //var contentFile = scenesOdsFile.Entries.First(e => e.Name == "content.xml");

            //XDocument scenesXmlDocument = XDocument.Load(contentFile.Open());

            //var columnNames = scenesXmlDocument.Descendants()
            //    .First(row => row.Name.LocalName == "table-row" && row.Attributes().First(attr => attr.Name.LocalName == "style-name").Value == "ro1")
            //    .Descendants().Where(cell => cell.Name.LocalName == "table-cell")
            //    .Descendants().Where(paragraph => paragraph.Name.LocalName == "p").Select(column => column.Value)
            //    .ToArray();

            //var rows = scenesXmlDocument.Descendants()
            //    .Where(row => row.Name.LocalName == "table-row" && row.Attributes().First(attr => attr.Name.LocalName == "style-name").Value != "ro1")
            //    .Select(row => row.Descendants()
            //        .Where(cell => cell.Name.LocalName == "table-cell" && cell.Attributes().All(attr => attr.Name.LocalName != "number-columns-repeated"))
            //        .Select(cell => string.Join(Environment.NewLine, cell.Descendants()
            //            .Where(paragraph => paragraph.Name.LocalName == "p").Select(column => column.Value))
            //        )
            //        .ToArray()
            //    )
            //    .ToArray();

            //List<Scene> scenes = new List<Scene>();

            //foreach (var row in rows)
            //{
            //    var scene = new Scene();

            //    for (int column = 0; column < row.Length; column += 1)
            //    {
            //        string value = row[column] ?? "";
            //        string columnName = columnNames[column];

            //        switch (columnName)
            //        {
            //            case "Identifier":
            //                scene.Identifier = value;
            //                break;
            //            case "Conditions":
            //                scene.Conditions = value;
            //                break;
            //            case "Message":
            //                scene.Message = value;
            //                break;
            //            case "Choice1":
            //                scene.Choice1 = value;
            //                break;
            //            case "Choice2":
            //                scene.Choice2 = value;
            //                break;
            //            case "Outro1":
            //                scene.Outro1 = value;
            //                break;
            //            case "Outro2":
            //                scene.Outro2 = value;
            //                break;
            //        }
            //    }

            //    // For each scene we got, assign some calculated variables...

            //    // The Stage
            //    scene.Stage = Pick.StageFromCode(scene.Identifier);

            //    // IsSubStage
            //    char lastCharacter = scene.Identifier[scene.Identifier.Length - 1];
            //    scene.IsSubStage = char.IsLetter(lastCharacter);

            //    scenes.Add(scene);
            //}
            //fileData.Scenes = scenes.ToArray();

            scenesStream.Dispose();

            return fileData;
        }
    }
}
