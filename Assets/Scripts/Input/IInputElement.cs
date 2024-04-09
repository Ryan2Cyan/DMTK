namespace Input
{
    public interface IInputElement
    {
        public void OnMouseDown();
        public void OnMouseUp();
        public void OnMouseEnter();
        public void OnMouseExit();
        public void OnDrag();
    }
}
