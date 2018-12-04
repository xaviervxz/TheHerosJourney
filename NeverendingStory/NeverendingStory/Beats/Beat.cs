namespace NeverendingStory.Beats
{
    public abstract class Beat
    {
        public bool BeatTold { get; private set; } = false;

        public string TellBeat()
        {
            BeatTold = true;

            return Text;
        }

        protected abstract string Text { get; }
    }
}