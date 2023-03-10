using System.Collections.Generic;
using Unity.Collections;

namespace ECT.Parallel
{
    public abstract class ECTParallelSystem<TRoot, TParent, TComponent, TSystemData, TParallelData> : ECTSystem<TRoot, TParent, TComponent, TSystemData>, IParallelSystem
    where TRoot : class, IRoot
    where TParent : class, IParent
    where TComponent : class, IComponent
    where TSystemData : ECTSystemData
    where TParallelData : unmanaged, IParallelData<TParallelData>
    {
        static ECTParallelScheduler<TParallelData> Scheduler { get; } = new();

        private TParallelData parallelData;

        public object ParallelData
        {
            get => parallelData;
            set => parallelData = (TParallelData) value;
        }

        protected sealed override void OnUpdate()
        {
            PopulateData(ref parallelData);

            Scheduler.Update(this);
        }

        protected abstract void PopulateData(ref TParallelData data);
        
        public void OnComplete() => ExtractData(parallelData);
        protected abstract void ExtractData(TParallelData data);
        
        public void Schedule(object dataArrayObject) => Schedule((NativeArray<TParallelData>)dataArrayObject);

        protected abstract void Schedule(NativeArray<TParallelData> dataArray);
    }

    public interface IParallelSystem
    {
        public object ParallelData { get; set; }
        public void Schedule (object dataArrayObject);
        public void OnComplete ();
    }
}