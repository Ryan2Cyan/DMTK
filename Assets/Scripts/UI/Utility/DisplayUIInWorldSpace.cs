using UnityEngine;

namespace UI.Utility
{
    public class DisplayUIInWorldSpace : MonoBehaviour
    {
        [Header("Components")] 
        public Camera Camera;
        [HideInInspector] public Vector3 WorldSpaceTarget;
        [HideInInspector] public Transform UIElementTransform;

        [Header("Settings")]
        public AnimationCurve ScaleCurve;
        public Vector3 Offset;
        public float MaxDistance;
        public float MaxScale;

        private void Update()
        {
            // Set UI position (as if in world-space):
            var position = WorldSpaceTarget + Offset;
            UIElementTransform.position = RectTransformUtility.WorldToScreenPoint(Camera, position);
            
            // Set UI scale (as if in world-space):
            var distanceToCamera = Vector3.Distance(Camera.transform.position, position);
            var distanceLerp = distanceToCamera / MaxDistance;
            var scale = MaxScale * ScaleCurve.Evaluate(distanceLerp);
            UIElementTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
