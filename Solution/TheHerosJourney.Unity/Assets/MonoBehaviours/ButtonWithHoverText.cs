using UnityEngine;

namespace Assets.MonoBehaviours
{
    public class ButtonWithHoverText : MonoBehaviour
    {
        [SerializeField]
#pragma warning disable 0649
        private GameObject hoverElement;
#pragma warning restore 0649

        private void Start()
        {
            hoverElement.SetActive(false);
        }

        private void OnEnable()
        {
            hoverElement.SetActive(false);
        }

        public void ShowHover()
        {
            hoverElement.SetActive(true);
        }

        public void HideHover()
        {
            hoverElement.SetActive(false);
        }
    }
}