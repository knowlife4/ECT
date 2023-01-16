namespace ECT
{
    public abstract class ECTSystem<MyReference, MyComponent, MyRoot, MyParent> : ISystem where MyReference : IReference where MyComponent : class, IComponent where MyRoot : class, IParent where MyParent : class, IParent
    {
        public MyReference Reference { get; private set; }

        public virtual ECTValidation?[] Validations => System.Array.Empty<ECTValidation?>();

        protected virtual void OnInitialize() {}
        public void Initialize() => OnInitialize();

        protected virtual void OnUpdate() {}
        public void Update() => OnUpdate();

        public void SetReference(IReference reference) => Reference = (MyReference)reference;

        public MyComponent Component => Reference.Component as MyComponent;
        public MyRoot Root => Reference.Root as MyRoot;
        public MyParent Parent => Reference.Parent as MyParent;
    }

    public interface ISystem
    {
        public ECTValidation?[] Validations { get; }
        public void Initialize();
        public void Update();
        public void SetReference(IReference reference);
    }
}