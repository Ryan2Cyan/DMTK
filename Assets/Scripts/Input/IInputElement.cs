namespace Input
{
    public interface IInputElement
    {
        public virtual void OnMouseDown() {}
        public virtual void OnMouseUp() {}
        public virtual void OnMouseEnter() {}
        public virtual void OnMouseExit() {}
        public virtual void OnDrag() {}
    }
}
