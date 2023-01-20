using Unity.Jobs;
using UnityEngine;

namespace ECT
{
    public abstract class ECTComponentGroup<MyRoot, MyParent, MyComponent, MyComponentGroup> : ECTComponent<MyRoot, MyParent, MyComponent>, IParent where MyRoot : class, IParent, IReferenceParent where MyParent : class, IParent where MyComponent : class, IComponent where MyComponentGroup : class, IComponent, IParent
    {
        public ECTBranch<ChildComponent> Components;

        protected override IReference CreateReference(MyRoot root, MyParent parent, ISystem system) => new ComponentGroupReference(this as MyComponentGroup, root, parent, system, Components);

        public abstract class ChildComponent : ECTComponent<MyRoot, MyComponentGroup, ChildComponent> {}

        public class ComponentGroupReference : ECTReference<MyRoot, MyParent, MyComponentGroup>
        {
            public ECTReferenceBranch ReferenceBranch { get; }

            public ComponentGroupReference(MyComponentGroup reference, MyRoot root, MyParent parent, ISystem system, ECTBranch<ChildComponent> branch) : base(reference, root, parent, system) => ReferenceBranch = new(branch.Components);

            public override FindSystem Get<FindSystem>() => System is FindSystem system ? system : ReferenceBranch.Get<FindSystem>();
        }

        public new abstract class ComponentSystem<Component> : ECTSystem<ComponentGroupReference, Component, MyRoot, MyParent> where Component : class, IComponent 
        {
            protected override void OnUpdate() => Reference.ReferenceBranch.Update(Reference.Root, Reference.Component);
        }
    }
}