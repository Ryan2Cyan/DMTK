using UnityEngine;
using UnityEngine.UI;

namespace UI.Miniature_Radial
{
    public class ScreenToWorldSpaceTest : MonoBehaviour
    {
        public Transform WorldSpaceTarget;
        public RectTransform UIElementRectTransform;
        public Image UIElementImage;

        public AnimationCurve ScaleCurve;
        public float MaxDistance;
        public float MaxScale;
    
        private void Update()
        {
            UIElementImage.enabled = false;
            var viewportPoint = Camera.main.WorldToViewportPoint(WorldSpaceTarget.position);
            if (viewportPoint.x is > 1f or < 0f) return;
            if (viewportPoint.y is > 1f or < 0f) return;
            if (viewportPoint.z < 0f) return;
            
            UIElementImage.enabled = true;
            UIElementRectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, WorldSpaceTarget.transform.position);
            var distanceToCamera = Vector3.Distance(Camera.main.transform.position, WorldSpaceTarget.transform.position);
            var distanceLerp = distanceToCamera / MaxDistance;
            var value = ScaleCurve.Evaluate(distanceLerp);
            var scale = MaxScale * value;
            UIElementRectTransform.sizeDelta = new Vector2(scale, scale);
            
            Debug.Log(
                "Distance To Camera: " + distanceToCamera + 
                      " Lerp: " + value + 
                " Scale: " + UIElementRectTransform.sizeDelta);
        }
    }
}
