using System.Collections.Generic;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

namespace ECT.Parallel
{
    public class ECTParallelScheduler
    {
        List<IParallelSystem> systems = new();
        int completed;

        public void Schedule (IParallelSystem system)
        {
            if(!systems.Contains(system)) systems.Add(system);
        }

        public void Execute<MyJob, MyData>(IParallelSystem current) where MyJob : unmanaged, IJobParallelFor where MyData : unmanaged, IParallelData<MyData>
        {
            if(systems.Count == 0) return;

            if(completed < systems.Count)
            {
                completed++;
                return;
            }

            completed = 0;

            List<MyData> allData = new();

            foreach (var system in systems)
            {
                allData.Add((MyData)system.CreateData());
            }

            NativeArray<MyData> nativeJobData = new(allData.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            nativeJobData.CopyFrom(allData.ToArray());

            MyJob job = (MyJob)current.CreateJob(nativeJobData);

            JobHandle jobHandle = job.Schedule(allData.Count, 1);

            jobHandle.Complete();

            for (int i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                system.OnComplete(nativeJobData[i]);
            }

            nativeJobData.Dispose();
        }
    }
}