using TMPro;

namespace UI.Utility
{
    public class CullTMPComponents : CullBase
    {
        private TextMeshProUGUI[] _uiTMPs;

        private void Awake()
        {
            _uiTMPs = GetComponentsInChildren<TextMeshProUGUI>();
        }

        protected override void ToggleUIImages(bool toggle)
        {
            base.ToggleUIImages(toggle);
            foreach (var image in _uiTMPs) image.enabled = toggle;
        }
    }
}
