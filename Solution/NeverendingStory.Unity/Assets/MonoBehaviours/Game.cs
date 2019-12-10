using NeverendingStory.Functions;
using NeverendingStory.Models;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

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

    // Start is called before the first frame update
    private void Start()
    {
        // RESET GAMEOBJECTS

        storyTextMesh.text = "";
        storyTextMesh.maxVisibleCharacters = 0;

        choice1Button.SetActive(false);
        choice2Button.SetActive(false);

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

    private IEnumerator ScrollToY(float targetY, float numSeconds)
    {
        var storyTextRect = storyTextMesh.rectTransform;

        float timeStarted = Time.time;
        float startingY = storyTextRect.anchoredPosition.y;
        float currentY = startingY;

        while (!Mathf.Approximately(currentY, targetY))
        {
            currentY = Mathf.SmoothStep(startingY, targetY, (Time.time - timeStarted) / numSeconds);
            storyTextRect.anchoredPosition = new Vector2(storyTextRect.anchoredPosition.x, currentY);

            yield return null;
        }

        storyTextRect.anchoredPosition = new Vector2(storyTextRect.anchoredPosition.x, targetY);

        yield return null;
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

            // SCROLL DOWN WHILE THE TEXT REVEALS.
            
            float parentsHeight = storyTextMesh.rectTransform.parent.GetComponent<RectTransform>().rect.height;
            var targetY = Math.Max(0, storyTextMesh.textInfo.lineCount * (storyTextMesh.fontSize + 4));
            //Debug.Log("Line count: " + storyTextMesh.textInfo.lineCount);

            // HOW LONG SHOULD THE SCROLL TAKE?
            //int charactersInNewText = text.Length;
            //float secondsToScroll = charactersInNewText / lettersPerSecond;
            
            StartCoroutine(ScrollToY(targetY, 3));

            // ADD THE NEW TEXT TO THE STORY.
            storyTextMesh.text += text;

            // REVEAL MORE CHARACTERS

            float charactersRevealed = storyTextMesh.maxVisibleCharacters;

            while (storyTextMesh.maxVisibleCharacters < storyTextMesh.text.Length)
            {
                charactersRevealed += lettersPerSecond * Time.deltaTime;

                storyTextMesh.maxVisibleCharacters = Math.Min(
                    Mathf.FloorToInt(charactersRevealed),
                    storyTextMesh.text.Length);

                yield return null;
            }

            isWaiting = false;

            StartCoroutine(FadeButton(choice1Button, choice1Text, buttonFadeInSeconds, fadeIn: true));
            StartCoroutine(FadeButton(choice2Button, choice2Text, buttonFadeInSeconds, fadeIn: true));

            storyTextMesh.maxVisibleCharacters = storyTextMesh.text.Length;

            yield return null;
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
