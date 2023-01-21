using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace ECT.Parallel
{
    public class ECTParallelScheduler<MyData>
    where MyData : unmanaged, IParallelData<MyData>
    {
        internal Dictionary<IParallelSystem, MyData> systems = new();
        int completed;

        public void Schedule (IParallelSystem system)
        {
            if(!systems.ContainsKey(system)) systems.Add(system, (MyData)system.Data);
        }

        public void Execute(IParallelSystem current)
        {
            if(systems.Count == 0) return;

            if(completed < systems.Count)
            {
                completed++;
                return;
            }

            completed = 0;

            NativeArray<MyData> nativeJobData = new(systems.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            nativeJobData.CopyFrom(systems.Values.ToArray());

            current.Schedule(nativeJobData);

            var iterator = 0;
            foreach (var system in systems.Keys)
            {
                system.Data = nativeJobData[iterator];
                system.OnComplete();
                iterator++;
            }

            nativeJobData.Dispose();
        }
    }
}