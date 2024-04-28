using Camera;
using UnityEngine;

namespace UI.Utility
{
    public class CullBase : MonoBehaviour
    {
        [Header("Components")]
        public Transform Target;

        private UnityEngine.Camera _camera;
        private bool _isActive = true;

        private void Start()
        {
            _camera = CameraManager.Instance.MainCamera;
        }

        private void Update()
        {
            if (Target == null) return;
            var viewportPoint = _camera.WorldToViewportPoint(Target.position);
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
