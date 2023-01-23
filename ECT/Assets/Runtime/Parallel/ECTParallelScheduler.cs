using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace ECT.Parallel
{
    public class ECTParallelScheduler<MyData>
    where MyData : unmanaged, IParallelData<MyData>
    {
        HashSet<IParallelSystem> currentSystems = new();
        List<MyData> currentData = new();
    
        public void Update (IParallelSystem current)
        {
            if(currentSystems.Contains(current))
            {
                Execute(current);
                return;
            }
            
            currentSystems.Add(current);
        }

        NativeArray<MyData> nativeJobData;

        void Execute(IParallelSystem current)
        {
            currentData.Capacity = currentSystems.Count;

            foreach (var system in currentSystems)
            {
                currentData.Add((MyData)system.Data);
            }

            nativeJobData = new(currentData.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            nativeJobData.CopyFrom(currentData.ToArray());

            current.Schedule(nativeJobData);

            var iterator = 0;
            foreach (var system in currentSystems)
            {
                system.Data = nativeJobData[iterator];
                system.OnComplete();
                iterator++;
            }

            nativeJobData.Dispose();
            currentSystems.Clear();
            currentData.Clear();
        }
    }
}