using System;
using System.Threading;
using System.Text.RegularExpressions;
using NeverendingStory.Data;
using System.Linq;
using System.Collections.Generic;

namespace NeverendingStory.Functions
{
    public static class Process
    {
        public static string ToTitleCase(this string text)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        public static T[] ParseToValidTypes<T>(this IEnumerable<string> rawTypes)
            where T : struct
        {
            var validTypes = rawTypes
                .Select(rt => rt.ParseToValidType<T>())
                .Where(t => t != null)
                .Select(t => (T)t)
                .ToArray();

            if (validTypes.Length == 0)
            {
                validTypes = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            }

            return validTypes;
        }

        public static T? ParseToValidType<T>(this string rawType)
            where T : struct
        {
            if (Enum.TryParse(rawType.ToTitleCase(), out T type))
            {
                return type;
            }

            return null;
        }

        public static string Message(string message, Story story, FileData fileData)
        {
            static string subMessage(string subMessage, Story story, FileData fileData)
            {
                var replacedSubMessage = subMessage.Replace("[", "{").Replace("]", "}").Replace("-", ":");

                string processedSubMessage = Process.Message(replacedSubMessage, story, fileData);

                return processedSubMessage;
            }

            var replacements = Regex.Matches(message, "\\{.+?\\}");

            string replacedMessage = message;

            foreach (Match replacement in replacements)
            {
                var key = replacement.Value.Substring(1, replacement.Value.Length - 2);

                var keyPieces = key.Split(':');
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

                            item.Description = subMessage(item.Description, story, fileData);

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
                    switch (key)
                    {
                        case "name":
                            replacementValue = story.You.Name;
                            break;
                        case "subPronoun":
                            replacementValue = story.You.SubPronoun;
                            break;
                        case "objPronoun":
                            replacementValue = story.You.ObjPronoun;
                            break;
                        case "possPronoun":
                            replacementValue = story.You.PossPronoun;
                            break;
                        case "chief":
                            replacementValue = story.You.Chief;
                            break;
                    }
                }
                else if (primaryKey == "if")
                {
                    string condition = string.Join(':', keyPieces.Skip(1).Take(keyPieces.Length - 2));
                    var conditionIsTrue = Condition.IsMet(story, condition);

                    if (conditionIsTrue)
                    {
                        replacementValue = subMessage(keyPieces.Last(), story, fileData);
                    }
                }
                else if (primaryKey == "almanac" && keyPieces.Length >= 3)
                {
                    // STORE THE LOCATION IN THE ALMANAC.
                    string almanacTitle = subMessage(keyPieces[1], story, fileData);
                    string almanacDescription = subMessage(keyPieces[2], story, fileData);

                    if (story.Almanac.TryGetValue(almanacTitle, out string existingDescription)
                        && !(keyPieces.Length == 4 && keyPieces[3] == "reset"))
                    {
                        if (existingDescription.Contains(almanacDescription))
                        {
                            almanacDescription = existingDescription;
                        }
                        else
                        {
                            almanacDescription = existingDescription + ", " + almanacDescription;
                        }
                    }

                    story.Almanac[almanacTitle] = almanacDescription;
                }
                else if (primaryKey == "location")
                {
                    static string locationProperty(Location location, string property, bool titleCase)
                    {
                        string value = "";

                        switch (property)
                        {
                            case "name":
                                value = location.Name;
                                break;
                            case "namewiththe":
                                value = location.NameWithThe;
                                break;
                            case "type":
                                value = location.SpecificType;
                                break;
                            default:
                                break;
                        }

                        if (titleCase)
                        {
                            value = value.ToTitleCase();
                        }

                        return value;
                    }

                    // PICK AND NAME A NEW LOCATION.
                    if (keyPieces[1] == "pick")
                    {
                        //     0       1        2        3          4          5                               6
                        // {location:pick:forest|swamp:current:pathtobaron:namewiththe:The home of [character|baron|baron] [character|baron|name].}

                        // GET LIST OF VALID LOCATION TYPES.
                        var rawValidTypes = keyPieces[2].Split('|');
                        var validTypes = rawValidTypes.ParseToValidTypes<LocationType>();

                        // WHAT LOCATION SHOULD THIS NEW LOCATION BE NEAR?
                        var nearbyLocationKey = keyPieces[3];
                        Location centerLocation = null;
                        if (nearbyLocationKey == "current")
                        {
                            centerLocation = story.You.CurrentLocation;
                        }
                        else if (nearbyLocationKey == "hometown")
                        {
                            centerLocation = story.You.Hometown;
                        }
                        else
                        {
                            story.NamedLocations.TryGetValue(nearbyLocationKey, out centerLocation);
                        }

                        // FIND LOCATION NEARBY THE CHOSEN "CENTER" THAT MATCHES.
                        Location nearbyLocation = null;
                        string centerLocationName = centerLocation.Name;
                        var nearbyLocationTuple = story.NearbyLocations
                            .Find(l => l.Item1 == centerLocationName || l.Item2 == centerLocationName);
                        if (nearbyLocationTuple != null)
                        {
                            nearbyLocation = story.Locations
                                .Where(l => l.Name != centerLocationName && validTypes.Contains(l.Type))
                                .FirstOrDefault(l => l.Name == nearbyLocationTuple.Item1 || l.Name == nearbyLocationTuple.Item2);
                        }

                        // IF NO NEARBY LOCATION EXISTS, CREATE/PICK A NEW ONE.
                        if (nearbyLocation == null)
                        {
                            nearbyLocation = Pick.Location(validTypes.Random(), story.Locations, fileData);

                            story.NearbyLocations.Add(Tuple.Create(centerLocation.Name, nearbyLocation.Name));
                        }

                        // STORE THE LOCATION IN NAMED LOCATIONS.
                        story.NamedLocations[keyPieces[4]] = nearbyLocation;

                        replacementValue = "";
                    }
                    else if (keyPieces.Length >= 3)
                    {
                        string locationRelation = keyPieces[1];

                        Location location = null;
                        string property = "";

                        if (locationRelation == "current")
                        {
                            location = story.You.CurrentLocation;
                            property = keyPieces[2];
                        }
                        else if (locationRelation == "goal")
                        {
                            location = story.You.Goal;
                            property = keyPieces[2];
                        }
                        else if (locationRelation == "hometown")
                        {
                            location = story.You.Hometown;
                            property = keyPieces[2];

                            if (property == "feature" && keyPieces.Length == 4)
                            {
                                if (keyPieces[3] == "relativeposition")
                                {
                                    replacementValue = story.You.Hometown.MainFeature.RelativePosition;
                                }
                            }
                        }
                        else
                        {
                            bool namedLocationExists = story.NamedLocations.TryGetValue(locationRelation, out location);

                            if (location != null)
                            {
                                property = keyPieces[2];
                            }
                        }

                        if (property != "feature")
                        {
                            replacementValue = locationProperty(location, property, keyPieces.Length >= 4 && keyPieces[3] == "cap");
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

                    switch (property.ToLower())
                    {
                        case "workger":
                            replacementValue = town.MainIndustryData.WorkGer;
                            break;
                        case "workplace":
                            replacementValue = town.MainIndustryData.Workplace;
                            break;
                        case "goodsger":
                            replacementValue = town.MainIndustryData.GoodsGer;
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
                    static string characterProperty(Character character, string property, bool titleCase)
                    {
                        if (character == null)
                        {
                            return "";
                        }

                        string value = "";

                        switch (property.ToLower())
                        {
                            case "name":
                                value = character.Name;
                                break;
                            case "subpronoun":
                                value = character.SubPronoun;
                                break;
                            case "objpronoun":
                                value = character.ObjPronoun;
                                break;
                            case "posspronoun":
                                value = character.PossPronoun;
                                break;
                            case "sexage":
                                value = character.SexAge;
                                break;
                            case "baron":
                                value = character.Baron;
                                break;
                            case "chief":
                                value = character.Chief;
                                break;
                            default:
                                value = "";
                                break;
                        }

                        if (titleCase)
                        {
                            value = value.ToTitleCase();
                        }

                        return value;
                    }

                    if (keyPieces[1] == "pick")
                    {
                        //      0      1       2         3        4
                        // {character:pick:baronhome:antagonist:baron}

                        string currentLocationName = keyPieces[2];
                        if (!story.NamedLocations.TryGetValue(currentLocationName, out Location currentLocation))
                        {
                            switch (currentLocationName)
                            {
                                case "hometown":
                                    currentLocation = story.You.Hometown;
                                    break;
                                case "current":
                                    currentLocation = story.You.CurrentLocation;
                                    break;
                                default:
                                    break;
                            }
                        }

                        string role = keyPieces[3];
                        var relationship = role.ParseToValidType<Relationship>();

                        Character character = story.Characters
                            .FirstOrDefault(c =>
                                c.Relationship == relationship &&
                                (c.CurrentLocation == currentLocation || currentLocation == null)
                            );

                        if (character == null && relationship != null)
                        {
                            // PICK A CHARACTER.
                            character = Pick.Character(relationship.Value, story.Characters, fileData.CharacterData);

                            character.CurrentLocation = currentLocation;
                        }

                        // STORE IT IN NAMED CHARACTER.
                        story.NamedCharacters[keyPieces[4]] = character;
                    }
                    else
                    {
                        string role = keyPieces[1];

                        bool isValidRole = Enum.TryParse(role, true, out Relationship roleEnum);

                        bool namedCharacterExists = story.NamedCharacters.TryGetValue(role, out Character character);

                        if (isValidRole && !namedCharacterExists)
                        {
                            // TODO ????????
                        }

                        string property = keyPieces[2];

                        replacementValue = characterProperty(character, property, keyPieces.Length == 4 && keyPieces[3] == "cap");
                    }
                }

                replacedMessage = replacedMessage.Replace("{" + key + "}", replacementValue);
            }

            return replacedMessage;
        }
    }
}
