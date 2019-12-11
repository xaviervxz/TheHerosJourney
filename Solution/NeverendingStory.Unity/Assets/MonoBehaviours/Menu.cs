using Assets.MonoBehaviours;
using TMPro;
using UnityEngine;
using NeverendingStory.Models;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private Button startGameButton;
#pragma warning restore 0649

    // Start is called before the first frame update
    private void Start()
    {
        GotoMainMenu();
    }

    public void GotoNewGameMenu()
    {
        mainMenu.SetActive(false);

        ValidateNewStory();

        newGameMenu.SetActive(true);
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

    public void StartNewGame()
    {
        Data.PlayersName = playersName.text;

        Data.PlayersSex = playersSex.options[playersSex.value].text == "Female" ? Sex.Female : Sex.Male;

        SceneManager.LoadScene("Game");
    }
}
