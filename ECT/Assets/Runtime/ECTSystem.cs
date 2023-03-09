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
        protected TSystemData Data { get; private set; }

        protected TRoot Root => Data.Info.Root as TRoot;
        protected TParent Parent => Data.Info.Parent as TParent;
        protected TComponent Component => Data.Component as TComponent;

        public void SetData(ECTSystemData data) => Data = (TSystemData)data;

        public IValidation IsValid() => ECTValidation.ValidateMany(Validations);
        protected virtual IValidation[] Validations => System.Array.Empty<IValidation>();

        public void Initialize() => OnInitialize();
        protected virtual void OnInitialize() { }

        public void Update() => OnUpdate();
        protected abstract void OnUpdate();

        public IValidation QuerySystem<TSystem>(out TSystem found) where TSystem : class, ISystem => Root.QuerySystem(out found);
        public IValidation QueryReference<TReference>(out TReference found) where TReference : struct, ISceneReference => Root.QueryReference(out found);
        public IValidation ValidateReference<T>(T input, out T output) where T : class => ECTValidation.ValidateReference(input, out output);
        public ECTBoolValidation Subscribe(ECTAction ectAction, Action action)
        {
            ectAction.Subscribe(action);
            return new(true);
        }
        
        public abstract class Parallel<TParallelData> : ECTParallelSystem<TRoot, TParent, TComponent, TSystemData, TParallelData> 
        where TParallelData : unmanaged, IParallelData<TParallelData> { }
    }

    public interface ISystem
    {
        public IValidation IsValid();
        
        public void Initialize();
        public void Update();
        public void SetData(ECTSystemData data);
    }
}