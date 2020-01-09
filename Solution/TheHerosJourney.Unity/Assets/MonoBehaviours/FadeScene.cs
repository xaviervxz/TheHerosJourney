using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MonoBehaviours
{
    public class FadeScene : MonoBehaviour
    {
        [SerializeField]
#pragma warning disable 0649
        private CanvasGroup blackOverlay;
#pragma warning restore 0649

        private static FadeScene Instance;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(blackOverlay.gameObject);

            Instance = this;

            In("Menu");
        }

        public static void In(string sceneName)
        {
            if (Instance == null)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }

            IEnumerator Fade(string cSceneName)
            {
                Instance.blackOverlay.alpha = 0F;
                Instance.blackOverlay.blocksRaycasts = true;
                Instance.blackOverlay.gameObject.SetActive(true);

                const float fadeDuration = 1.5F;
                const float inBetweenPause = 0.5F;

                {
                    var timeStarted = Time.time;
                    float lerpValue;
                    do
                    {
                        lerpValue = (Time.time - timeStarted) / fadeDuration;
                        var newAlpha = Mathf.Lerp(0F, 1F, lerpValue);
                        Instance.blackOverlay.alpha = newAlpha;
                        yield return null;
                    }
                    while (lerpValue < 1F);
                }

                Instance.blackOverlay.alpha = 1F;
                yield return null;

                {
                    var loadingScene = SceneManager.LoadSceneAsync(cSceneName);

                    var timeStarted = Time.time;
                    while (!loadingScene.isDone || Time.time - timeStarted < inBetweenPause)
                    {
                        yield return null;
                    }
                }

                Instance.blackOverlay.blocksRaycasts = false;

                {
                    var timeStarted = Time.time;
                    float lerpValue;
                    do
                    {
                        lerpValue = (Time.time - timeStarted) / fadeDuration;
                        var newAlpha = Mathf.Lerp(1F, 0F, lerpValue);
                        Instance.blackOverlay.alpha = newAlpha;
                        yield return null;
                    }
                    while (lerpValue < 1F);
                }

                Instance.blackOverlay.alpha = 0F;
                Instance.blackOverlay.gameObject.SetActive(false);
                yield return null;
            }

            Instance.StartCoroutine(Fade(sceneName));
        }
    }
}