namespace ECT
{
    [System.Serializable]
    public abstract class ECTReference<MyRoot, MyParent, MyComponent> : IReference where MyRoot : class, IParent where MyParent : class, IParent where MyComponent : class, IComponent
    {
        public ECTReference(MyComponent component, MyRoot root, MyParent parent, ISystem system)
        {
            Component = component;
            Root = root;
            Parent = parent;
            System = system;
        }

        public bool State = true;
        public MyComponent Component { get; }
        public MyRoot Root { get; }
        public MyParent Parent { get; }
        public ISystem System { get; }

        bool IReference.State { get => State; set => State = value; }
        IComponent IReference.Component => Component;
        IParent IReference.Root => Root;
        IParent IReference.Parent => Parent;

        public virtual FindSystem Get<FindSystem>() where FindSystem : class, ISystem => System as FindSystem;
    }

    public interface IReference
    {
        public bool State { get; set; }
        public IComponent Component { get; }
        public IParent Root { get; }
        public IParent Parent { get; }
        public ISystem System { get; }
        public FindSystem Get<FindSystem>() where FindSystem : class, ISystem;
    }
}