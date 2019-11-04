using System;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;
using NeverendingStory.Data;
using System.Linq;

namespace NeverendingStory.Functions
{
    using Names = Dictionary<NameOrigin, Dictionary<Sex, string[]>>;

    public static class Process
    {
        public static string Message(string message, Story story, Names names)
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
                        character = Pick.Character(roleEnum, story.Characters, names);
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
                        switch (command)
                        {
                            case "CTA":
                                story.CurrentStage = JourneyStage.CallToAdventure;
                                break;
                            case "ROC":
                                story.CurrentStage = JourneyStage.RefusalOfCall;
                                break;
                            case "MTM":
                                story.CurrentStage = JourneyStage.MeetingTheMentor;
                                break;
                            case "CTT":
                                story.CurrentStage = JourneyStage.CrossingTheThreshhold;
                                break;
                            case "BOTW":
                                story.CurrentStage = JourneyStage.BellyOfTheWhale;
                                break;
                            case "ROT":
                                story.CurrentStage = JourneyStage.RoadOfTrials;
                                break;
                            case "MWG":
                                story.CurrentStage = JourneyStage.MeetingWithGoddess;
                                break;
                            case "WAT":
                                story.CurrentStage = JourneyStage.WomanAsTemptress;
                                break;
                            case "AWF":
                                story.CurrentStage = JourneyStage.AtonementWithFather;
                                break;
                            case "A":
                                story.CurrentStage = JourneyStage.Apotheosis;
                                break;
                            case "UB":
                                story.CurrentStage = JourneyStage.UltimateBoon;
                                break;
                            case "ROR":
                                story.CurrentStage = JourneyStage.RefusalOfReturn;
                                break;
                            case "MF":
                                story.CurrentStage = JourneyStage.MagicFlight;
                                break;
                            case "RFW":
                                story.CurrentStage = JourneyStage.RescueFromWithout;
                                break;
                            case "CRT":
                                story.CurrentStage = JourneyStage.CrossingReturnThreshhold;
                                break;
                            case "MOTW":
                                story.CurrentStage = JourneyStage.MasterOfTwoWorlds;
                                break;
                            case "FTL":
                                story.CurrentStage = JourneyStage.FreedomToLive;
                                break;
                        }
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
