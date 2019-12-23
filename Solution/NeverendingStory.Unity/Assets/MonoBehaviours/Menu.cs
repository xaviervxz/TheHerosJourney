using Assets.MonoBehaviours;
using TMPro;
using UnityEngine;
using NeverendingStory.Models;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NeverendingStory.Functions;
using System.IO;
using System;
using System.Collections;
using System.Linq;

public class Menu : MonoBehaviour
{
    [SerializeField]
#pragma warning disable 0649
    private GameObject mainMenu;
#pragma warning restore 0649

    [SerializeField]
#pragma warning disable 0649
    private GameObject newGameMenu;
#pragma warning restore 0649

    [SerializeField]
#pragma warning disable 0649
    private TMP_InputField playersName;
#pragma warning restore 0649

    [SerializeField]
#pragma warning disable 0649
    private TMP_Dropdown playersSex;
#pragma warning restore 0649

    [SerializeField]
#pragma warning disable 0649
    private GameObject storySeedEntry;
#pragma warning restore 0649

    [SerializeField]
#pragma warning disable 0649
    private TMP_InputField storySeed;
#pragma warning restore 0649

    [SerializeField]
#pragma warning disable 0649
    private Button startGameButton;
#pragma warning restore 0649

    // Start is called before the first frame update
    private void Start()
    {
        LoadFileData();

        playersName.text = Data.PlayersName;
        playersSex.value = Data.PlayersSex == Sex.Female ? 0 : 1;

        GotoMainMenu();
    }

    internal static void LoadFileData(Action callback = null)
    {
        Stream GenerateStreamFromStreamingAsset(string fileName)
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, fileName);

            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            return stream;
        }

        Stream characterDataStream = GenerateStreamFromStreamingAsset("CharacterData.json");
        Stream locationDataStream = GenerateStreamFromStreamingAsset("LocationData.json");
        Stream scenesStream = GenerateStreamFromStreamingAsset("Scenes.ods");

        Data.FileData = Run.LoadGameData(characterDataStream, locationDataStream, scenesStream, callback);

        characterDataStream.Close();
        locationDataStream.Close();
        scenesStream.Close();
    }

    public void GotoNewGameMenu()
    {
        mainMenu.SetActive(false);

        ValidateNewStory();

        newGameMenu.SetActive(true);

        GenerateNewName();

        HighlightPlayersName();
    }

    private void HighlightPlayersName()
    {
        // Highlight the player's name and move the cursor to the end.

        playersName.ActivateInputField();

        IEnumerator MoveTextEnd()
        {
            yield return null;

            playersName.MoveTextEnd(false);
        }
        StartCoroutine(MoveTextEnd());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GotoMainMenu()
    {
        mainMenu.SetActive(true);

        newGameMenu.SetActive(false);
    }

    public void GenerateNewName()
    {
        var newName = Run.NewName(Data.FileData, SelectedPlayersSex);

        playersName.text = newName;

        HighlightPlayersName();
    }

    public void ShowStorySeedEntry()
    {
        storySeedEntry.SetActive(true);

        storySeed.Select();
    }

    public void HideStorySeedEntry()
    {
        storySeedEntry.SetActive(false);
    }

    public void ValidateNewStory()
    {
        if (playersName.text.Length > 0)
        {
            startGameButton.interactable = true;
        }
        else
        {
            startGameButton.interactable = false;
        }
    }

    private Sex SelectedPlayersSex => playersSex.options[playersSex.value].text == "Female" ? Sex.Female : Sex.Male;

    public void StartNewGame()
    {
        if (!startGameButton.interactable)
        {
            return;
        }

        Data.PlayersName = playersName.text;

        Data.PlayersSex = SelectedPlayersSex;

        const string testPrefix = "TEST:";
        if (storySeed.text.StartsWith(testPrefix))
        {
            try
            {
                Data.ScenesToTest = storySeed.text.Substring(testPrefix.Length).Split(',').Select(s => s.Trim()).ToArray();
            }
            catch
            {
                Debug.LogError("Story seed looked like it was a list of scenes, but didn't parse correctly. Sorry!");
                Data.StorySeed = storySeed.text;
            }
        }
        else
        {
            Data.StorySeed = storySeed.text;
        }

        SceneManager.LoadScene("Game");
    }
}
