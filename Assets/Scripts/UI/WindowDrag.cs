using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary> Clicking on the object attached to this script will move the object to the
    /// dragged cursor's position. </summary>
    public class WindowDrag : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public Transform WindowTransform;
        private Vector3 _mouseDownPosition;
        
        public void OnDrag(PointerEventData eventData)
        {
            WindowTransform.localPosition = Input.mousePosition - _mouseDownPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _mouseDownPosition = Input.mousePosition - WindowTransform.localPosition;
        }
    }
}
