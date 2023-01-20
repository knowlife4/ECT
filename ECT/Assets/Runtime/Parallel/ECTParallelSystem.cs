using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace ECT.Parallel
{
    public abstract class ECTParallelSystem<MyReference, MyComponent, MyRoot, MyParent, MyData, MyJob> : ECTSystem<MyReference, MyComponent, MyRoot, MyParent>, IParallelSystem where MyReference : IReference where MyComponent : class, IComponent where MyRoot : class, IParent, IReferenceParent where MyParent : class, IParent where MyData : unmanaged, IParallelData<MyData> where MyJob : unmanaged, IJobParallelFor
    {
        static ECTParallelScheduler scheduler = new();

        protected sealed override void OnInitialize() => scheduler.Schedule(this);

        protected sealed override void OnUpdate() => scheduler.Execute<MyJob, MyData>(this);

        public abstract MyData Data { get; }
        object IParallelSystem.CreateData() => Data;

        public abstract MyJob Job { get; set; }
        public abstract MyJob CreateJob(NativeArray<MyData> dataArray);
        public IJobParallelFor CreateJob(object dataArray) => CreateJob((NativeArray<MyData>)dataArray);

        public abstract void OnComplete (MyData data);
        public void OnComplete(object data) => OnComplete((MyData)data);
    }

    [BurstCompile]
    public struct ECTParallelJob<MyData> : IJobParallelFor where MyData : unmanaged, IParallelData<MyData>
    {
        public ECTParallelJob(NativeArray<MyData> thisData) => data = thisData;

        NativeArray<MyData> data;

        public void Execute(int index) => data[index] = data[index].Execute();
    }

    public interface IParallelSystem
    {
        public IJobParallelFor CreateJob(object dataArray);
        public object CreateData();
        public void OnComplete (object data);
    }
}