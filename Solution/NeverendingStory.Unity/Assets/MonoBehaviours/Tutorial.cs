using Assets.MonoBehaviours;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private bool alwaysRunTutorial;

    [SerializeField]
    private RectTransform cover;

    [SerializeField]
    private TextMeshProUGUI tutorialText;

    [SerializeField]
    private TextMeshProUGUI continueButtonText;

    [SerializeField]
    private string continueText;

    [SerializeField]
    private string doneText;

    [SerializeField]
    public TutorialStep[] steps;

    private static int CurrentStep = 0;

    private const string PlayerPrefsHasRunTutorialKey = "HasRunTutorial";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsHasRunTutorialKey) || alwaysRunTutorial)
        {
            StartTutorial();
        }
        else
        {
            DismissTutorial();
        }
    }

    public void DismissTutorial()
    {
        PlayerPrefs.SetString(PlayerPrefsHasRunTutorialKey, "true");

        gameObject.SetActive(false);
    }

    public void StartTutorial()
    {
        continueButtonText.text = continueText;

        CurrentStep = -1;

        ContinueTutorial();

        gameObject.SetActive(true);
    }

    public void ContinueTutorial()
    {
        CurrentStep += 1;

        if (CurrentStep >= steps.Length)
        {
            DismissTutorial();
            return;
        }

        if (CurrentStep == steps.Length - 1)
        {
            continueButtonText.text = doneText;
        }

        var currentStep = steps[CurrentStep];

        cover.position = currentStep.targetObject.position + (Vector3) currentStep.targetObject.rect.center;
        tutorialText.text = currentStep.instructions.Replace("{name}", Data.PlayersName);
    }
}

[System.Serializable]
public class TutorialStep
{
    public RectTransform targetObject;

    [TextArea(3, 6)]
    public string instructions;
}