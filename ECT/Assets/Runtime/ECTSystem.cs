using System;
using System.Linq;
using ECT.Parallel;

namespace ECT
{
    public abstract class ECTSystem<TRoot, TParent, TComponent, TSystemData> : ISystem
        where TParent : class, IParent
        where TRoot : class, IRoot
        where TComponent : class, IComponent
        where TSystemData : ECTSystemData
    {
        public TSystemData Data { get; private set; }

        protected TRoot Root => Data.Root as TRoot;

        protected TParent Parent => Data.Parent as TParent;
        protected TComponent Component => Data.Component as TComponent;

        public void SetData(ECTSystemData data) => Data = (TSystemData)data;

        public ECTValidation IsValid() => new(Validations.All(validation => validation.Successful == true));
        protected virtual ECTValidation[] Validations => System.Array.Empty<ECTValidation>();

        public void Initialize() => OnInitialize();
        protected virtual void OnInitialize() { }

        public void Update() => OnUpdate();
        protected abstract void OnUpdate();

        public ECTValidation QuerySystem<TSystem>(out TSystem find) where TSystem : class, ISystem => Root.QuerySystem(out find);
        public ECTValidation Validate<T>(T input, out T output) where T : class => ECTValidation.Validate(input, out output);
        public ECTValidation Subscribe(ECTAction ectAction, Action action)
        {
            ectAction.Subscribe(action);
            return new(true);
        }
        
        public abstract class Parallel<TParallelData> : ECTParallelSystem<TRoot, TParent, TComponent, TSystemData, TParallelData> 
        where TParallelData : unmanaged, IParallelData<TParallelData> { }
    }

    public interface ISystem
    {
        public ECTValidation IsValid();
        
        public void Initialize();
        public void Update();
        public void SetData(ECTSystemData data);
    }
}