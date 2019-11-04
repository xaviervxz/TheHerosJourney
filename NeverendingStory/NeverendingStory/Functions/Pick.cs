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
            bool SceneMatchesAndIsFilledOut(Scene s, JourneyStage currentStage)
            {
                bool sceneIsFilledOut = !string.IsNullOrWhiteSpace(s.Message)
                && !string.IsNullOrWhiteSpace(s.Choice1)
                && !string.IsNullOrWhiteSpace(s.Choice2)
                && !string.IsNullOrWhiteSpace(s.Outro1)
                && !string.IsNullOrWhiteSpace(s.Outro2);

                bool sceneMatches = s.Stage == currentStage;

                return sceneMatches && sceneIsFilledOut;    
            }


            var scene = Random(scenes.Where(s => SceneMatchesAndIsFilledOut(s, story.CurrentStage)).ToArray());

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
    }
}
