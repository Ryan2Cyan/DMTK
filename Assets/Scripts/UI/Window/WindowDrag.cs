using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Window
{
    /// <summary>Enables window GUI dragging. Object holding this script can be clicked, moving parent window. </summary>
    public class WindowDrag : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public Transform WindowTransform;
        
        private Canvas _parentCanvas;
        private Vector2 _mouseDownPosition;
        private Window _window;
        
        private void Start()
        {
            _window = GetComponentInParent<Window>();
            _parentCanvas = GetComponentInParent<Canvas>();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_window.FixedPosition) return;
            var moveVector = InputManager.MousePosition / _parentCanvas.scaleFactor - _mouseDownPosition;
            var windowPosition = WindowTransform.localPosition;
            windowPosition = new Vector3(moveVector.x, moveVector.y, windowPosition.z);
            WindowTransform.localPosition = windowPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_window.FixedPosition) return;
            var windowPosition = WindowTransform.localPosition;
            _mouseDownPosition = InputManager.MousePosition / _parentCanvas.scaleFactor - new Vector2(windowPosition.x, windowPosition.y);
        }
    }
}
