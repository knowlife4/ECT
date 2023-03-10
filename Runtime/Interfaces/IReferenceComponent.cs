using System.Collections.Generic;

namespace ECT
{
    public interface IReferenceComponent
    {
        public IEnumerable<IComponent> GetComponentsRecursively();
    }
}