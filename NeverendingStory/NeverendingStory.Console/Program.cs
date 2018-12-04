using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverendingStory.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var story = new Story();

            while (story.IsNextPart)
            {
                var nextPart = story.GetNextPart();

                System.Console.Write(nextPart);
                System.Console.ReadLine();
            }
        }
    }
}
