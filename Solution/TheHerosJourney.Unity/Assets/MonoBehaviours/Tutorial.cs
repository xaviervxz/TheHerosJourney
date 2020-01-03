using TMPro;
using UnityEngine;

namespace Assets.MonoBehaviours
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField]
#pragma warning disable 0649
        private bool alwaysRunTutorialInDebug;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private RectTransform cover;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private TextMeshProUGUI tutorialText;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private TextMeshProUGUI continueButtonText;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private string continueText;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private string doneText;
#pragma warning restore 0649

        [SerializeField]
#pragma warning disable 0649
        private TutorialStep[] steps;
#pragma warning restore 0649

        private static int CurrentStep = 0;

        private const string PlayerPrefsHasRunTutorialKey = "HasRunTutorial";

        private void Start()
        {
#if DEBUG
            if (!PlayerPrefs.HasKey(PlayerPrefsHasRunTutorialKey) || alwaysRunTutorialInDebug)
#else
            if (!PlayerPrefs.HasKey(PlayerPrefsHasRunTutorialKey))
#endif
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

            cover.anchorMin = currentStep.targetObject.anchorMin;
            cover.anchorMax = currentStep.targetObject.anchorMax;
            cover.anchoredPosition =
                currentStep.targetObject.anchoredPosition
                + currentStep.targetObject.rect.center
                - cover.rect.center;
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
}