namespace ECT
{
    public class ECTSystemData : IReferenceSystem
    {
        public ECTSystemData(SystemInfo info, IComponent component)
        {
            Info = info;
            Component = component;
        }

        public SystemInfo Info { get; }
        
        public IComponent Component { get; }

        public virtual TSystem GetSystem<TSystem>() where TSystem : class, ISystem => Info.System as TSystem;

        public struct SystemInfo
        {
            public SystemInfo(IRoot root, IParent parent, ISystem system)
            {
                Root = root;
                Parent = parent;
                System = system;
            }

            public IRoot Root { get; }

            public IParent Parent { get; }

            public ISystem System { get; }
        }
    }
}