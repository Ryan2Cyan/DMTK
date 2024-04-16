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
        public Image BaseImage;
        [HideInInspector] public RectTransform RectTransform;

        #region UnityFunctions

        private void Awake()
        {
            ButtonOverlayImage.color = UnhighlightedColour;
            RectTransform = GetComponent<RectTransform>();
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

        public void OnMouseDown()
        {
            OnPress?.Invoke();
        }

        #endregion

        public void Instantiate() { }
        public void Release() { }
    }
}
