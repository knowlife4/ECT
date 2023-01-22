using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace ECT.Parallel
{
    public class ECTParallelScheduler<MyData>
    where MyData : unmanaged, IParallelData<MyData>
    {
        internal HashSet<IParallelSystem> systems = new();
        internal Dictionary<IParallelSystem, MyData> data = new();

        public void Schedule (IParallelSystem system)
        {
            if(systems.Contains(system)) return;
            systems.Add(system);
            data.Add(system, (MyData)system.Data);
        }

        List<MyData> currentData = new();

        NativeArray<MyData> nativeJobData;

        public void Execute(IParallelSystem current)
        {
            if(systems.Count == 0) return;

            foreach (var system in systems)
            {
                if(!system.Execute) return;
                currentData.Add(data[system]);
            }
            
            if(currentData.Count == 0) return;

            nativeJobData = new(currentData.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            nativeJobData.CopyFrom(currentData.ToArray());

            current.Schedule(nativeJobData);
            
            var iterator = 0;
            foreach (var system in systems)
            {
                system.Data = nativeJobData[iterator];
                system.OnComplete();
                iterator++;
            }

            nativeJobData.Dispose();
            currentData.Clear();
        }
    }
}