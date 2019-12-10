using UnityEngine;
using NeverendingStory.Models;
using NeverendingStory.Functions;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class Game : MonoBehaviour
{
    public float buttonFadeInSeconds = 0.5F;
    public float buttonFadeOutSeconds = 0.1F;
    public int lettersPerSecond = 10;

    [SerializeField]
    private TextMeshProUGUI storyTextMesh;

    [SerializeField]
    private GameObject choice1Button;

    [SerializeField]
    private GameObject choice2Button;

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

        (FileData, Story) = Run.LoadGame(ShowLoadGameFilesError);
        Story.You.Name = "Alex";
        Story.You.Sex = Sex.Male;

        RunNewScenes();
    }

    private void RunNewScenes()
    {
        bool choicesExist = false;

        do
        {
            WriteMessage("");

            currentScene = Run.NewScene(FileData, Story, WriteMessage);

            choicesExist = Run.PresentChoices(FileData, Story, currentScene, PresentChoices, WriteMessage);
        }
        while (!choicesExist);

        IEnumerator WriteToStory(string text)
        {
            int numLinesBefore = storyTextMesh.text.Split('\n').Length;

            storyTextMesh.text += text;

            Debug.Log($"Adding Lines: {storyTextMesh.text.Split('\n').Length - numLinesBefore}");

            // REVEAL MORE CHARACTERS

            float timeLastCharacterAdded = Time.time;

            while (storyTextMesh.maxVisibleCharacters < storyTextMesh.text.Length)
            {
                float timeDiff = Time.time - timeLastCharacterAdded;
                int numberOfCharsToReveal = (int)Math.Floor(lettersPerSecond * timeDiff);

                if (numberOfCharsToReveal > 0)
                {
                    timeLastCharacterAdded = Time.time;
                }

                storyTextMesh.maxVisibleCharacters = Math.Min(storyTextMesh.maxVisibleCharacters + numberOfCharsToReveal, storyTextMesh.text.Length);

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

    private void ShowLoadGameFilesError()
    {
        WriteMessage("");
        WriteMessage("Sorry, the Neverending Story couldn't load because it can't find the files it needs.");
        WriteMessage("First, make sure you're running the most current version.");
        WriteMessage("Then, if you are and this still happens, contact the developer and tell him to fix it.");
        WriteMessage("Thanks! <3");
    }

    private void PresentChoices(string choice1, string choice2)
    {
        choice1Text = choice1;
        choice2Text = choice2;
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

        WriteMessage("");
        WriteMessage("");

        // LOWERCASE THE FIRST LETTER OF THE ACTION YOU CHOSE.
        string action = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        action = action.Substring(0, 1).ToLower() + action.Substring(1);

        WriteMessage($"<i>You {action}.</i>");
        
        WriteMessage("");
        WriteMessage("");

        runOutro();

        RunNewScenes();
    }

    private IEnumerator FadeButton(GameObject button, string text, float secondsToFade, bool fadeIn)
    {
        var buttonImage = button.GetComponent<CanvasGroup>();

        float startingAlpha = buttonImage.alpha;
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
