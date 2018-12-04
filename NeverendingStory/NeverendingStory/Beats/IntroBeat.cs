namespace NeverendingStory.Beats
{
    public class IntroBeat : Beat
    {
        private readonly Character character;

        private readonly bool isYou;

        public IntroBeat(Character character, bool isYou = false)
        {
            this.character = character;
            this.isYou = isYou;
        }

        protected override string Text => $"{(isYou ? "You are" : character.Name + " is")} the {character.Intro}.";
    }
}