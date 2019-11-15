using NeverendingStory.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeverendingStory.Functions
{
    using Names = Dictionary<NameOrigin, Dictionary<Sex, string[]>>;

    public static class Pick
    {
        private static readonly Random rng = new Random();

        public static T Random<T>(T[] array)
        {
            if (array.Length == 0)
            {
                return default(T);
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

            bool SceneCanBeUsedHere(Scene s, JourneyStage currentStage)
            {
                bool sceneIsFilledOut = !string.IsNullOrWhiteSpace(s.Message);

                bool sceneMatches = s.Stage == currentStage;

                var conditions = s.Conditions.Split('|');
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

                    return false;
                }
                bool sceneConditionsAreMet = conditions.All(AreMet);

                return sceneConditionsAreMet && sceneMatches && sceneIsFilledOut && !s.Done && !s.IsSubStage;
            }


            var scene = scenes
                .Where(s => SceneCanBeUsedHere(s, story.CurrentStage))
                .OrderByDescending(s => s.Conditions.Length)
                .FirstOrDefault();

            return scene;
        }

        public static Character Character(
            Relationship relationship,
            IList<Character> characters,
            Names names)
        {
            var character = characters.FirstOrDefault(c => c.Relationship == relationship);

            if (character == null)
            {
                character = new Character
                {
                    Relationship = relationship
                };
                character.Name = Pick.Random(names[NameOrigin.Westron][character.SexEnum]);

                characters.Add(character);
            }

            return character;
        }

        public static JourneyStage NextStage(Story story)
        {
            JourneyStage nextStage;

            if (story.CurrentStage == JourneyStage.RoadOfTrials)
            {
                nextStage = story.CurrentStage;
                story.CurrentStageNumber += 1;

                if (story.CurrentStageNumber > 3)
                {
                    story.CurrentStageNumber = 1;
                }
            }
            else if (story.CurrentStage == JourneyStage.FreedomToLive)
            {
                return JourneyStage.CallToAdventure;
            }
            else
            {
                nextStage = story.CurrentStage + 1;
            }

            return nextStage;
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
