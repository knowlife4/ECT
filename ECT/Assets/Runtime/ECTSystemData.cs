namespace ECT
{
    public class ECTSystemData : IReferenceSystem
    {
        public ECTSystemData(IRoot root, IParent parent, IComponent component, ISystem system)
        {
            Parent = parent;
            Root = root;
            Component = component;
            System = system;
        }

        public IRoot Root { get; }

        public IParent Parent { get; }

        public IComponent Component { get; }
        
        public ISystem System { get; }
        
        public virtual TSystem GetSystem<TSystem>() where TSystem : class, ISystem => System as TSystem;
    }
}