using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI.UI_Interactables
{
    public class DMTKSimpleButton : UIElement, IInputElement, IPooledObject
    {
        [Header("Settings")] 
        public Color UnhighlightedColour;
        public Color HighlightedColour;
        public UnityEvent OnPress;

        [Header("Components")] 
        public Image ButtonOverlayImage;

        #region UnityFunctions

        protected virtual void Awake()
        {
            ButtonOverlayImage.color = UnhighlightedColour;
        }

        #endregion

        #region InputFunctions

        public void OnMouseEnter()
        {
            ButtonOverlayImage.color = HighlightedColour;
        }

        public void OnMouseExit()
        {
            ButtonOverlayImage.color = UnhighlightedColour;
        }

        public virtual void OnMouseDown()
        {
            OnPress?.Invoke();
        }

        #endregion

        public void Instantiate() { }
        public void Release() { }
    }
}
