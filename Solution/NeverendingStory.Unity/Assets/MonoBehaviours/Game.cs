using NeverendingStory.Functions;
using NeverendingStory.Models;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public float scrollSpeed = 5F;
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

    private static bool buttonsFadedIn = false;

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
        isWaiting = true;

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
        if (!isWaiting)
        {
            // CHANGE THE TARGET SCROLL Y IF THE SCROLL WHEEL WAS USED.
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
            if (!Mathf.Approximately(scrollAmount, 0) && !isWaiting)
            {
                ScrollTo(targetScrollY - scrollAmount * scrollSpeed * 10 * (storyTextMesh.fontSize + 4));
            }

            // FADE IN OR OUT BUTTONS

            int bottomLine = Math.Max(0, storyTextMesh.textInfo.lineCount - 3);
            // Checking the actual position here, not the targetScrollY,
            // because we want to know if the story has ACTUALLY cleared the buttons,
            // Not just if it's going to.
            if (storyTextMesh.rectTransform.anchoredPosition.y > ScrollYForLine(bottomLine, lineAtTop: false))
            {
                if (!buttonsFadedIn)
                {
                    StopCoroutine("FadeOutButtons");

                    StartCoroutine(FadeInButtons());
                }
            }
            else
            {
                if (buttonsFadedIn)
                {
                    StopCoroutine("FadeInButtons");
                
                    StartCoroutine(FadeOutButtons());
                }
            }
        }


        // SCROLL TO THE TARGET SCROLL Y

        const float secondsToScrollFor = 0.5F;

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
        var scrollY = lineNumber * (storyTextMesh.fontSize + 4);
        if (!lineAtTop)
        {
            float parentsHeight = storyTextMesh.rectTransform.parent.GetComponent<RectTransform>().rect.height;
            scrollY -= (parentsHeight - storyTextMesh.margin.w - 40);
        }

        return scrollY;
    }

    private void ScrollTo(float newScrollY)
    {
        // Doing this to force textInfo.lineCount to update below.
        storyTextMesh.ForceMeshUpdate();

        int bottomLine = Math.Max(0, storyTextMesh.textInfo.lineCount - 3);
        float scrollYForBottomLine = ScrollYForLine(bottomLine, lineAtTop: true);

        if (scrollYForBottomLine > 0 && newScrollY > scrollYForBottomLine)
        {
            newScrollY = scrollYForBottomLine;
        }

        if (newScrollY < 0)
        {
            newScrollY = 0;
        }

        targetScrollY = newScrollY;
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

            ScrollTo(ScrollYForLine(oldLineCount)); // This needs to be AFTER, the new text is added, otherwise the clamping screws this up.

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
                    ScrollTo(ScrollYForLine(currentLineNumber, lineAtTop: false));
                }

                yield return null;
            }

            isWaiting = false;

            var lastLineScrollY = ScrollYForLine(storyTextMesh.textInfo.lineCount - 1, lineAtTop: false);

            if (lastLineScrollY > targetScrollY)
            {
                ScrollTo(ScrollYForLine(storyTextMesh.textInfo.lineCount - 1, lineAtTop: false));
            }

            storyTextMesh.maxVisibleCharacters = storyTextMesh.text.Length;
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

        StartCoroutine(FadeOutButtons());

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

    private IEnumerator FadeInButtons()
    {
        buttonsFadedIn = true;

        var button1Fade = StartCoroutine( FadeButton(choice1Button, choice1Text, buttonFadeInSeconds, fadeIn: true) );
        var button2Fade = StartCoroutine( FadeButton(choice2Button, choice2Text, buttonFadeInSeconds, fadeIn: true) );

        yield return button1Fade;
        yield return button2Fade;
    }

    private IEnumerator FadeOutButtons()
    {
        buttonsFadedIn = false;

        var button1Fade = StartCoroutine(FadeButton(choice1Button, choice1Text, buttonFadeOutSeconds, fadeIn: false));
        var button2Fade = StartCoroutine(FadeButton(choice2Button, choice2Text, buttonFadeOutSeconds, fadeIn: false));

        yield return button1Fade;
        yield return button2Fade;
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
