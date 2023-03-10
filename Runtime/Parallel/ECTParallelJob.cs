using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace ECT.Parallel
{
    public interface IParallelData<TData> where TData : unmanaged, IParallelData<TData>
    {
        public TData Execute(NativeArray<TData> DataArray);
    }

    public static class ParallelConfigAPI
    {
        public static ParallelConfig<TData> Create<TData> (NativeArray<TData> dataArray) where TData : unmanaged, IParallelData<TData>
        {
            return new ParallelConfig<TData>
            {
                DataArray = dataArray
            };
        }
    }

    public struct ParallelConfig<TData> where TData : unmanaged, IParallelData<TData>
    {
        public NativeArray<TData> DataArray;

        public void Run(bool burst = false)
        {
            switch (burst)
            {
                case true:
                    RunBurst();
                    break;

                case false:
                    RunNonBurst();
                    break;
            }
        }

        void RunBurst()
        {
            JobHandle handle = new ParallelJobs.ECTParallelJobBurst{ DataArray = DataArray }.Schedule(DataArray.Length, 1);
            handle.Complete();
        }

        void RunNonBurst()
        {
            JobHandle handle = new ParallelJobs.ECTParallelJob{ DataArray = DataArray }.Schedule(DataArray.Length, 1);
            handle.Complete();
        }

        internal static class ParallelJobs
        {
            [BurstCompile(CompileSynchronously = true)]
            public struct ECTParallelJobBurst : IJobParallelFor
            {
                [NativeDisableParallelForRestriction] public NativeArray<TData> DataArray;

                public void Execute(int index)
                {
                    DataArray[index] = DataArray[index].Execute(DataArray);
                }
            }

            public struct ECTParallelJob : IJobParallelFor
            {
                [NativeDisableParallelForRestriction] public NativeArray<TData> DataArray;

                public void Execute(int index)
                {
                    DataArray[index] = DataArray[index].Execute(DataArray);
                }
            }
        }
    }
}