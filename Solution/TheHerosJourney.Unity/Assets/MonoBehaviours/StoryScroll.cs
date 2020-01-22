using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.MonoBehaviours
{
    public class StoryScroll : MonoBehaviour
    {
        public float scrollSpeed = 5F;

        [SerializeField]
#pragma warning disable 0649
        private GameUi instanceGameUi;
#pragma warning restore 0649

        private void Start()
        {
            instanceGameUi.storyText.text = "";
        }

        private void Update()
        {
            // IF WE'RE DONE REVEALING TEXT,
            // CHECK FOR INPUT.
            if (!instanceGameUi.stillRevealingText)
            {
                // CHECK FOR THE "ENTER" KEYPRESS
                if (Input.GetButton("Submit") && !instanceGameUi.feedbackFormParent.isActiveAndEnabled)
                {
                    SkipToChoice();
                }
                
                // SCROLL IF THEY USE THE SCROLL WHEEL.
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
                if (!Mathf.Approximately(scrollAmount, 0))
                {
                    StopCoroutine("ScrollToSmooth");

                    var currentY = instanceGameUi.scrollContainer.anchoredPosition.y;
                    ScrollToNow(instanceGameUi, currentY - scrollAmount * scrollSpeed * 10 * (instanceGameUi.storyText.fontSize + 4));
                }
            }

            // ***********************
            // FADE IN A BUNCH OF LETTERS IF WE NEED TO.
            // ***********************

            // START FADING IN THE NEXT LETTER, IF WE AREN'T AT THE END.
            instanceGameUi.currentCharacterIndex += instanceGameUi.lettersPerSecond * Time.deltaTime;
            instanceGameUi.currentCharacterIndex = Mathf.Min(
                    instanceGameUi.currentCharacterIndex,
                    instanceGameUi.storyText.textInfo.characterCount
                );
            int intCurrentCharacterIndex = Math.Min(
                    Mathf.FloorToInt(instanceGameUi.currentCharacterIndex),
                    instanceGameUi.storyText.textInfo.characterCount
                );

            instanceGameUi.stillRevealingText = intCurrentCharacterIndex < instanceGameUi.storyText.textInfo.characterCount;

            instanceGameUi.storyText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        internal static bool GetChoicesShowing(GameUi gameUi)
        {
            if (gameUi.stillRevealingText)
            {
                return false;
            }

            // AHHH the offset here (the -1 part in lineCount - 1) is kind of fragile, unfortunately.
            // You've been warned.
            int bottomLine = Math.Max(0, gameUi.storyText.textInfo.lineCount - 1);
            bool lastLineIsHighEnough = gameUi.scrollContainer.anchoredPosition.y > ScrollYForLine(gameUi, bottomLine, Line.AtBottom);

            return lastLineIsHighEnough;
        }

        // ************
        // TEXT
        // ************

        private static IEnumerator FadeInLetter(GameUi gameUi, int characterIndex)
        {
            TMP_CharacterInfo letter = gameUi.storyText.textInfo.characterInfo[characterIndex];

            if (!letter.isVisible)
            {
                yield break;
            }

            // Get the index of the material used by the current character.
            int materialIndex = letter.materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            var newVertexColors = gameUi.storyText.textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = letter.vertexIndex;

            // MAKE CLEAR TO START.

            var color = newVertexColors[vertexIndex + 0];
            color.a = 0;

            newVertexColors[vertexIndex + 0] = color;
            newVertexColors[vertexIndex + 1] = color;
            newVertexColors[vertexIndex + 2] = color;
            newVertexColors[vertexIndex + 3] = color;

            // NOTE TO FUTURE SELF:
            // NEVER call UpdateVertexData in this function.
            // It makes the scrolling lag a ton if you skip forward about 4-5 choices in.
            // NEVER CALL THIS storyText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            do
            {
                yield return null;
            }
            while (characterIndex >= gameUi.currentCharacterIndex);

            // FADE IN AND MOVE DOWN.

            // TODO: REPLACE 3 BELOW WITH AN ACTUAL CALCULATED letterFadeInDuration VARIABLE
            var alphaPerSecond = 255F / (3F / gameUi.lettersPerSecond);

            while (color.a < 255)
            {
                int previousAlpha = color.a;
                color.a += (byte)Mathf.RoundToInt(alphaPerSecond * Time.deltaTime);
                if (previousAlpha > color.a)
                {
                    // We've looped around, break out of this loop.
                    break;
                }

                newVertexColors[vertexIndex + 0] = color;
                newVertexColors[vertexIndex + 1] = color;
                newVertexColors[vertexIndex + 2] = color;
                newVertexColors[vertexIndex + 3] = color;

                yield return null;
            }

            // SET BACK TO ORIGINAL COLOR TO END.

            color.a = 255;

            newVertexColors[vertexIndex + 0] = color;
            newVertexColors[vertexIndex + 1] = color;
            newVertexColors[vertexIndex + 2] = color;
            newVertexColors[vertexIndex + 3] = color;

            yield return null;
        }

        private struct FadeInLetterData
        {
            internal TMP_CharacterInfo letter;
            internal int index;
            public override string ToString()
            {
                return letter.character.ToString();
            }
        }

        internal static void AddText(GameUi gameUi, params string[] newParagraphs)
        {
            gameUi.stillRevealingText = true;

            string twoBlankLines = Environment.NewLine + Environment.NewLine;

            string newText = string.Join(twoBlankLines, newParagraphs);

            if (!string.IsNullOrWhiteSpace(gameUi.storyText.text))
            {
                gameUi.storyText.text += twoBlankLines;
            }

            int oldCharacterCount = gameUi.storyText.textInfo.characterCount;

            gameUi.storyText.text += newText;
            gameUi.storyText.ForceMeshUpdate();

            // START FADING IN **ALL** THE NEW LETTERS.
            // TO START, THIS HIDES THEM, HOPEFULLY BEFORE THE NEXT UPDATE.
            var newLetterIndexes = gameUi.storyText.textInfo.characterInfo
                .Select((characterInfo, index) => index)
                // NOTE: You HAVE to Select BEFORE you Skip on this command.
                // If you Skip first, all of the indexes will be off. :/
                .Skip(oldCharacterCount)
                .Take(gameUi.storyText.textInfo.characterCount - oldCharacterCount)
                .ToArray();

            foreach (var newLetterIndex in newLetterIndexes)
            {
                gameUi.StartCoroutine(FadeInLetter(gameUi, newLetterIndex));
            }
        }

        internal static string GetScrollText(GameUi gameUi)
        {
            return gameUi.storyText.text;
        }

        internal static void ContinueRevealing(GameUi gameUi)
        {
            //ScrollToNow(storyText.textInfo.lineCount - 2, Line.AtTop);
        }

        // ************
        // INTERACTING
        // ************

        public void SkipToChoice()
        {
            instanceGameUi.currentCharacterIndex = instanceGameUi.storyText.textInfo.characterCount;
        }

        // ************
        // SCROLLING
        // ************

        private static float ScrollYForLine(GameUi gameUi, int lineNumber, Line linePos)
        {
            // EDGE CASE FOR THE FIRST LINE,
            // TO SHOW OFF THE TOP OF THE PARCHMENT SCROLL.
            if (lineNumber <= 0 && linePos == Line.AtTop)
            {
                return 0;
            }

            float lineHeight =
                (gameUi.storyText.font.faceInfo.lineHeight + gameUi.storyText.lineSpacing)
                /
                (gameUi.storyText.font.faceInfo.pointSize / gameUi.storyText.fontSize);
            var scrollY = (lineNumber - 1) * lineHeight + 45;

            if (linePos == Line.AtBottom)
            {
                float parentsHeight = gameUi.storyText.rectTransform.rect.height;
                scrollY -= (parentsHeight - gameUi.storyText.margin.w);
            }

            var storyTextOffset = gameUi.storyText.rectTransform.anchoredPosition.y;
            return scrollY - storyTextOffset;
        }

        private static void ScrollToNow(GameUi gameUi, int lineNumber, Line linePos)
        {
            var newY = ScrollYForLine(gameUi, lineNumber, linePos);

            ScrollToNow(gameUi, newY);
        }

        private static void ScrollToNow(GameUi gameUi, float newScrollY)
        {
            int bottomLine = Math.Max(0, gameUi.storyText.textInfo.lineCount - 3);
            float scrollYForBottomLine = ScrollYForLine(gameUi, bottomLine, Line.AtTop);

            if (scrollYForBottomLine > 0 && newScrollY > scrollYForBottomLine)
            {
                newScrollY = scrollYForBottomLine;
            }

            if (newScrollY < 0)
            {
                newScrollY = 0;
            }

            gameUi.scrollContainer.anchoredPosition = new Vector2(gameUi.scrollContainer.anchoredPosition.x, newScrollY);
        }

        internal static void ScrollToSmooth(GameUi gameUi, int lineNumber, Line linePos)
        {
            var targetScrollY = ScrollYForLine(gameUi, lineNumber, linePos);

            gameUi.StartCoroutine(ScrollToSmooth(gameUi, targetScrollY));
        }

        private static IEnumerator ScrollToSmooth(GameUi gameUi, float targetScrollY)
        {
            gameUi.StopCoroutine("ScrollToSmooth");

            float startingY = gameUi.scrollContainer.anchoredPosition.y;

            float startingTime = Time.time;

            const float secondsToScrollFor = 1F;

            float currentY = startingY;
            while (!Mathf.Approximately(currentY, targetScrollY))
            {
                currentY = Mathf.SmoothStep(startingY, targetScrollY, (Time.time - startingTime) / secondsToScrollFor);

                ScrollToNow(gameUi, currentY);

                yield return null;
            }

            ScrollToNow(gameUi, targetScrollY);
        }

        /// <summary>
        /// This function is public so that the "Scroll to End" button can call it directly.
        /// </summary>
        public void ScrollToEnd()
        {
            int currentLineCount = instanceGameUi.storyText.textInfo.lineCount;
            var lastLineScrollY = ScrollYForLine(instanceGameUi, currentLineCount, Line.AtBottom);

            // TODO: Figure out what to put here instead of "0"!
            if (lastLineScrollY > instanceGameUi.scrollContainer.anchoredPosition.y)
            {
                ScrollToSmooth(instanceGameUi, currentLineCount, Line.AtBottom);
            }
        }
    }

    internal enum Line
    {
        AtTop,
        AtBottom
    }
}
