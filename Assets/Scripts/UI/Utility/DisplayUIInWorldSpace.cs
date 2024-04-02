using System.Collections.Generic;
using UnityEngine;

namespace UI.Utility
{
    public class DisplayUIInWorldSpace : MonoBehaviour
    {
        [Header("Components")] 
        public Camera Camera;
        public Transform WorldSpaceTarget;
        public List<Transform> UIElementTransforms;

        [Header("Settings")] 
        public AnimationCurve ScaleCurve;
        public Vector3 Offset;
        public float MaxDistance;
        public float MaxScale;

        private void Update()
        {
            // Set UI position (as if in world-space):
            var position = WorldSpaceTarget.position + Offset;
            
            // Set UI scale (as if in world-space):
            var distanceToCamera = Vector3.Distance(Camera.transform.position, position);
            var distanceLerp = distanceToCamera / MaxDistance;
            var scale = MaxScale * ScaleCurve.Evaluate(distanceLerp);

            foreach (var uiElementTransform in UIElementTransforms)
            {
                uiElementTransform.position = RectTransformUtility.WorldToScreenPoint(Camera, position);
                uiElementTransform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}
