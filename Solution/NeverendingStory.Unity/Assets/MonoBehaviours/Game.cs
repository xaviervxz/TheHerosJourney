using NeverendingStory.Functions;
using NeverendingStory.Models;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public float buttonFadeInSeconds = 0.5F;
    public float buttonFadeOutSeconds = 0.1F;
    public int lettersPerSecond = 10;

    [SerializeField]
#pragma warning disable 0649
    private TextMeshProUGUI storyTextMesh;
#pragma warning restore 0649

    [SerializeField]
#pragma warning disable 0649
    private GameObject choice1Button;
#pragma warning restore 0649

    [SerializeField]
#pragma warning disable 0649
    private GameObject choice2Button;
#pragma warning restore 0649

    private static FileData FileData;
    private static Story Story;
    private static Scene currentScene = null;
    private static bool isWaiting = false;

    private static string newStoryText = "";
    private static string choice1Text = "";
    private static string choice2Text = "";
    private static float targetScrollY = 0;
    private static float prevTargetScrollY = 0;
    private static float startingScrollY = 0;
    private static float startingScrollTime;

    /// <summary>
    /// RESET GAMEOBJECTS. LOAD FILEDATA AND PICK A STORY. RUN THE FIRST SCENE.
    /// </summary>
    private void Start()
    {
        // RESET GAMEOBJECTS

        storyTextMesh.text = "";
        storyTextMesh.maxVisibleCharacters = 0;

        choice1Button.SetActive(false);
        choice2Button.SetActive(false);

        // RESET VARIABLES

        startingScrollTime = Time.time;

        // LOAD THE STORY

        void ShowLoadGameFilesError()
        {
            WriteMessage("");
            WriteMessage("Sorry, the Neverending Story couldn't load because it can't find the files it needs.");
            WriteMessage("First, make sure you're running the most current version.");
            WriteMessage("Then, if you are and this still happens, contact the developer and tell him to fix it.");
            WriteMessage("Thanks! <3");
        }

        (FileData, Story) = Run.LoadGame(ShowLoadGameFilesError);

        // TODO: Make a "New Game" page where you can enter this information.
        Story.You.Name = "Alex";
        Story.You.Sex = Sex.Male;

        RunNewScenes();
    }

    /// <summary>
    /// HANDLE SCROLLING.
    /// </summary>
    private void Update()
    {
        const float secondsToScrollFor = 1F;

        var storyTextRect = storyTextMesh.rectTransform;
        float currentY = storyTextRect.anchoredPosition.y;

        if (!Mathf.Approximately(targetScrollY, prevTargetScrollY))
        {
            startingScrollY = currentY;
            startingScrollTime = Time.time;

            prevTargetScrollY = targetScrollY;
        }

        float newY;
        if (!Mathf.Approximately(currentY, targetScrollY))
        {
            newY = Mathf.SmoothStep(startingScrollY, targetScrollY, (Time.time - startingScrollTime) / secondsToScrollFor);
        }
        else
        {
            newY = targetScrollY;
        }

        storyTextRect.anchoredPosition = new Vector2(storyTextRect.anchoredPosition.x, newY);
    }

    private float ScrollYForLine(int lineNumber, bool lineAtTop = true)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyTextMesh.rectTransform);
        float myHeight = storyTextMesh.rectTransform.rect.height;

        var scrollY = lineNumber * (storyTextMesh.fontSize + 4);
        if (!lineAtTop)
        {
            float parentsHeight = storyTextMesh.rectTransform.parent.GetComponent<RectTransform>().rect.height;
            scrollY -= (parentsHeight - storyTextMesh.margin.w - 40);
        }
        scrollY = Mathf.Clamp(scrollY, 0, myHeight - 80 - storyTextMesh.margin.w); // w = bottom margin

        return scrollY;
    }

    private void ScrollToLine(int lineNumber, bool lineAtTop = true)
    {
        targetScrollY = ScrollYForLine(lineNumber, lineAtTop);
    }

    private void RunNewScenes()
    {
        void PresentChoices(string choice1, string choice2)
        {
            choice1Text = choice1;
            choice2Text = choice2;
        }

        bool choicesExist = false;

        do
        {
            currentScene = Run.NewScene(FileData, Story, WriteMessage);

            choicesExist = Run.PresentChoices(FileData, Story, currentScene, PresentChoices, WriteMessage);

            WriteMessage("");
        }
        while (!choicesExist);

        WriteMessage("");

        IEnumerator WriteToStory(string text)
        {
            float parentsHeight = storyTextMesh.rectTransform.parent.GetComponent<RectTransform>().rect.height;

            // SCROLL DOWN TO THE END OF THE LAST LINE,
            // AND ADD THE NEW TEXT TO THE STORY.
            
            int oldLineCount = storyTextMesh.textInfo.lineCount;

            storyTextMesh.text += text;

            ScrollToLine(oldLineCount); // This needs to be AFTER, the new text is added, otherwise the clamping screws this up.

            // REVEAL MORE CHARACTERS

            float charactersRevealed = storyTextMesh.maxVisibleCharacters;

            while (storyTextMesh.maxVisibleCharacters < storyTextMesh.text.Length)
            {
                charactersRevealed += lettersPerSecond * Time.deltaTime;

                storyTextMesh.maxVisibleCharacters = Math.Min(
                    Mathf.FloorToInt(charactersRevealed),
                    storyTextMesh.text.Length);

                // SCROLL UP, IF NECESSARY, TO REVEAL NEW TEXT.
                int currentCharacterIndex = Math.Min(storyTextMesh.textInfo.characterInfo.Length - 1, Math.Max(0, storyTextMesh.maxVisibleCharacters - 1));
                int currentLineNumber = storyTextMesh.textInfo.characterInfo[currentCharacterIndex].lineNumber;

                var currentLineScrollY = ScrollYForLine(currentLineNumber, lineAtTop: false);

                if (currentLineScrollY > targetScrollY)
                {
                    ScrollToLine(currentLineNumber, lineAtTop: false);
                }

                yield return null;
            }

            isWaiting = false;

            var lastLineScrollY = ScrollYForLine(storyTextMesh.textInfo.lineCount - 1, lineAtTop: false);

            if (lastLineScrollY > targetScrollY)
            {
                ScrollToLine(storyTextMesh.textInfo.lineCount - 1, lineAtTop: false);
            }

            storyTextMesh.maxVisibleCharacters = storyTextMesh.text.Length;

            while (!Mathf.Approximately(targetScrollY, storyTextMesh.rectTransform.anchoredPosition.y))
            {
                yield return null;
            }

            StartCoroutine(FadeButton(choice1Button, choice1Text, buttonFadeInSeconds, fadeIn: true));
            StartCoroutine(FadeButton(choice2Button, choice2Text, buttonFadeInSeconds, fadeIn: true));
        }

        StartCoroutine(WriteToStory(newStoryText));

        newStoryText = "";
    }

    private void WriteMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            message = Environment.NewLine;
        }

        newStoryText += message;
    }

    public void SkipToChoice()
    {
        storyTextMesh.maxVisibleCharacters = storyTextMesh.text.Length;
    }

    public void Choose1()
    {
        Choose(() => Run.Outro1(FileData, Story, currentScene, WriteMessage), choice1Button.gameObject);
    }

    public void Choose2()
    {
        Choose(() => Run.Outro2(FileData, Story, currentScene, WriteMessage), choice2Button.gameObject);
    }

    private void Choose(Action runOutro, GameObject gameObject)
    {
        if (isWaiting)
        {
            return;
        }

        isWaiting = true;

        StartCoroutine(FadeButton(choice1Button, null, buttonFadeOutSeconds, fadeIn: false));
        StartCoroutine(FadeButton(choice2Button, null, buttonFadeOutSeconds, fadeIn: false));

        // LOWERCASE THE FIRST LETTER OF THE ACTION YOU CHOSE.
        string action = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        action = action.Substring(0, 1).ToLower() + action.Substring(1);

        WriteMessage($"<i>You {action}.</i>");

        WriteMessage("");
        WriteMessage("");

        runOutro();

        WriteMessage("");

        RunNewScenes();
    }

    private IEnumerator FadeButton(GameObject button, string text, float secondsToFade, bool fadeIn)
    {
        var buttonImage = button.GetComponent<CanvasGroup>();

        float startingAlpha = fadeIn ? 0 : 1;
        float targetAlpha = fadeIn ? 1 : 0;

        buttonImage.alpha = startingAlpha;
        if (fadeIn)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
        button.SetActive(true);

        yield return null;

        var startingTime = Time.time;

        float newAlpha = startingAlpha;
        while (Mathf.Abs(newAlpha - targetAlpha) > Mathf.Epsilon)
        {
            newAlpha = Mathf.Lerp(startingAlpha, targetAlpha, (Time.time - startingTime) / secondsToFade);

            buttonImage.alpha = newAlpha;

            yield return null;
        }

        buttonImage.alpha = targetAlpha;

        if (!fadeIn)
        {
            button.SetActive(false);
        }
    }
}
