namespace ECT
{
    public abstract class ECTSystem<MyReference, MyComponent, MyRoot, MyParent> : ISystem, IReferenceParent
    where MyReference : IReference
    where MyComponent : class, IComponent
    where MyRoot : class, IRoot
    where MyParent : class, IParent
    {
        public MyReference Reference { get; private set; }

        protected virtual ECTValidation[] Validations => System.Array.Empty<ECTValidation>();
        public ECTValidation[] GetValidations() => Validations;

        public bool IsValid()
        {
            bool passed = true;
            foreach (var validation in GetValidations())
            {
                if(validation.Successful == true) continue;
                passed = false;
                break;
            }

            return passed;
        }

        protected virtual void OnInitialize() {}
        public void Initialize() => OnInitialize();

        protected virtual void OnUpdate() {}
        public void Update() => OnUpdate();

        public void SetReference(IReference reference) => Reference = (MyReference)reference;

        public ECTValidation QuerySystem<FindSystem>(out FindSystem find) where FindSystem : class, ISystem => Root.QuerySystem(out find);
        public ECTValidation Validate<T>(T input, out T output) where T : class => ECTValidation.Validate(input, out output);
        public ECTValidation Subscribe(ECTAction ectAction, System.Action action)
        {
            ectAction.Subscribe(action);
            return new(true);
        }

        public MyComponent Component => Reference.Component as MyComponent;
        public MyRoot Root => Reference.Root as MyRoot;
        public MyParent Parent => Reference.Parent as MyParent;
    }

    public interface ISystem
    {
        public ECTValidation[] GetValidations();
        public bool IsValid();
        public void Initialize();
        public void Update();
        public void SetReference(IReference reference);
    }
}