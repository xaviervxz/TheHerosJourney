using System;
using System.Threading;
using System.Text.RegularExpressions;
using NeverendingStory.Data;
using System.Linq;

namespace NeverendingStory.Functions
{
    public static class Process
    {
        public static string ToTitleCase(this string text)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        public static string Message(string message, Story story, FileData fileData)
        {
            var replacements = Regex.Matches(message, "\\{.+?\\}");

            string replacedMessage = message;

            foreach (Match replacement in replacements)
            {
                var key = replacement.Value.Substring(1, replacement.Value.Length - 2);

                var keyPieces = key.ToLower().Split(':');
                var primaryKey = keyPieces[0];

                string replacementValue = "";

                if (key.StartsWith("|") && key.EndsWith("|"))
                {
                    var command = key.Substring(1, key.Length - 2);

                    var commandOptions = command.Split(':');

                    if (commandOptions.Length == 1)
                    {
                        story.NextSceneIdentifier = commandOptions[0];
                    }
                    else
                    {
                        if (commandOptions[0] == "GIVE" && commandOptions.Length == 4)
                        {
                            var item = new Item
                            {
                                Identifier = commandOptions[1],
                                Name = commandOptions[2],
                                Description = commandOptions[3]
                            };

                            item.Description = item.Description.Replace("[", "{").Replace("]", "}").Replace("-", ":");
                            item.Description = Process.Message(item.Description, story, fileData);

                            story.You.Inventory.Add(item);
                        }
                        else if (commandOptions[0] == "REMOVE" && commandOptions.Length == 2)
                        {
                            var item = story.You.Inventory.FirstOrDefault(i => i.Identifier == commandOptions[1]);

                            if (item != null)
                            {
                                story.You.Inventory.Remove(item);
                            }
                        }
                        else if (commandOptions[0] == "RENAME" && commandOptions.Length == 4)
                        {
                            var item = story.You.Inventory.FirstOrDefault(i => i.Identifier == commandOptions[1]);

                            if (item != null)
                            {
                                item.Name = commandOptions[2];
                                item.Description = commandOptions[3];
                            }
                        }
                        else if (commandOptions[0] == "GOTO" && commandOptions.Length >= 2)
                        {
                            Location newLocation;
                            bool namedLocationExists = story.NamedLocations.TryGetValue(commandOptions[1], out newLocation);

                            if (namedLocationExists)
                            {
                                story.You.CurrentLocation = newLocation;
                            }
                            else if (commandOptions[1] == "hometown")
                            {
                                story.You.CurrentLocation = story.You.Hometown;
                            }
                            else if (commandOptions[1] == "goal")
                            {
                                story.You.CurrentLocation = story.You.Goal;
                            }
                        }
                        else if (commandOptions[0] == "SET" && commandOptions.Length == 3)
                        {
                            string flagKey = commandOptions[1];
                            string flagValue = commandOptions[2];

                            if (flagKey == "goal")
                            {
                                if (flagValue == "hometown")
                                {
                                    story.You.Goal = story.You.Hometown;
                                }
                                else
                                {
                                    story.NamedLocations.TryGetValue(flagValue, out Location goalLocation);

                                    story.You.Goal = goalLocation;
                                }
                            }
                            else
                            {
                                story.Flags[flagKey] = flagValue;
                            }
                        }
                    }
                }
                else if (keyPieces.Length == 1)
                {
                    if (key == "name")
                    {
                        replacementValue = story.You.Name;
                    }
                    else if (key == "subPronoun")
                    {
                        replacementValue = story.You.SubPronoun;
                    }
                    else if (key == "objPronoun")
                    {
                        replacementValue = story.You.ObjPronoun;
                    }
                    else if (key == "possPronoun")
                    {
                        replacementValue = story.You.PossPronoun;
                    }
                }
                else if (primaryKey == "location")
                {
                    string locationRelation = keyPieces[1];

                    Location location = null;
                    string property = "";

                    if (locationRelation == "current" && keyPieces.Length >= 3)
                    {
                        location = story.You.CurrentLocation;
                        property = keyPieces[2];
                    }
                    else if (locationRelation == "goal" && keyPieces.Length >= 3)
                    {
                        location = story.You.Goal;
                        property = keyPieces[2];
                    }
                    else if (locationRelation == "hometown" && keyPieces.Length >= 3)
                    {
                        location = story.You.Hometown;
                        property = keyPieces[2];

                        if (property == "feature" && keyPieces.Length == 4)
                        {
                            property = keyPieces[3];

                            if (property == "relativeposition")
                            {
                                replacementValue = story.You.Hometown.MainFeature.RelativePosition;
                            }
                        }
                    }
                    else if (locationRelation == "nearby" && keyPieces.Length >= 3)
                    {
                        // FIND A NEARBY LOCATION TO THE CURRENT LOCATION
                        Location nearbyLocation = null;

                        string currentLocationName = story.You.CurrentLocation.Name;
                        var nearbyLocationTuple = story.NearbyLocations
                            .Find(l => l.Item1 == currentLocationName || l.Item2 == currentLocationName);
                        if (nearbyLocationTuple != null)
                        {
                            nearbyLocation = story.Locations
                                .Where(l => l.Name != currentLocationName)
                                .FirstOrDefault(l => l.Name == nearbyLocationTuple.Item1 || l.Name == nearbyLocationTuple.Item2);
                        }

                        if (nearbyLocation == null)
                        {
                            var validNearbyLocations = new[] { LocationType.Forest, LocationType.Swamp, LocationType.Mountain, LocationType.Plains };

                            nearbyLocation = Pick.Location(validNearbyLocations.Random(), story.Locations, fileData);

                            story.NearbyLocations.Add(Tuple.Create(story.You.CurrentLocation.Name, nearbyLocation.Name));
                        }

                        location = nearbyLocation;
                        property = keyPieces[2];
                    }
                    else
                    {
                        string rawLocationType = locationRelation.ToTitleCase();
                        bool locationTypeExists = Enum.TryParse(rawLocationType, out LocationType locationType);

                        if (locationTypeExists)
                        {
                            location = Pick.Location(locationType, story.Locations, fileData);
                        }
                        else
                        {
                            bool namedLocationExists = story.NamedLocations.TryGetValue(locationRelation, out location);
                        }

                        if (location != null && keyPieces.Length >= 3)
                        {
                            property = keyPieces[2];
                        }
                    }

                    switch (property)
                    {
                        case "name":
                            replacementValue = location.Name;
                            break;
                        case "namewiththe":
                            replacementValue = location.NameWithThe;
                            break;
                        case "type":
                            replacementValue = location.SpecificType;
                            break;
                        case "covers":
                            replacementValue = fileData.LocationData.Names.Terrain[location.Type].Covers;
                            break;
                        case "cover":
                            replacementValue = fileData.LocationData.Names.Terrain[location.Type].Cover;
                            break;
                        default:
                            break;
                    }

                    if (keyPieces.Length == 4)
                    {
                        if (keyPieces[3] == "cap")
                        {
                            replacementValue = replacementValue.ToTitleCase();
                        }
                        // IF THE LOCATION IS NAMED, STORE IT IN NAMED LOCATIONS.
                        else if (property != "relativeposition")
                        {
                            story.NamedLocations[keyPieces[3]] = location;
                        }
                    }

                }
                else if (primaryKey == "industry")
                {
                    string locationRelation = keyPieces[1];

                    Town town = null;
                    string property = "";

                    if (locationRelation == "current" && keyPieces.Length == 3)
                    {
                        town = story.You.CurrentLocation as Town;
                        property = keyPieces[2];
                    }
                    else if (locationRelation == "hometown" && keyPieces.Length == 3)
                    {
                        town = story.You.Hometown;
                        property = keyPieces[2];
                    }
                    else
                    {
                        string rawLocationType = locationRelation.ToTitleCase();
                        bool locationTypeExists = Enum.TryParse(rawLocationType, out LocationType locationType);

                        if (locationTypeExists)
                        {
                            town = Pick.Town(story.Locations, fileData);
                        }
                        else
                        {
                            bool namedLocationExists = story.NamedLocations.TryGetValue(locationRelation, out Location location);
                            town = location as Town;
                        }

                        if (town != null && keyPieces.Length == 3)
                        {
                            property = keyPieces[2];
                        }
                    }

                    switch (property)
                    {
                        case "workplace":
                            replacementValue = town.MainIndustryData.Workplace;
                            break;
                        case "workger":
                            replacementValue = town.MainIndustryData.WorkGer;
                            break;
                        case "goods":
                            replacementValue = town.MainIndustryData.Goods;
                            break;
                        case "purpose":
                            replacementValue = town.MainIndustryData.Purpose;
                            break;
                        case "goodday":
                            replacementValue = town.MainIndustryData.GoodDay;
                            break;
                        case "gooddayfinal":
                            replacementValue = town.MainIndustryData.GoodDayFinal;
                            break;
                        default:
                            break;
                    }

                    if (keyPieces.Length == 4)
                    {
                        if (keyPieces[3] == "cap")
                        {
                            replacementValue = replacementValue.ToTitleCase();
                        }
                    }

                }
                else if (primaryKey == "character")
                {
                    string role = keyPieces[1];

                    bool isValidRole = Enum.TryParse(role, true, out Relationship roleEnum);

                    bool namedCharacterExists = story.NamedCharacters.TryGetValue(role, out Character character);

                    if (isValidRole && !namedCharacterExists)
                    {
                        character = Pick.Character(roleEnum, story.Characters, fileData.CharacterData);
                    }

                    string property = keyPieces[2];
                    switch (property)
                    {
                        case "name":
                            replacementValue = character.Name;
                            break;
                        case "subpronoun":
                            replacementValue = character.SubPronoun;
                            break;
                        case "objpronoun":
                            replacementValue = character.ObjPronoun;
                            break;
                        case "posspronoun":
                            replacementValue = character.PossPronoun;
                            break;
                        case "sexage":
                            replacementValue = character.SexAge;
                            break;
                        case "baron":
                            replacementValue = character.Baron;
                            break;
                        default:
                            replacementValue = "";
                            break;
                    }

                    if (keyPieces.Length == 4)
                    {
                        if (keyPieces[3] == "cap")
                        {
                            replacementValue = replacementValue.ToTitleCase();
                        }
                        // IF THE CHARACTER IS NAMED, STORE IT IN NAMED CHARACTER.
                        else
                        {
                            story.NamedCharacters[keyPieces[3]] = character;
                        }
                    }
                }

                replacedMessage = replacedMessage.Replace("{" + key + "}", replacementValue);
            }

            return replacedMessage;
        }

        public static string InventoryOf(Character character)
        {
            var inventoryLines = character.Inventory.Select(i => "* " + i.Name + " - " + i.Description);

            var inventory = string.Join(Environment.NewLine, inventoryLines);

            return inventory;
        }
    }
}
