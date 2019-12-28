using UnityEngine;
using UnityEngine.UI;

namespace Assets.MonoBehaviours
{
    public class PortraitLandscapeSizeSwitcher : MonoBehaviour
    {
        public int minLandscapeWidth = 800;
        [Header("Landscape")]
        /// MULTIPLY BY HEIGHT TO GET WIDTH IN LANDSCAPE.
        public float landscapeRatioWidth = 1F;
        /// WIDTH IS ALWAYS MAX IN PORTRAIT.

        private int prevWidth;
        private int scaledHeight;

        private void Start()
        {
            var canvasScaler = GetComponentInParent<CanvasScaler>();

            var referenceHeight = canvasScaler.referenceResolution.y;

            float ratio = (float)minLandscapeWidth / referenceHeight;

            minLandscapeWidth = Mathf.FloorToInt(ratio * Screen.height);

            scaledHeight = Mathf.FloorToInt(Screen.width / ratio);

            prevWidth = Screen.width;
        }

        private void Update()
        {
            if (prevWidth != Screen.width)
            {
                prevWidth = Screen.width;

                var rectTransform = GetComponent<RectTransform>();

                if (prevWidth < minLandscapeWidth)
                {
                    // SET PORTRAIT

                    rectTransform.sizeDelta = new Vector2(Screen.width, rectTransform.sizeDelta.y);
                }
                else
                {
                    // SET LANDSCAPE

                    rectTransform.sizeDelta = new Vector2(landscapeRatioWidth * scaledHeight, rectTransform.sizeDelta.y);
                }
            }
        }
    }
}