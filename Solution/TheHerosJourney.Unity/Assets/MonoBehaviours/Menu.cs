using TMPro;
using TheHerosJourney.Models;
using TheHerosJourney.Functions;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

namespace Assets.MonoBehaviours
{
    public class Menu : MonoBehaviour
    {
        [Header("Root Menu Objects")]

        [SerializeField]
#pragma warning disable 0649
        private GameObject title;
#pragma warning restore 0649

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
        private GameObject loadGameMenu;
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

        [Header("Load Game Menu")]

        [SerializeField]
#pragma warning disable 0649
        private Transform savedGameParent;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private Button savedGamePrefab;
#pragma warning restore 0649

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
            // SCENES ARE BEING LOADED FROM A CSV INSTEAD OF FROM AN .ODS
            // BECAUSE ANDROID'S STREAMING ASSETS ARE A PAIN IN THE BUTT,
            // AND THE TOOL I FOUND TO SOLVE THAT (BETTER STREAMING ASSETS)
            // DOESN'T SUPPORT LOADING COMPRESSED FILES, AND AN .ODS FILE
            // IS A COMPRESSED FILE.
            Stream scenesStream = GenerateStreamFromStreamingAsset("scenes.csv");
            Stream adventuresStream = GenerateStreamFromStreamingAsset("adventures.csv");

            Data.FileData = Run.LoadGameData(characterDataStream, locationDataStream, scenesStream, adventuresStream, callback);

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

#if !UNITY_ANDROID
            HighlightPlayersName();
#endif

            title.SetActive(true);
        }

        public void GotoLoadGameMenu()
        {
            mainMenu.SetActive(false);

            foreach (Transform oldButton in savedGameParent)
            {
                Destroy(oldButton.gameObject);
            }

            string saveFolder = Game.GetSaveFolderPath();
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            var existingSaveFiles = Directory.GetFiles(saveFolder);

            foreach (var saveFile in existingSaveFiles)
            {
                var rawJson = File.ReadAllText(saveFile);
                var savedGame = JsonConvert.DeserializeObject<SavedGameData>(rawJson);

                var newLoadGameButton = Instantiate(savedGamePrefab, savedGameParent);

                // SET THE BUTTON ACTION.
                newLoadGameButton.onClick.AddListener(() => LoadGame(Path.GetFileName(saveFile)));

                // SET THE BUTTON TEXT.
                var text = newLoadGameButton.GetComponentInChildren<TextMeshProUGUI>();
                var currentLocation = savedGame.Locations.First(l => l.Name == savedGame.You.CurrentLocation);
                var currentLocationName = (currentLocation.HasThe ? "the " : "") + currentLocation.Name;
                text.text = $"#{Path.GetFileNameWithoutExtension(saveFile)} {savedGame.You.Name}, in {currentLocationName}"
                    + Environment.NewLine
                    + $"<size=75%>Last saved: {savedGame.TimeLastSaved.ToString("yyyy.MM.dd 'at' hh:mm tt")}</size>";
            }

            loadGameMenu.SetActive(true);

            title.SetActive(false);
        }

        public void LoadGame(string saveFileName)
        {
            Data.SaveFileName = saveFileName;

            FadeScene.In("Game");
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
            loadGameMenu.SetActive(false);

            title.SetActive(true);
        }

        public void GenerateNewName()
        {
            var newName = Run.NewName(Data.FileData, SelectedPlayersSex);

            playersName.text = newName;

#if !UNITY_ANDROID
            HighlightPlayersName();
#endif
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

            // SET THE PLAYER'S NAME AND SEX
            Data.PlayersName = playersName.text;
            Data.PlayersSex = SelectedPlayersSex;

            // PARSE AND SET THE STORY SEED.
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

            // CREATE A NEW SAVE FILE.
            string saveFolder = Game.GetSaveFolderPath();
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            var existingSaveFiles = Directory.GetFiles(saveFolder).Select(Path.GetFileName).ToArray();
            string newSaveFileName = "";
            int savedFileNumber = 1;
            do
            {
                newSaveFileName = $"{savedFileNumber}.sav";
                savedFileNumber += 1;
            }
            while (existingSaveFiles.Contains(newSaveFileName));
            Data.SaveFileName = newSaveFileName;

            // START THE GAME! :D
            FadeScene.In("Game");
        }
    }
}