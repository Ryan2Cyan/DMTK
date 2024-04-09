
using System.Collections.Generic;
using General;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIManager : MonoBehaviour, IManager<UIElement>
    {
        public static UIManager Instance;
        public List<UIElement> AllUIElements;
        public bool ElementSelected;
        
        private IInputElement _currentUIElementInteraction;
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

        private void Update()
        {
            RaycastMouseOnUI();
        }

        #endregion
        
        
        #region ManagerFunctions
        
        public void RegisterElement(UIElement element)
        {
            AllUIElements.Add(element);
        }

        public void UnregisterElement(UIElement element)
        {
            AllUIElements.Remove(element);
        }
        
        #endregion

        #region InputFunctions

        public void OnMouseDown()
        {
            if(ElementSelected) _currentUIElementInteraction.OnMouseDown();
        }
        
        public void OnMouseUp()
        {
            if(ElementSelected) _currentUIElementInteraction.OnMouseUp();
        }

        public void OnMouseDrag()
        {
            if(ElementSelected) _currentUIElementInteraction.OnDrag();
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
                if (!result.gameObject.CompareTag("Interactable_UI")) continue;
                
                var newElement = result.gameObject.GetComponent<IInputElement>();
                if (newElement == null) continue;
                if (newElement == _currentUIElementInteraction) return;
                
                if(ElementSelected) _currentUIElementInteraction.OnMouseExit();
                _currentUIElementInteraction = newElement;
                newElement.OnMouseEnter();
                ElementSelected = true;
                return;
            }
            
            if(ElementSelected) _currentUIElementInteraction.OnMouseExit();
            _currentUIElementInteraction = null;
            ElementSelected = false;
        }

        #endregion
    }
}
