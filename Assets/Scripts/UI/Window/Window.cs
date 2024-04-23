using System;
using TMPro;
using UnityEngine;

namespace UI.Window
{
    public class Window : MonoBehaviour, UIElement
    {
        [Header("Settings")]
        public Vector2 MinimumSize;
        public string WindowTitle;
        public bool FixedSize;
        public bool FixedPosition;
        
        [Header("Components")]
        public TextMeshProUGUI TitleTMP;
        
        private WindowDrag _windowDragScript;
        private Canvas _parentCanvas;
        private RectTransform _rectTransform;
        
        public bool UIElementActive { get; set; }
        public int UIElementPriority { get; set; }

        #region UnityFunctions
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>();
            
            _windowDragScript = GetComponentInChildren<WindowDrag>();
            _windowDragScript.WindowTransform = transform;
            _windowDragScript.ParentCanvas = _parentCanvas;
            
            TitleTMP.text = WindowTitle;
        }

        private void OnEnable()
        {
            UIElementActive = true;
        }

        private void OnDisable()
        {
            UIElementActive = false;
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
        }

        public void OnMouseUp()
        {
            if(!FixedPosition) _windowDragScript.OnMouseUp();
        }

        public void OnMouseEnter()
        {
            
        }

        public void OnMouseExit()
        {
            
        }

        public void OnDrag()
        {
            if(!FixedPosition) _windowDragScript.OnDrag();
        }
        
        #endregion
    }
}
