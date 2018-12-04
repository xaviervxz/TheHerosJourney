namespace NeverendingStory
{
    public class Character
    {
        public Character()
        {
            Name = "Peter";
            Intro = "second son of the second son of the King";
        }

        public string Intro { get; }

        public string Name { get; private set; }
    }
}