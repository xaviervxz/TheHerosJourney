using NeverendingStory.Functions;
using NUnit.Framework;

namespace NeverendingStory.Test.Functions
{
    [TestFixture]
    public class ProcessTests
    {
        [TestCase("Breath of the Wild", "Breath of the Wild")]
        [TestCase("the Breath of the Wild", "The Breath of the Wild")]
        [TestCase("the Carrock", "The Carrock")]
        [TestCase("the Misty Mountains", "The Misty Mountains")]
        public void Process_ToTitleCase_CapitalizesJustFirstLetter(string input, string expectedOutput)
        {

            string output = Process.ToTitleCase(input);

            Assert.AreEqual(expectedOutput, output);
        }
    }
}
