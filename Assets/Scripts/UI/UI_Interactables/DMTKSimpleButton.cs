using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.UI_Interactables
{
    public class DMTKSimpleButton : UIElement, IInputElement
    {
        [Header("Settings")] 
        public Color UnhighlightedColour;
        public Color HighlightedColour;
        public UnityEvent OnPress;

        [Header("Component")] 
        public Image ButtonImage;

        #region UnityFunctions

        private void Awake()
        {
            ButtonImage.color = UnhighlightedColour;
        }

        #endregion

        #region InputFunctions

        public void OnMouseEnter()
        {
            ButtonImage.color = HighlightedColour;
        }

        public void OnMouseExit()
        {
            ButtonImage.color = UnhighlightedColour;
        }

        public void OnMouseDown()
        {
            OnPress?.Invoke();
        }

        #endregion
    }
}
