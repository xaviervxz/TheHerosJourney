using NUnit.Framework;
using TheHerosJourney.Functions;
using TheHerosJourney.Models;

namespace TheHerosJourney.Test.Functions
{
    [TestFixture]
    public class ConditionsTests
    {

        [Test]
        public void Conditions_IsMet_OccupationRelationshipCondition()
        {
            var storyWithRangerFriend = new Story();
            storyWithRangerFriend.Characters.Add(new Character
            {
                Occupation = Occupation.Ranger,
                Relationship = Relationship.Friend
            });
            const string rangerFriendCondition = "character:ranger:friend";

            bool storyHasRangerFriend = Condition.IsMet(storyWithRangerFriend, rangerFriendCondition);

            Assert.IsTrue(storyHasRangerFriend);
        }

        [TestCase(Sex.Female)]
        [TestCase(Sex.Male)]
        public void Conditions_IsMet_NamedCharacterSexConditionFailsIfSexIfWrong(Sex sex)
        {
            var storyWithForestGuardian = new Story();
            var forestGuardian = new Character
            {
                Sex = sex == Sex.Female ? Sex.Male : Sex.Female
            };
            storyWithForestGuardian.Characters.Add(forestGuardian);
            const string tag = "forestguardian";
            storyWithForestGuardian.NamedCharacters.Add(tag, forestGuardian);
            string forestGuardianSexCondition = $"character:{tag}:{sex.ToString().ToLower()}";

            bool storyHasForestGuardianWithMatchingSex = Condition.IsMet(storyWithForestGuardian, forestGuardianSexCondition);

            Assert.IsFalse(storyHasForestGuardianWithMatchingSex);
        }

        [TestCase(Sex.Female)]
        [TestCase(Sex.Male)]
        public void Conditions_IsMet_NamedCharacterSexCondition(Sex sex)
        {
            var storyWithForestGuardian = new Story();
            var forestGuardian = new Character
            {
                Sex = sex
            };
            storyWithForestGuardian.Characters.Add(forestGuardian);
            const string tag = "forestguardian";
            storyWithForestGuardian.NamedCharacters.Add(tag, forestGuardian);
            string forestGuardianSexCondition = $"character:{tag}:{sex.ToString().ToLower()}";

            bool storyHasForestGuardianWithMatchingSex = Condition.IsMet(storyWithForestGuardian, forestGuardianSexCondition);

            Assert.IsTrue(storyHasForestGuardianWithMatchingSex);
        }
    }
}
