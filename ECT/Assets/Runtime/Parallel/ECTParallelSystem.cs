using Unity.Collections;

namespace ECT.Parallel
{
    public abstract class ECTParallelSystem<MyReference, MyComponent, MyRoot, MyParent, MyData> : ECTSystem<MyReference, MyComponent, MyRoot, MyParent>, IParallelSystem
    where MyReference : IReference
    where MyComponent : class, IComponent
    where MyRoot : class, IParent, IReferenceParent
    where MyParent : class, IParent
    where MyData : unmanaged, IParallelData<MyData>
    {
        static internal ECTParallelScheduler<MyData> scheduler = new();
        private MyData data;

        public object Data
        {
            get
            {
                return data;
            }
            set
            {
                data = (MyData)value;
            }
        }

        protected sealed override void OnInitialize() => scheduler.Schedule(this);
        protected sealed override void OnUpdate()
        {
            UpdateData(ref data);
            scheduler.systems[this] = data;
            scheduler.Execute(this);
        }

        public abstract void UpdateData(ref MyData data);

        public abstract void OnComplete(MyData data);
        public void OnComplete() => OnComplete(data);

        public abstract void Schedule(NativeArray<MyData> dataArray);
        public void Schedule(object dataArrayObject) => Schedule((NativeArray<MyData>)dataArrayObject);
    }

    public interface IParallelSystem
    {
        public object Data { get; set; }
        public void Schedule (object dataArrayObject);
        public void OnComplete ();
    }
}