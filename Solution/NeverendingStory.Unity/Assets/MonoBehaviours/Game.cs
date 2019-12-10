using UnityEngine;
using NeverendingStory.Models;
using NeverendingStory.Functions;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class Game : MonoBehaviour
{
    public int lettersPerSecond = 10;

    [SerializeField]
    private TextMeshProUGUI storyText;

    [SerializeField]
    private GameObject choice1Button;

    [SerializeField]
    private GameObject choice2Button;

    private static FileData FileData;
    private static Story Story;
    private static Scene currentScene = null;
    private static bool isWaiting = false;

    private static string newText = "";
    private static string choice1Text = "";
    private static string choice2Text = "";

    // Start is called before the first frame update
    private void Start()
    {
        // RESET GAMEOBJECTS

        storyText.text = "";
        storyText.maxVisibleCharacters = 0;

        choice1Button.SetActive(false);
        choice2Button.SetActive(false);
        newText = "";

        // LOAD THE STORY

        (FileData, Story) = Run.LoadGame(ShowLoadGameFilesError);
        Story.You.Name = "Alex";
        Story.You.Sex = Sex.Male;

        RunNewScenes();
    }

    // Update is called once per frame
    private void Update()
    {
        // REVEAL MORE CHARACTERS

        int numberOfCharsToReveal = Math.Max(1, (int) Math.Floor(lettersPerSecond * Time.deltaTime));

        storyText.maxVisibleCharacters = Math.Min(storyText.maxVisibleCharacters + numberOfCharsToReveal, storyText.text.Length);

        // IF WE'RE DONE DISPLAYING WHAT WE HAVE SO FAR...
        if (storyText.maxVisibleCharacters == storyText.text.Length)
        {
            // AND THERE'S NEW TEXT LEFT TO DISPLAY, DISPLAY IT.
            if (!string.IsNullOrEmpty(newText))
            {
                string newTextToAdd = newText;

                var justTheFirstParagraph = Regex.Match(newText, "(.*\n\r?\n\r?)", RegexOptions.Multiline);

                if (justTheFirstParagraph.Length > 0)
                {
                    newTextToAdd = justTheFirstParagraph.Groups["0"].Value;
                }

                storyText.text += newTextToAdd;

                newText = newText.Substring(newTextToAdd.Length);
            }
            // OTHERWISE, SHOW THE CHOICES.
            else
            {
                isWaiting = false;

                StartCoroutine(FadeButton(choice1Button, choice1Text, 0.5F, fadeIn: true));
                StartCoroutine(FadeButton(choice2Button, choice2Text, 0.5F, fadeIn: true));
            }
        }
    }

    IEnumerator FadeButton(GameObject button, string text, float secondsToFade, bool fadeIn)
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
    }

    private void WriteMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            newText += Environment.NewLine;
        }
        else
        {
            newText += message;
        }
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
        storyText.maxVisibleCharacters = storyText.text.Length;
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

        StartCoroutine(FadeButton(choice1Button, null, 0.1F, fadeIn: false));
        StartCoroutine(FadeButton(choice2Button, null, 0.1F, fadeIn: false));

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
}
