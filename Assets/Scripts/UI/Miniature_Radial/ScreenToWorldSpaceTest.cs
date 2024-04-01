using UnityEngine;

namespace UI.Miniature_Radial
{
    public class ScreenToWorldSpaceTest : MonoBehaviour
    {
        public Transform WorldSpaceTarget;
        public RectTransform UIElement;

        private void Update()
        {
            UIElement.position = RectTransformUtility.WorldToScreenPoint(Camera.main, WorldSpaceTarget.transform.position);
            var distanceToCamera = Vector3.Distance(Camera.main.transform.position, WorldSpaceTarget.transform.position);
            Debug.Log(distanceToCamera);
            UIElement.sizeDelta = UIElement.sizeDelta * distanceToCamera;
        }
    }
}
