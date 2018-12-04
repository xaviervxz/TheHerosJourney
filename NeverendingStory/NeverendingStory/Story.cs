using NeverendingStory.Beats;

namespace NeverendingStory
{
    public class Story
    {
        private readonly Beat currentBeat;

        private readonly Character protagonist;

        public Story()
        {
            protagonist = new Character();

            currentBeat = new IntroBeat(protagonist, isYou: true);
        }

        public bool IsNextPart => currentBeat?.BeatTold != true;

        public string GetNextPart()
        {
            return currentBeat?.TellBeat();
        }
    }
}