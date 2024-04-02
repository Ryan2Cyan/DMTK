using UnityEngine.UI;

namespace UI.Utility
{
    public class CullImageComponents : CullBase
    {
        private Image[] _uiImages;

        private void Awake()
        {
            _uiImages = GetComponentsInChildren<Image>();
        }

        protected override void ToggleUIImages(bool toggle)
        {
            base.ToggleUIImages(toggle);
            foreach (var image in _uiImages) image.enabled = toggle;
        }
    }
}
