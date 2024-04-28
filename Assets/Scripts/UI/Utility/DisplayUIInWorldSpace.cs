using System.Collections.Generic;
using Camera;
using UnityEngine;

namespace UI.Utility
{
    public class DisplayUIInWorldSpace : MonoBehaviour
    {
        [Header("Components")] 
        public Transform WorldSpaceTarget;
        public List<Transform> UIElementTransforms;

        [Header("Settings")] 
        public AnimationCurve ScaleCurve;
        public Vector3 Offset;
        public float MaxDistance;
        public float MaxScale;

        private UnityEngine.Camera _camera;

        private void Start()
        {
            _camera = CameraManager.Instance.MainCamera;
        }

        private void Update()
        {
            if (_camera == null) return;
            CalculateWorldSpacePosition();
        }

        public void CalculateWorldSpacePosition()
        {
            if(_camera == null) _camera = CameraManager.Instance.MainCamera;
            
            // Set UI position (as if in world-space):
            var position = WorldSpaceTarget.position + Offset;
            
            // Set UI scale (as if in world-space):
            var distanceToCamera = Vector3.Distance(_camera.transform.position, position);
            var distanceLerp = distanceToCamera / MaxDistance;
            var scale = MaxScale * ScaleCurve.Evaluate(distanceLerp);

            foreach (var uiElementTransform in UIElementTransforms)
            {
                uiElementTransform.position = RectTransformUtility.WorldToScreenPoint(_camera, position);
                uiElementTransform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}
