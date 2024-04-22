namespace General
{
    public interface IManager<in T>
    {
        public void OnUpdate();
        public void OnLateUpdate();
        public void RegisterElement(T element);
        public void UnregisterElement(T element);
    }
}
