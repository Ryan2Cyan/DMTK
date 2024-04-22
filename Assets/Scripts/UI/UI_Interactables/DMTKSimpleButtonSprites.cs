using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI.UI_Interactables
{
    public class DMTKSimpleButtonSprites : MonoBehaviour, UIElement, IPooledObject
    {
        [Header("Settings")] 
        public Sprite UnhighlightedSprite;
        public Sprite HighlightedSprite;
        public UnityEvent OnPress;

        [Header("Components")] 
        public Image ButtonOverlayImage;
        public bool UIElementActive { get; set; }

        #region UnityFunctions

        protected virtual void Awake()
        {
            ButtonOverlayImage.sprite = UnhighlightedSprite;
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
            ButtonOverlayImage.sprite = HighlightedSprite;
        }

        public void OnMouseExit()
        {
            ButtonOverlayImage.sprite = UnhighlightedSprite;
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
