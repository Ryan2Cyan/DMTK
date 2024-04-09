namespace General
{
    public interface IManager<in T>
    {
        public void RegisterElement(T element);
        public void UnregisterElement(T element);
    }
}
