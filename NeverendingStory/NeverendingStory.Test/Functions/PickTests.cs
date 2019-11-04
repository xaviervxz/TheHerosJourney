using NeverendingStory.Functions;
using NUnit.Framework;
using System.Collections.Generic;

namespace NeverendingStory.Test
{
    public class PickTests
    {
        [TestCase("Female", "Male", 10)]
        public void Pick_Random_FromTwoItemList_ContainsItem1<T>(T item1, T item2, int times)
        {
            var list = new T[] { item1, item2 };

            List<T> selections = new List<T>();
            for (int i = 0; i < times; i += 1)
            {
                var selection = Pick.Random(list);
                selections.Add(selection);
            }

            Assert.IsTrue(selections.Contains(item1));
        }

        [TestCase("Female", "Male", 10)]
        public void Pick_Random_FromTwoItemList_ContainsItem2<T>(T item1, T item2, int times)
        {
            var list = new T[] { item1, item2 };

            List<T> selections = new List<T>();
            for (int i = 0; i < times; i += 1)
            {
                var selection = Pick.Random(list);
                selections.Add(selection);
            }

            Assert.IsTrue(selections.Contains(item2));
        }
    }
}