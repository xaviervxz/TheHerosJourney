using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class FadeInText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    const string sentence = "The quick brown fox jumps over the lazy dog. ";
    const int lettersPerSecond = 60;
    const float letterFadeInDuration = 30F;

    private void Start()
    {
        textMesh.text = sentence;
    }

    private static int currentCharacterInt = 0;

    public void AddLetter()
    {
        IEnumerator WriteToTextMesh(string text)
        {
            IEnumerator FadeInLetter(TMP_CharacterInfo letter, int characterIndex)
            {
                if (!letter.isVisible)
                {
                    yield break;
                }

                // Get the index of the material used by the current character.
                int materialIndex = letter.materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                var newVertexColors = textMesh.textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = letter.vertexIndex;

                // MAKE CLEAR TO START.

                var color = newVertexColors[vertexIndex + 0];
                color.a = 0;
                //yield return null;

                newVertexColors[vertexIndex + 0] = color;
                newVertexColors[vertexIndex + 1] = color;
                newVertexColors[vertexIndex + 2] = color;
                newVertexColors[vertexIndex + 3] = color;

                // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                //Debug.Log("Color BEFORE: " + newVertexColors[vertexIndex].a);

                //yield return new WaitForEndOfFrame();

                //Debug.Log("Color AFTER: " + newVertexColors[vertexIndex].a);
                do
                {
                    newVertexColors[vertexIndex + 0] = color;
                    newVertexColors[vertexIndex + 1] = color;
                    newVertexColors[vertexIndex + 2] = color;
                    newVertexColors[vertexIndex + 3] = color;

                    yield return null;
                }
                while (characterIndex >= currentCharacterInt);
                    //&& isWaiting);

                // FADE IN AND MOVE DOWN.

                var alphaPerSecond = 255F / (letterFadeInDuration / lettersPerSecond);

                while (color.a < 255)
                {
                    int previousAlpha = color.a;
                    color.a += (byte)Mathf.RoundToInt(alphaPerSecond * Time.deltaTime);
                    if (previousAlpha > color.a)
                    {
                        // We've looped around, break out.
                        break;
                    }

                    newVertexColors[vertexIndex + 0] = color;
                    newVertexColors[vertexIndex + 1] = color;
                    newVertexColors[vertexIndex + 2] = color;
                    newVertexColors[vertexIndex + 3] = color;

                    // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                    textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                    yield return null;
                }

                // SET BACK TO ORIGINAL COLOR TO END.

                color.a = 255;

                newVertexColors[vertexIndex + 0] = color;
                newVertexColors[vertexIndex + 1] = color;
                newVertexColors[vertexIndex + 2] = color;
                newVertexColors[vertexIndex + 3] = color;

                // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                yield return null;
            }

            // SCROLL DOWN TO THE END OF THE LAST LINE,
            // AND ADD THE NEW TEXT TO THE STORY.

            currentCharacterInt = textMesh.textInfo.characterCount;
            int oldLineCount = textMesh.textInfo.lineCount;

            // Note to future me: Do NOT depend on text.Length in this function.
            // The text variable has formatting info in it, which is NOT
            // counted in the textInfo.characterCount variable.
            textMesh.text += text;
            textMesh.ForceMeshUpdate(ignoreInactive: true);

            //yield return null; // DO NOT REMOVE THIS LINE. EVERYTHING BREAKS WITHOUT IT. P.S. I DESPERATELY NEED TO REMOVE THIS RIGHT NOW.

            // HIDE ALL THE NEW LETTERS

            var newLetters = textMesh.textInfo.characterInfo.Select((ci, index) => new { letter = ci, index }).Skip(currentCharacterInt).ToArray();
            foreach (var newLetter in newLetters)
            {
                StartCoroutine(FadeInLetter(newLetter.letter, newLetter.index));
            }

            //yield return null;

            //StartCoroutine(ScrollToSmooth(oldLineCount, Line.AtTop)); // This needs to be AFTER the new text is added, otherwise the Y-coordinate clamping screws this up because the text mesh hasn't increased its size yet.

            // FADE IN ALL NEW LETTERS ONE BY ONE

            float currentCharacterFloat = currentCharacterInt;
            int test = 0;
            while (currentCharacterInt < textMesh.textInfo.characterCount)
            {
                int previousCharacter = currentCharacterInt;
                currentCharacterFloat += lettersPerSecond * Time.deltaTime;

                //if (skipToChoice)
                //{
                //    currentCharacterFloat = storyText.textInfo.characterCount;
                //    skipToChoice = false;
                //}

                // Advancing this variable makes the fading in letters realize
                // it's their turn and they should just go for it.
                currentCharacterInt = Math.Min(Mathf.FloorToInt(currentCharacterFloat), textMesh.textInfo.characterCount);
                test = currentCharacterInt;

                yield return null;
            }

            //isWaiting = false;

            //ScrollToEnd();

            //// SCROLL UP, IF NECESSARY, TO REVEAL NEW TEXT.
            ////int currentCharacterIndex = Math.Min(storyText.textInfo.characterInfo.Length - 1, Math.Max(0, numCharactersRevealed));//storyTextMesh.maxVisibleCharacters - 1));
            //int currentLineNumber = storyText.textInfo.characterInfo[currentCharacterInt].lineNumber;

            //var currentLineScrollY = ScrollYForLine(currentLineNumber, Line.AtBottom);

            //if (currentLineScrollY > targetScrollY)
            //{
            //    ScrollToEnd();
            //}
        }

        StartCoroutine(WriteToTextMesh(Environment.NewLine + Environment.NewLine + string.Join(" ", Enumerable.Repeat(sentence, 10))));
    }
}
