using System.Collections.Generic;

namespace ECT
{
    [System.Serializable]
    public class ECTComponentGroup<TComponent> : IComponentGroup where TComponent : IComponent
    {
        public TComponent[] Components = System.Array.Empty<TComponent>();

        public IEnumerable<IComponent> GetComponentsRecursively()
        {
            foreach (TComponent childComponent in Components)
            {
                if(childComponent == null) continue;

                foreach (IComponent component in childComponent.GetComponentsRecursively())
                {
                    yield return component;
                }
            }
        }
    }

    public interface IComponentGroup : IReferenceComponent { }
}