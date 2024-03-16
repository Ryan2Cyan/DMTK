using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// Clicking on the object attached to this script will move the object to the
    /// dragged cursor's position.
    /// </summary>
    public class WindowDrag : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public Transform WindowTransform;
        
        private Canvas _parentCanvas;
        private Vector3 _mouseDownPosition;
        private Window _window;
        
        private void Start()
        {
            _window = GetComponentInParent<Window>();
            _parentCanvas = GetComponentInParent<Canvas>();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_window.FixedPosition) return;
            // WindowTransform.localPosition = Input.mousePosition / _parentCanvas.scaleFactor - _mouseDownPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_window.FixedPosition) return;
            // _mouseDownPosition = Input.mousePosition / _parentCanvas.scaleFactor - WindowTransform.localPosition;
        }
    }
}
