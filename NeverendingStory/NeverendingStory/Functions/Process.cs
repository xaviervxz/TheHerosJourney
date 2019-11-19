using System;
using System.Threading;
using System.Text.RegularExpressions;
using NeverendingStory.Data;
using System.Linq;

namespace NeverendingStory.Functions
{
    public static class Process
    {
        public static string Message(string message, Story story, FileData fileData)
        {
            var replacements = Regex.Matches(message, "\\{.+?\\}");

            string replacedMessage = message;

            foreach (Match replacement in replacements)
            {
                var key = replacement.Value.Substring(1, replacement.Value.Length - 2);

                if (key == "name")
                {
                    replacedMessage = replacedMessage.Replace("{name}", story.You.Name);
                }
                else if (key == "subPronoun")
                {
                    replacedMessage = replacedMessage.Replace("{subPronoun}", story.You.SubPronoun);
                }
                else if (key == "objPronoun")
                {
                    replacedMessage = replacedMessage.Replace("{objPronoun}", story.You.ObjPronoun);
                }
                else if (key == "possPronoun")
                {
                    replacedMessage = replacedMessage.Replace("{possPronoun}", story.You.PossPronoun);
                }
                else if (key.StartsWith("location"))
                {
                    var keyPieces = key.ToLower().Split(':');
                    string value = "";

                    if (keyPieces.Length > 1)
                    {
                        string locationRelation = keyPieces[1];

                        if (locationRelation == "current" && keyPieces.Length == 3)
                        {
                            string property = keyPieces[2];

                            if (property == "name")
                            {
                                value = story.You.CurrentLocation.Name;
                            }
                            else if (property == "namewiththe")
                            {
                                value = story.You.CurrentLocation.NameWithThe;
                            }
                            else if (property == "type")
                            {
                                value = story.You.CurrentLocation.SpecificType;
                            }
                        }
                        else if (locationRelation == "hometown" && keyPieces.Length >= 3)
                        {
                            string property = keyPieces[2];

                            if (property == "feature" && keyPieces.Length == 4)
                            {
                                var subProperty = keyPieces[3];
                                if (subProperty == "name")
                                {
                                    value = story.You.Hometown.MainGeologicalFeature.Name;
                                }
                                else if (subProperty == "namewiththe")
                                {
                                    value = story.You.Hometown.MainGeologicalFeature.NameWithThe;
                                }
                            }
                            else if (property == "name")
                            {
                                value = story.You.Hometown.Name;
                            }
                            else if (property == "namewiththe")
                            {
                                value = story.You.Hometown.NameWithThe;
                            }
                        }
                        else if (locationRelation == "nearby" && keyPieces.Length >= 3)
                        {
                            string property = keyPieces[2];

                            string currentLocationName = story.You.CurrentLocation.Name;
                            
                            Location nearbyLocation = null;

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
                                nearbyLocation = Pick.Location(LocationType.Forest, story.Locations, fileData);

                                story.NearbyLocations.Add(Tuple.Create(story.You.CurrentLocation.Name, nearbyLocation.Name));
                            }

                            // IF THE LOCATION IS NAMED, STORE IT IN NAMED LOCATIONS.
                            if (keyPieces.Length == 4)
                            {
                                story.NamedLocations[keyPieces[3]] = nearbyLocation;
                            }

                            if (property == "name")
                            {
                                value = nearbyLocation.Name;
                            }
                            else if (property == "namewiththe")
                            {
                                value = nearbyLocation.NameWithThe;
                            }
                        }
                    }

                    replacedMessage = replacedMessage.Replace("{" + key + "}", value);
                }
                else if (key.StartsWith("character"))
                {
                    var keyPieces = key.ToLower().Split(':');

                    string role = keyPieces[1];
                    Relationship roleEnum;

                    bool isValidRole = Enum.TryParse(role, true, out roleEnum);

                    Character character;
                    bool namedCharacterExists = story.NamedCharacters.TryGetValue(role, out character);

                    if (isValidRole && !namedCharacterExists)
                    {
                        character = Pick.Character(roleEnum, story.Characters, fileData.PeopleNames);
                    }

                    string property = keyPieces[2];
                    string value;
                    switch (property)
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
                        default:
                            value = "";
                            break;
                    }

                    if (keyPieces.Length == 4)
                    {
                        if (keyPieces[3] == "cap")
                        {
                            value = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value);
                        }
                        // IF THE CHARACTER IS NAMED, STORE IT IN NAMED CHARACTER.
                        else
                        {
                            story.NamedCharacters[keyPieces[3]] = character;
                        }
                    }

                    replacedMessage = replacedMessage.Replace("{" + key + "}", value);
                }
                else if (key.StartsWith("|") && key.EndsWith("|"))
                {
                    var command = key.Substring(1, key.Length - 2);

                    replacedMessage = replacedMessage.Replace("{" + key + "}", "");

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

                            story.You.Inventory.Add(item);
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

                            story.You.CurrentLocation = newLocation;
                        }
                    }
                }
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
