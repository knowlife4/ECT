using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace ECT.Parallel
{
    public interface IParallelData<MyData> where MyData : unmanaged, IParallelData<MyData>
    {
        public MyData Execute();
    }

    public static class API
    {
        public static Config<MyData> ParallelJobExecute<MyData> (NativeArray<MyData> dataArray) where MyData : unmanaged, IParallelData<MyData>
        {
            return new Config<MyData>
            {
                DataArray = dataArray
            };
        }
    }

    public struct Config<MyData> where MyData : unmanaged, IParallelData<MyData>
    {
        public NativeArray<MyData> DataArray;

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
                public NativeArray<MyData> DataArray;

                public void Execute(int index)
                {
                    DataArray[index] = DataArray[index].Execute();
                }
            }

            public struct ECTParallelJob : IJobParallelFor
            {
                public NativeArray<MyData> DataArray;

                public void Execute(int index)
                {
                    DataArray[index] = DataArray[index].Execute();
                }
            }
        }
    }
}