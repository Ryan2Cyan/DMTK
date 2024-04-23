using Input;
using UnityEngine.UI;

namespace UI.UI_Interactables
{
    public class DMTKButton : Button, UIElement
    {
        public bool UIElementActive { get; set; }
        public int UIElementPriority { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            UIElementActive = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UIElementActive = false;
        }

        public void OnMouseDown() { }
        public void OnMouseUp() { }
        public void OnMouseEnter() { }
        public void OnMouseExit() { }
        public void OnDrag() { }
    }
}
