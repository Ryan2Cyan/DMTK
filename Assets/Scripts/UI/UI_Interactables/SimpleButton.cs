using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI.UI_Interactables
{
    public class SimpleButton : MonoBehaviour, UIElement, IPooledObject
    {
        public bool UseColours;
        public Color UnhighlightedColour;
        public Color HighlightedColour;
        
        public UnityEvent OnPress;
        
        public Image Image;
        
        public bool UIElementActive { get; set; }
        public int UIElementPriority { get; set; }

        #region UnityFunctions

        protected virtual void Awake()
        {
            Image.color = UnhighlightedColour;
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

        public virtual void OnMouseEnter()
        {
            if(UseColours) Image.color = HighlightedColour;
        }

        public virtual void OnMouseExit()
        {
            if(UseColours) Image.color = UnhighlightedColour;
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
