using System.Collections.Generic;
using General;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UI
{
    public class UIManager : MonoBehaviour, IManager<UIElement>
    {
        [Header("Components")]
        public static UIManager Instance;
        public readonly List<UIElement> AllUIElements = new();
        
        [Header("Settings")]
        public bool DebugEnabled;
        
        
        [HideInInspector] public bool UIInteraction;
        [HideInInspector] public bool UISelected;
        
        private UIElement _currentUIElementInteraction;
        private PointerEventData _pointerEventData;
        
        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
            _pointerEventData = new PointerEventData(EventSystem.current) { pointerId = -1 };
        }

        private void OnEnable()
        {
            InputManager.OnMouseDown += OnMouseDown;
            InputManager.OnMouseUp += OnMouseUp;
            InputManager.OnMouseDrag += OnMouseDrag;
        }

        private void OnDisable()
        {
            InputManager.OnMouseDown -= OnMouseDown;
            InputManager.OnMouseUp -= OnMouseUp;
            InputManager.OnMouseDrag -= OnMouseDrag;
        }

        #endregion
        
        
        #region ManagerFunctions
        
        public void OnUpdate()
        {
            RaycastMouseOnUI();
        }

        public void OnLateUpdate()
        {
            UIInteraction = false;
        }

        public void RegisterElement(UIElement element)
        {
            AllUIElements.Add(element);
        }

        public void UnregisterElement(UIElement element)
        {
            AllUIElements.Remove(element);
        }
        
        #endregion

        #region InputQueueFunctions

        public void OnMouseDown()
        {
            InputManager.Instance.QueueUIInputFunction(MouseDown);
        }

        public void OnMouseUp()
        {
            InputManager.Instance.QueueUIInputFunction(MouseUp);
        }

        public void OnMouseDrag()
        {
            InputManager.Instance.QueueUIInputFunction(MouseDrag);
        }

        #endregion
        
        #region InputFunctions

        private void MouseDown()
        {
            if (!UISelected) return;
            _currentUIElementInteraction.OnMouseDown();
            UIInteraction = true;
        }

        private void MouseUp()
        {
            if (!UISelected) return; 
            _currentUIElementInteraction.OnMouseUp();
            UIInteraction = true;
        }

        private void MouseDrag()
        {
            if (!UISelected) return; 
            _currentUIElementInteraction.OnDrag();
            UIInteraction = true;
        }

        #endregion

        #region PrivateFunctions

        /// <remarks>Source: https://discussions.unity.com/t/detect-canvas-object-under-mouse-because-only-some-canvases-should-block-mouse/144125/3</remarks>
        private void RaycastMouseOnUI()
        {
            _pointerEventData.position = InputManager.MousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_pointerEventData, results);
            foreach (var result in results)
            {
                // Check has "Interactable_UI" tag:
                if (!result.gameObject.CompareTag("Interactable_UI")) continue;
                
                // Check has UIElement and is Active:
                var newElementUI = result.gameObject.GetComponent<UIElement>();
                if (newElementUI == null) continue;
                if (!newElementUI.UIElementActive) continue;
                
                // Select new element:
                if (UISelected) _currentUIElementInteraction.OnMouseExit();
                _currentUIElementInteraction = newElementUI;
                newElementUI.OnMouseEnter();
                UISelected = true;
                if (DebugEnabled) Debug.Log(result.gameObject.name);
                return;
            }
            
            // Cursor is selecting no interactable UI element:
            if(UISelected) _currentUIElementInteraction.OnMouseExit();
            _currentUIElementInteraction = null;
            UISelected = false;
        }

        #endregion
    }
}
