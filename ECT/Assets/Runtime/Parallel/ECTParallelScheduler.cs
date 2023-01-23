using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace ECT.Parallel
{
    public class ECTParallelScheduler<MyData>
    where MyData : unmanaged, IParallelData<MyData>
    {
        HashSet<IParallelSystem> systems = new();
        List<MyData> currentData = new();

        public void Schedule (IParallelSystem system)
        {
            if(systems.Contains(system)) return;
            systems.Add(system);
        }

        IParallelSystem firstSystem = null;

        public void Update (IParallelSystem current)
        {
            if(firstSystem == current) Execute(current);
            firstSystem ??= current;
        }

        NativeArray<MyData> nativeJobData;

        void Execute(IParallelSystem current)
        {
            foreach (var system in systems)
            {
                if(!system.Execute) return;
                currentData.Add((MyData)system.Data);
            }

            nativeJobData = new(currentData.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            nativeJobData.CopyFrom(currentData.ToArray());

            current.Schedule(nativeJobData);

            var iterator = 0;
            foreach (var system in systems)
            {
                if(!system.Execute) return;
                system.Data = nativeJobData[iterator];
                system.OnComplete();
                iterator++;
            }

            nativeJobData.Dispose();
            currentData.Clear();
            firstSystem = null;
        }
    }
}