using NeverendingStory.Models;
using System;
using System.Linq;

namespace NeverendingStory.Functions
{
    internal static class Condition
    {
        internal static bool IsMet(Story story, string condition)
        {
            // IF THIS CONDITION CONTAINS MULTIPLE, ORed CONDITIONS,
            // RETURN TRUE IF ANY OF THOSE ARE TRUE.
            {
                var separateConditions = condition.Split('|');
                if (separateConditions.Length > 1)
                {
                    return separateConditions.Any(c => IsMet(story, c));
                }
            }

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

            if (conditionPieces[0] == "noitem" && conditionPieces.Length == 2)
            {
                bool doesNotHaveItem = story.You.Inventory.All(i => i.Identifier != conditionPieces[1]);

                return doesNotHaveItem;
            }

            if (conditionPieces[0] == "character" && conditionPieces.Length == 2)
            {
                bool namedCharacterExists = story.NamedCharacters.ContainsKey(conditionPieces[1]);

                return namedCharacterExists;
            }

            if (conditionPieces[0] == "nocharacter" && conditionPieces.Length == 2)
            {
                bool namedCharacterExists = story.NamedCharacters.ContainsKey(conditionPieces[1]);

                return !namedCharacterExists;
            }

            if (conditionPieces[0] == "location" && conditionPieces.Length == 3)
            {
                Location location;

                // GET THE LOCATION WE'RE LOOKING AT
                if (conditionPieces[1] == "current")
                {
                    location = story.You.CurrentLocation;
                }
                else
                {
                    story.NamedLocations.TryGetValue(conditionPieces[1], out location);
                }

                // FIGURE OUT WHAT WE WANT TO KNOW ABOUT THAT LOCATION
                bool isInLocation = false;

                bool isLocationType = Enum.TryParse(conditionPieces[2].ToTitleCase(), out LocationType locationType);

                if (isLocationType)
                {
                    isInLocation = location.Type == locationType;
                }
                else
                {
                    if (conditionPieces[2] == "hometown")
                    {
                        isInLocation = location == story.You.Hometown;
                    }
                    else if (conditionPieces[2] == "goal")
                    {
                        isInLocation = location == story.You.Goal;
                    }
                    else
                    {
                        story.NamedLocations.TryGetValue(conditionPieces[2], out Location namedLocation);

                        isInLocation = location == namedLocation;
                    }
                }

                return isInLocation;
            }

            if (conditionPieces[0] == "goal" && conditionPieces.Length == 2)
            {
                bool goalIsAsDescribed = false;

                if (conditionPieces[1] == "hometown")
                {
                    goalIsAsDescribed = story.You.Goal == story.You.Hometown;
                }
                else
                {
                    if (story.NamedLocations.TryGetValue(conditionPieces[1], out Location namedLocation))
                    {
                        goalIsAsDescribed = story.You.Goal == namedLocation;
                    }
                }

                return goalIsAsDescribed;
            }

            bool doesFlagExist = story.Flags.TryGetValue(conditionPieces[0], out string value);
            if (!doesFlagExist)
            {
                value = "false";
            }

            return conditionPieces.Length >= 2 && value == conditionPieces[1];
        }
    }
}
