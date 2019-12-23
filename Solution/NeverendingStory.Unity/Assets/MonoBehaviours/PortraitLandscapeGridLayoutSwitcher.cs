using UnityEngine;
using UnityEngine.UI;

public class PortraitLandscapeGridLayoutSwitcher : MonoBehaviour
{
    public int minLandscapeWidth = 800;
    [Header("Landscape")]
    public int landscapeCellWidth = 200;
    public int landscapeCellHeight = 150;
    public int landscapePosY = 150;
    [Header("Portrait")]
    public int portraitCellWidth = 200;
    public int portraitCellHeight = 150;
    public int portraitPosY = 150;

    int prevWidth;

    private void Start()
    {
        var canvasScaler = GetComponentInParent<CanvasScaler>();

        var referenceHeight = canvasScaler.referenceResolution.y;

        float ratio = (float) minLandscapeWidth / referenceHeight;

        minLandscapeWidth = Mathf.FloorToInt(ratio * Screen.height);

        prevWidth = Screen.width;
    }

    private void Update()
    {
        if (prevWidth != Screen.width)
        {
            prevWidth = Screen.width;

            var gridLayoutGroup = GetComponent<GridLayoutGroup>();
            var rectTransform = GetComponent<RectTransform>();

            if (prevWidth < minLandscapeWidth)
            {
                // SET PORTRAIT

                gridLayoutGroup.cellSize = new Vector3(portraitCellWidth, portraitCellHeight);
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, portraitPosY);
            }
            else
            {
                // SET LANDSCAPE
                gridLayoutGroup.cellSize = new Vector3(landscapeCellWidth, landscapeCellHeight);
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, landscapePosY);
            }
        }
    }
}
