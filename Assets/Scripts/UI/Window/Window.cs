using System.Collections.Generic;
using Input;
using TMPro;
using UnityEngine;

namespace UI.Window
{
    public class Window : UIElement, IInputElement
    {
        [Header("Settings")]
        public Vector2 MinimumSize;
        public string WindowTitle;
        public bool FixedSize;
        public bool FixedPosition;
        
        [Header("Components")]
        public TextMeshProUGUI TitleTMP;
        
        private WindowDrag _windowDragScript;
        private List<WindowResizer> _windowResizerScripts;
        private Canvas _parentCanvas;
        private RectTransform _rectTransform;

        #region UnityFunctions
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>();
            
            _windowDragScript = GetComponentInChildren<WindowDrag>();
            _windowDragScript.WindowTransform = transform;
            _windowDragScript.ParentCanvas = _parentCanvas;
            
            // _windowResizerScripts = new List<WindowResizer>(GetComponentsInChildren<WindowResizer>());
            TitleTMP.text = WindowTitle;
        }

        [ExecuteAlways]
        private void OnValidate()
        {
            TitleTMP.text = WindowTitle;
        }
        
        #endregion

        #region PublicFunctions
        
        public RectTransform GetRectTransform()
        {
            return _rectTransform;
        }

        public Canvas GetCanvas()
        {
            return _parentCanvas;
        }
        
        #endregion

        #region InputFunctions
        
        public void OnMouseDown()
        {
            if(!FixedPosition) _windowDragScript.OnMouseDown();
            // foreach (var windowResizer in _windowResizerScripts) windowResizer.OnMouseDown();
        }

        public void OnMouseUp()
        {
            if(!FixedPosition) _windowDragScript.OnMouseUp();
            if (FixedSize) return;
            // foreach (var windowResizer in _windowResizerScripts) windowResizer.OnMouseUp();
        }

        public void OnMouseEnter()
        {
            if (FixedSize) return;
            // foreach (var windowResizer in _windowResizerScripts) windowResizer.OnMouseEnter();
        }

        public void OnMouseExit()
        {
            if (FixedSize) return;
            // foreach (var windowResizer in _windowResizerScripts) windowResizer.OnMouseExit();
        }

        public void OnDrag()
        {
            if(!FixedPosition) _windowDragScript.OnDrag();
            if (FixedSize) return;
            // foreach (var windowResizer in _windowResizerScripts) windowResizer.OnDrag();
        }
        
        #endregion
    }
}
