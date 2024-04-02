using UnityEngine;
using UnityEngine.UI;

namespace UI.Utility
{
    public class CullImages : MonoBehaviour
    {
        public Vector3 Target;
        public Camera Camera;
        
        private Image[] _uiImages;
        private bool _imagesActive;

        private void Awake()
        {
            _uiImages = GetComponentsInChildren<Image>();
        }

        private void Update()
        {
            var viewportPoint = Camera.WorldToViewportPoint(Target);
            if (viewportPoint.x is > 1f or < 0f)
            {
                ToggleUIImages(false); 
                return;
            }

            if (viewportPoint.y is > 1f or < 0f)
            {
                ToggleUIImages(false); 
                return;
            }

            if (viewportPoint.z < 0f)
            {
                ToggleUIImages(false);
                return;
            }
            
            ToggleUIImages(true);
        }
        
        private void ToggleUIImages(bool toggle)
        {
            if (toggle && _imagesActive) return;
            foreach (var image in _uiImages) image.enabled = toggle;
            _imagesActive = toggle;
        }
    }
}
