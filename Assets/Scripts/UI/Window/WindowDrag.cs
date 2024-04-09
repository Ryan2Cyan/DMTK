using Input;
using UnityEngine;

namespace UI.Window
{
    /// <summary>Enables window GUI dragging. Object holding this script can be clicked, moving parent window. </summary>
    public class WindowDrag : MonoBehaviour
    {
        [HideInInspector] public Transform WindowTransform;
        [HideInInspector] public Canvas ParentCanvas;
        private Vector2 _mouseDownPosition;
        private bool _clicked;
        
        private void Start()
        {
            ParentCanvas = GetComponentInParent<Canvas>();
        }
        
        public void OnDrag()
        {
            if (!_clicked) return;
            var moveVector = InputManager.MousePosition / ParentCanvas.scaleFactor - _mouseDownPosition;
            var windowPosition = WindowTransform.localPosition;
            windowPosition = new Vector3(moveVector.x, moveVector.y, windowPosition.z);
            WindowTransform.localPosition = windowPosition;
        }

        public void OnMouseDown()
        {
            var windowPosition = WindowTransform.localPosition;
            _mouseDownPosition = InputManager.MousePosition / ParentCanvas.scaleFactor - new Vector2(windowPosition.x, windowPosition.y);
            _clicked = true;
        }

        public void OnMouseUp()
        {
            _clicked = false;
        }
    }
}
