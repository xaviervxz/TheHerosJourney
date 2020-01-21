using UnityEngine;
using UnityEngine.UI;

namespace Assets.MonoBehaviours
{
    public class StorySpeedChanger : MonoBehaviour
    {
        [SerializeField]
#pragma warning disable 0649
        private Sprite slowSprite;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private Sprite mediumSprite;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private Sprite fastSprite;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private Image speedImage;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private StoryScroll storyScroll;
#pragma warning restore 0649

        public StorySpeed startingStorySpeed;

        private StorySpeed currentStorySpeed;

        // Start is called before the first frame update
        private void Start()
        {
            SetStorySpeed(startingStorySpeed);
        }

        public void IncrementStorySpeed()
        {
            switch (currentStorySpeed)
            {
                case StorySpeed.Slow:
                default:
                    currentStorySpeed = StorySpeed.Medium;
                    break;
                case StorySpeed.Medium:
                    currentStorySpeed = StorySpeed.Fast;
                    break;
                case StorySpeed.Fast:
                    currentStorySpeed = StorySpeed.Slow;
                    break;
            }

            SetStorySpeed(currentStorySpeed);
        }

        private void SetStorySpeed(StorySpeed storySpeed)
        {
            currentStorySpeed = storySpeed;
            switch (currentStorySpeed)
            {
                case StorySpeed.Slow:
                default:
                    speedImage.sprite = slowSprite;
                    break;
                case StorySpeed.Medium:
                    speedImage.sprite = mediumSprite;
                    break;
                case StorySpeed.Fast:
                    speedImage.sprite = fastSprite;
                    break;
            }

            storyScroll.SetLettersPerSecond(currentStorySpeed);
        }
    }

    // RESEARCH NOTES:
    // Average letters in an English word: 5-6 (or 6)
    // Average adult reading speed: 200-250 wpm (words per minute)
    // Average public speaking speed: 160 wpm
    // Average speed reading speed: 600-1000 wpm

    public enum StorySpeed : int
    {
        Slow = 16, // 160 wpm * 6 lpw (letters per word) / 60 seconds per minute == 16 lps
        Medium = 25, // 250 wpm * 6 lpw / 60 spm == 25 lps
        Fast = 60 // 600 wpm * 6 lpw / 60 spm == 60 lps
    }
}