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
        public bool DebugEnabled;
        public delegate void DMTKUIDelegate();
        public static event DMTKUIDelegate DMTKUISelected;
        public static event DMTKUIDelegate DMTKUIDeselected;
        
        private IInputElement _currentUIElementInteraction;
        private PointerEventData _pointerEventData;
        private bool _isUIElementSelected;
        
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
            if(_isUIElementSelected) _currentUIElementInteraction.OnMouseDown();
        }
        
        public void OnMouseUp()
        {
            if(_isUIElementSelected) _currentUIElementInteraction.OnMouseUp();
        }

        public void OnMouseDrag()
        {
            if(_isUIElementSelected) _currentUIElementInteraction.OnDrag();
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
                
                if(_isUIElementSelected) _currentUIElementInteraction.OnMouseExit();
                _currentUIElementInteraction = newElement;
                newElement.OnMouseEnter();
                if(!_isUIElementSelected) DMTKUISelected?.Invoke();
                _isUIElementSelected = true;
                if(DebugEnabled) Debug.Log(result.gameObject.name);
                return;
            }
            
            if(_isUIElementSelected) _currentUIElementInteraction.OnMouseExit();
            _currentUIElementInteraction = null;
            if(_isUIElementSelected) DMTKUIDeselected?.Invoke();
            _isUIElementSelected = false;
        }

        #endregion
    }
}
