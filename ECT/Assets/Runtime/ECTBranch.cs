namespace ECT
{
    [System.Serializable]
    public class ECTBranch<MyComponent> : IBranch where MyComponent : class, IComponent
    {
        public MyComponent[] Components;
    }

    public interface IBranch
    {

    }
}