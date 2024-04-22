using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI.UI_Interactables
{
    public class DMTKSimpleButton : MonoBehaviour, UIElement, IPooledObject
    {
        [Header("Settings")] 
        public Color UnhighlightedColour;
        public Color HighlightedColour;
        public UnityEvent OnPress;

        [Header("Components")] 
        public Image ButtonOverlayImage;
        public bool UIElementActive { get; set; }

        #region UnityFunctions

        protected virtual void Awake()
        {
            ButtonOverlayImage.color = UnhighlightedColour;
        }

        private void OnEnable()
        {
            UIElementActive = true;
        }

        private void OnDisable()
        {
            UIElementActive = false;
        }

        #endregion

        #region InputFunctions

        public void OnMouseUp()
        {
            OnPress?.Invoke();
        }

        public void OnMouseEnter()
        {
            ButtonOverlayImage.color = HighlightedColour;
        }

        public void OnMouseExit()
        {
            ButtonOverlayImage.color = UnhighlightedColour;
        }

        public void OnDrag()
        {
            
        }

        public virtual void OnMouseDown() { }

        #endregion

        public void Instantiate() { }
        public void Release() { }
    }
}
