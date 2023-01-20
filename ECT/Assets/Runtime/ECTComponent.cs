using UnityEngine;
using ECT.Parallel;

namespace ECT
{
    public abstract class ECTComponent<MyRoot, MyParent, MyComponent> : ScriptableObject, IComponent where MyRoot : class, IParent, IReferenceParent where MyParent : class, IParent where MyComponent : class, IComponent
    {
        protected MyComponent ThisComponent => this as MyComponent;

        protected abstract ISystem System { get; }
        public ISystem CreateSystem() => System;

        protected virtual IReference CreateReference (MyRoot root, MyParent parent, ISystem system) => new ComponentReference(ThisComponent, root, parent, system);
        public IReference CreateReference(IParent root, IParent parent)
        {
            ISystem system = System;
            IReference reference = CreateReference((MyRoot)root, (MyParent)parent, system);
            system.SetReference(reference);
            system.Initialize();

            return reference;
        }

        public abstract class ComponentSystem<Component> : ECTSystem<ComponentReference, Component, MyRoot, MyParent> where Component : class, IComponent {}

        public abstract class ComponentParallelSystem<Component, MyData> : ECTParallelSystem<ComponentReference, Component, MyRoot, MyParent, MyData, ECTParallelJob<MyData>> where Component : class, IComponent where MyData : unmanaged, IParallelData {}

        public abstract class ComponentGroup : ECTComponentGroup<MyRoot, MyParent, MyComponent, ComponentGroup> {}

        public class ComponentReference : ECTReference<MyRoot, MyParent, MyComponent>
        {
            public ComponentReference(MyComponent reference, MyRoot root, MyParent parent, ISystem system) : base(reference, root, parent, system) {}
        }
    }

    public interface IComponent
    {
        public ISystem CreateSystem();
        public IReference CreateReference(IParent root, IParent parent);
    }
}