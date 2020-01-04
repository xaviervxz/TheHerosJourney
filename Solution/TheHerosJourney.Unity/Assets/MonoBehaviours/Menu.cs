using TMPro;
using TheHerosJourney.Models;
using TheHerosJourney.Functions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;

namespace Assets.MonoBehaviours
{
    public class Menu : MonoBehaviour
    {
        [Header("Root Menu Objects")]
        [SerializeField]
#pragma warning disable 0649
        private GameObject mainMenu;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private GameObject newGameMenu;
#pragma warning restore 0649

        [Header("New Game Menu")]
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
            playersName.text = Data.PlayersName;
            playersSex.value = Data.PlayersSex == Sex.Female ? 0 : 1;

            GotoMainMenu();

            LoadFileData(() => Debug.LogError("Could not load FileData--the game will not run correctly."));
        }

        internal static void LoadFileData(Action callback = null)
        {
#if !UNITY_ANDROID
            Stream GenerateStreamFromStreamingAsset(string fileName)
            {
                var filePath = Path.Combine(Application.streamingAssetsPath, fileName);

                var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                return stream;
            }
#else
            BetterStreamingAssets.Initialize();

            Stream GenerateStreamFromStreamingAsset(string fileName)
            {
                var stream = BetterStreamingAssets.OpenRead(fileName);
                
                return stream;
            }
#endif

            Stream characterDataStream = GenerateStreamFromStreamingAsset("character_data.json");
            Stream locationDataStream = GenerateStreamFromStreamingAsset("location_data.json");
            Stream scenesStream = GenerateStreamFromStreamingAsset("scenes.ods");

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

#if !UNITY_ANDROID
            storySeed.Select();
#endif
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
}