using UnityEngine;

namespace UI.Utility
{
    public class CullBase : MonoBehaviour
    {
        [Header("Components")]
        public Camera Camera;
        public Transform Target;

        private bool _isActive = true;
        
        
        private void Update()
        {
            if (Target == null) return;
            var viewportPoint = Camera.WorldToViewportPoint(Target.position);
            if (viewportPoint.x is > 1f or < 0f)
            {
                if(_isActive) ToggleUIImages(false);
                return;
            }

            if (viewportPoint.y is > 1f or < 0f)
            {
                if(_isActive) ToggleUIImages(false);
                return;
            }

            if (viewportPoint.z < 0f)
            {
                if(_isActive) ToggleUIImages(false);
                return;
            }
            
            if(!_isActive) ToggleUIImages(true);
        }

        protected virtual void ToggleUIImages(bool toggle)
        {
            _isActive = toggle;
        }
    }
}
