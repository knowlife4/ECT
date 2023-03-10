using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace ECT.Parallel
{
    public class ECTParallelScheduler<TParallelData> where TParallelData : unmanaged, IParallelData<TParallelData>
    {
        HashSet<IParallelSystem> currentSystems = new();
        List<TParallelData> currentData = new();
    
        public void Update (IParallelSystem current)
        {
            if(currentSystems.Contains(current))
            {
                Execute(current);
                return;
            }
            
            currentSystems.Add(current);
        }

        NativeArray<TParallelData> nativeJobData;

        void Execute(IParallelSystem current)
        {
            currentData.Capacity = currentSystems.Count;

            foreach (var system in currentSystems)
            {
                currentData.Add((TParallelData)system.ParallelData);
            }

            nativeJobData = new(currentData.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            nativeJobData.CopyFrom(currentData.ToArray());

            current.Schedule(nativeJobData);

            var iterator = 0;
            foreach (var system in currentSystems)
            {
                system.ParallelData = nativeJobData[iterator];
                system.OnComplete();
                iterator++;
            }

            nativeJobData.Dispose();
            currentSystems.Clear();
            currentData.Clear();
        }
    }
}