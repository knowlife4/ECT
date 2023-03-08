namespace ECT
{
    [System.Serializable]
    public class ECTComponentGroup<TComponent> : IComponentBranch where TComponent : IComponent
    {
        public TComponent[] Components;
    }

    public interface IComponentBranch
    {
        
    }
}