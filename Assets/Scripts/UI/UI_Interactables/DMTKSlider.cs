using UnityEngine.UI;

namespace UI.UI_Interactables
{
    public class DMTKSlider : Slider, UIElement
    {
        public bool UIElementActive { get; set; }
        public int UIElementPriority { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            UIElementActive = true;
            UIElementPriority = 1;
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
