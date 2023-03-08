using System.Collections.Generic;

namespace ECT
{
    [System.Serializable]
    public class ECTComponentGroup<TComponent> : IComponentGroup where TComponent : IComponent
    {
        public TComponent[] Components;

        public IComponent[] GetComponents()
        {
            List<IComponent> components = new();
            foreach (TComponent component in Components)
            {
                if(component == null) continue;
                components.AddRange(component.GetComponents());
            }

            return components.ToArray();
        }
    }

    public interface IComponentGroup : IReferenceComponent
    {
        
    }
}