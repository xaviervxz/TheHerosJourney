using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.MonoBehaviours
{
    public class StoryScroll : MonoBehaviour
    {
        public float scrollSpeed = 5F;

        private int lettersPerSecond = 25;

        private bool isWaitingForStory = true;

        private bool skipToChoice = true;

        [SerializeField]
#pragma warning disable 0649
        private GameUi gameUi;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private TextMeshProUGUI storyText;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private RectTransform storyContainer;
#pragma warning restore 0649

        private void Start()
        {
            storyText.text = "";
            isWaitingForStory = true;
        }

        // Update is called once per frame
        void Update()
        {
            // ***********************
            // WAITING FOR STORY MODE,
            // AFTER THEY CHOOSE.
            // ***********************

            if (isWaitingForStory)
            {
                gameUi.scrollToEndButton.SetActive(false);

                // CHECK FOR THE "ENTER" KEYPRESS
                if (Input.GetButton("Submit") && !gameUi.feedbackFormParent.isActiveAndEnabled)
                {
                    SkipToChoice();
                }
            }

            // CHANGE THE TARGET SCROLL Y IF THE SCROLL WHEEL WAS USED.
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
            if (!Mathf.Approximately(scrollAmount, 0))
            {
                StopCoroutine("ScrollToSmooth");

                var currentY = storyContainer.anchoredPosition.y;
                ScrollToNow(currentY - scrollAmount * scrollSpeed * 10 * (storyText.fontSize + 4));
                var targetScrollY = storyContainer.anchoredPosition.y;
            }
        }

        internal bool GetChoicesShowing()
        {
            return true;
            // int bottomLine = Math.Max(0, storyText.textInfo.lineCount - 3);
            //storyContainer.anchoredPosition.y > ScrollYForLine(bottomLine, Line.AtBottom)
        }

        // ************
        // TEXT
        // ************

        internal void AddText(params string[] newParagraphs)
        {
            string twoBlankLines = Environment.NewLine + Environment.NewLine;

            string newText = string.Join(twoBlankLines, newParagraphs);

            if (!string.IsNullOrWhiteSpace(storyText.text))
            {
                storyText.text += twoBlankLines;
            }

            storyText.text += newText;

            // TODO: Get rid of this line and move it somewhere it actually belongs.
            ScrollToEnd();
        }

        internal string GetText()
        {
            return storyText.text;
        }

        internal void ContinueRevealing()
        {
            // TODO: Get rid of this line and move it somewhere it actually belongs.
            ScrollToEnd();
        }

        // ************
        // ANIMATING
        // ************

        public void SetLettersPerSecond(StorySpeed newLettersPerSecond)
        {
            lettersPerSecond = (int)newLettersPerSecond;
        }

        // ************
        // INTERACTING
        // ************

        public void SkipToChoice()
        {
            if (isWaitingForStory)
            {
                skipToChoice = true;
            }
        }

        // ************
        // SCROLLING
        // ************

        private enum Line
        {
            AtTop,
            AtBottom
        }

        private float ScrollYForLine(int lineNumber, Line linePos)
        {
            // EDGE CASE FOR THE FIRST LINE,
            // TO SHOW OFF THE TOP OF THE PARCHMENT SCROLL.
            if (lineNumber <= 0 && linePos == Line.AtTop)
            {
                return 0;
            }

            float lineHeight = (storyText.font.faceInfo.lineHeight + storyText.lineSpacing) / (storyText.font.faceInfo.pointSize / storyText.fontSize);
            var scrollY = (lineNumber - 1) * lineHeight + 45;

            if (linePos == Line.AtBottom)
            {
                float parentsHeight = storyText.rectTransform.rect.height;
                scrollY -= (parentsHeight - storyText.margin.w);
            }

            var storyTextOffset = storyText.rectTransform.anchoredPosition.y;
            return scrollY - storyTextOffset;
        }

        private void ScrollToNow(float newScrollY)
        {
            int bottomLine = Math.Max(0, storyText.textInfo.lineCount - 3);
            float scrollYForBottomLine = ScrollYForLine(bottomLine, Line.AtTop);

            if (scrollYForBottomLine > 0 && newScrollY > scrollYForBottomLine)
            {
                newScrollY = scrollYForBottomLine;
            }

            if (newScrollY < 0)
            {
                newScrollY = 0;
            }

            storyContainer.anchoredPosition = new Vector2(storyContainer.anchoredPosition.x, newScrollY);
        }

        private IEnumerator ScrollToSmooth(int lineNumber, Line linePos)
        {
            StopCoroutine("ScrollToSmooth");

            var targetScrollY = ScrollYForLine(lineNumber, linePos);

            float startingY = storyContainer.anchoredPosition.y;

            float startingTime = Time.time;

            const float secondsToScrollFor = 1F;

            float currentY = startingY;
            while (!Mathf.Approximately(currentY, targetScrollY))
            {
                currentY = Mathf.SmoothStep(startingY, targetScrollY, (Time.time - startingTime) / secondsToScrollFor);

                ScrollToNow(currentY);

                yield return null;
            }

            ScrollToNow(targetScrollY);
        }

        /// <summary>
        /// This function is public so that the "Scroll to End" button can call it directly.
        /// </summary>
        public void ScrollToEnd()
        {
            var lastLineScrollY = ScrollYForLine(storyText.textInfo.lineCount, Line.AtBottom);

            // TODO: Figure out what to put here instead of "0"!
            if (lastLineScrollY > 0)//targetScrollY)
            {
                StartCoroutine(ScrollToSmooth(storyText.textInfo.lineCount, Line.AtBottom));
            }
        }
    }
}
