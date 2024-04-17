namespace UI
{
    public interface UIElement
    {
        public bool UIElementActive { get; set; }

        #region InputFunctions

        public void OnMouseDown();
        public void OnMouseUp();
        public void OnMouseEnter();
        public void OnMouseExit();
        public void OnDrag();

        #endregion
    }
}
