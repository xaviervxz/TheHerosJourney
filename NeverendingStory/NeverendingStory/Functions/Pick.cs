﻿using NeverendingStory.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeverendingStory.Functions
{
    using CharacterData = Dictionary<PeopleNameOrigin, Dictionary<Sex, string[]>>;

    public static class Pick
    {
        private static readonly Random rng = new Random();
        public static T Random<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                return default;
            }

            var array = list.ToArray();

            if (array.Length == 0)
            {
                return default;
            }

            int randomIndex = rng.Next(0, array.Length);

            return array[randomIndex];
        }

        public static Scene NextScene(Scene[] scenes, Story story)
        {
            if (story.NextSceneIdentifier != null)
            {
                string nextSceneIdentifier = story.NextSceneIdentifier;
                story.NextSceneIdentifier = null;

                var nextScene = scenes.FirstOrDefault(s => s.Identifier == nextSceneIdentifier);

                if (nextScene != null)
                {
                    return nextScene;
                }
                else if (!string.IsNullOrWhiteSpace(nextSceneIdentifier))
                {
                    var nextStage = Pick.StageFromCode(nextSceneIdentifier);

                    if (nextStage != null)
                    {
                        story.CurrentStage = nextStage.Value;
                    }
                }
            }
            else
            {
                JourneyStage NextStage(Story s)
                {
                    JourneyStage nextStage;

                    if (s.CurrentStage == JourneyStage.RoadOfTrials)
                    {
                        nextStage = s.CurrentStage;
                        s.CurrentStageNumber += 1;

                        if (s.CurrentStageNumber > 3)
                        {
                            s.CurrentStageNumber = 1;
                            nextStage = s.CurrentStage + 1;
                        }
                    }
                    else if (s.CurrentStage == JourneyStage.FreedomToLive)
                    {
                        return JourneyStage.FreedomToLive;
                    }
                    else if (story.CurrentStageNumber > 0)
                    {
                        nextStage = s.CurrentStage + 1;
                    }
                    else
                    {
                        s.CurrentStageNumber = 1;

                        nextStage = s.CurrentStage;
                    }

                    return nextStage;
                }

                story.CurrentStage = NextStage(story);
            }

            bool SceneCanBeUsedHere(Scene s, JourneyStage currentStage)
            {
                bool sceneIsFilledOut = !string.IsNullOrWhiteSpace(s.Message);

                bool sceneMatches = s.Stage == currentStage;

                var conditions = s.Conditions.Split('&');
                bool AreMet(string condition)
                {
                    var conditionPieces = condition.Split(':');

                    if (conditionPieces.Length == 0 || string.IsNullOrWhiteSpace(conditionPieces[0]))
                    {
                        return true;
                    }

                    if (conditionPieces[0] == "item" && conditionPieces.Length == 2)
                    {
                        bool haveItem = story.You.Inventory.Any(i => i.Identifier == conditionPieces[1]);

                        return haveItem;
                    }

                    if (conditionPieces[0] == "character" && conditionPieces.Length == 2)
                    {
                        bool hasNamedCharacter = story.NamedCharacters.ContainsKey(conditionPieces[1]);

                        return hasNamedCharacter;
                    }

                    if (conditionPieces[0] == "location" && conditionPieces.Length == 3 && conditionPieces[1] == "current")
                    {
                        bool isInLocation = story.You.CurrentLocation.Type.ToString().ToLower() == conditionPieces[2];

                        return isInLocation;
                    }

                    bool doesFlagExist = story.Flags.TryGetValue(conditionPieces[0], out string value);

                    if (doesFlagExist && conditionPieces.Length >= 2)
                    {
                        return value == conditionPieces[1];
                    }

                    return false;
                }
                bool sceneConditionsAreMet = conditions.All(AreMet);

                return sceneConditionsAreMet && sceneMatches && sceneIsFilledOut && !s.Done && !s.IsSubStage;
            }

            
            // RANDOMLY PICK A NEW SCENE
            var scene = scenes
                .Where(s => SceneCanBeUsedHere(s, story.CurrentStage))

                // The idea of this next three lines is that the Scenes would be
                // select randomly from those scenes that have the most
                // conditions (i.e. requires Baron and Ranger would be 2).
                .GroupBy(s => s.Conditions.Split('&').Length)
                .OrderByDescending(s => s.Key)
                .FirstOrDefault()
                
                .Random();

            if (scene == null && story.CurrentStage != JourneyStage.FreedomToLive)
            {
                scene = NextScene(scenes, story);
            }

            return scene;
        }

        public static Character Character(
            Relationship relationship,
            IList<Character> characters,
            CharacterData names)
        {
            var character = characters.FirstOrDefault(c => c.Relationship == relationship);

            if (character == null)
            {
                character = new Character
                {
                    Relationship = relationship
                };
                character.Name = Pick.Random(names[PeopleNameOrigin.Westron][character.Sex].Except(characters.Select(c => c.Name)).ToArray());

                characters.Add(character);
            }

            return character;
        }

        public static Town Town(
            List<Location> locations,
            FileData data)
        {
            Town town = locations.FirstOrDefault(c => c.Type == LocationType.Town) as Town;

            if (town == null)
            {
                // PICK A RANDOM TOWN
                var townTemplate = data.LocationData.Towns.Random();

                // CREATE THE TOWN WITH ITS NAME
                town = new Town
                {
                    Name = townTemplate.Name
                };

                // GENERATE A MAIN FEATURE
                var feature = data.LocationData.MainFeatures[townTemplate.MainFeature];

                var featureLocations = new List<Location>();
                foreach (var type in feature.Types)
                {
                    var location = Pick.Location(type, locations.Except(featureLocations).ToList(), data);

                    featureLocations.Add(location);
                }
                locations.AddRange(featureLocations.Except(locations));

                town.MainFeature = new Feature
                {
                    Locations = featureLocations.ToArray(),
                    RelativePosition = feature.RelativePosition
                };

                if (town.MainFeature.Locations.Length > 0)
                {
                    town.MainFeature.RelativePosition = town.MainFeature.RelativePosition
                        .Replace("{name}", town.MainFeature.Locations[0].NameWithThe)
                        .Replace("{name1}", town.MainFeature.Locations[0].NameWithThe);

                    if (town.MainFeature.Locations.Length > 1)
                    {
                        town.MainFeature.RelativePosition = town.MainFeature.RelativePosition
                            .Replace("{name2}", town.MainFeature.Locations[1].NameWithThe);
                    }
                }

                // PICK AN INDUSTRY
                town.MainIndustry = townTemplate.Industry;
                town.MainIndustryData = data.LocationData.Industries[town.MainIndustry];

                // ADD THE TOWN TO THE LIST OF LOCATIONS
                locations.Add(town);
            }

            return town as Town;
        }

        public static Location Location(
            LocationType type,
            IList<Location> locations,
            FileData data)
        {
            var location = locations.FirstOrDefault(c => c.Type == type);

            if (location == null)
            {
                string terrain = data.LocationData.Names.Terrain[type].SpecificTypes.Random();
                string adjective = data.LocationData.Names.Adjectives.Random();
                string noun = data.LocationData.Names.Nouns.Random();
                string personName = Pick.Random(data.CharacterData[PeopleNameOrigin.Westron][new[] { Sex.Female, Sex.Male }.Random()].ToArray());
                string format = data.LocationData.Names.Terrain[type].Formats.Random();

                string name = format
                    .Replace("{terrain}", terrain)
                    .Replace("{adjective}", adjective)
                    .Replace("{noun}", noun)
                    .Replace("{name}", personName);

                const string the = "the ";
                bool hasThe = false;
                if (name.StartsWith(the))
                {
                    name = name.Substring(the.Length);
                    hasThe = true;
                }

                location = new Location
                {
                    Name = name,
                    HasThe = hasThe,
                    SpecificType = terrain.ToLower(),
                    Type = type
                };

                locations.Add(location);
            }

            return location;
        }

        private static Dictionary<string, JourneyStage> stageCodes = new Dictionary<string, JourneyStage>
        {
            { "CTA", JourneyStage.CallToAdventure },
            { "ROC", JourneyStage.RefusalOfCall },
            { "MTM", JourneyStage.MeetingTheMentor },
            { "CTT", JourneyStage.CrossingTheThreshhold },
            { "BOTW", JourneyStage.BellyOfTheWhale },
            { "ROT", JourneyStage.RoadOfTrials },
            { "MWG", JourneyStage.MeetingWithGoddess },
            { "WAT", JourneyStage.WomanAsTemptress },
            { "AWF", JourneyStage.AtonementWithFather },
            { "A", JourneyStage.Apotheosis },
            { "UB", JourneyStage.UltimateBoon },
            { "ROR", JourneyStage.RefusalOfReturn },
            { "MF", JourneyStage.MagicFlight },
            { "RFW", JourneyStage.RescueFromWithout },
            { "CRT", JourneyStage.CrossingReturnThreshhold },
            { "MOTW", JourneyStage.MasterOfTwoWorlds },
            { "FTL", JourneyStage.FreedomToLive }
        };

        public static JourneyStage? StageFromCode(string code)
        {
            var matchingStage = stageCodes.FirstOrDefault(c => code.StartsWith(c.Key));

            if (matchingStage.Equals(default(KeyValuePair<string, JourneyStage>)))
            {
                return null;
            }

            return matchingStage.Value;
        }
    }
}
