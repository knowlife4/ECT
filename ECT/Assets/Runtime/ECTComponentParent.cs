using System.Collections.Generic;

namespace ECT
{
    public abstract class ECTComponentParent<TRoot, TParent, TComponentParent> : ECTComponent<TRoot, TParent>, IParent
    where TRoot : class, IRoot
    where TParent : class, IParent
    where TComponentParent : class, IComponent, IParent
    {
        public ECTComponentGroup<ECTComponent<TRoot, TComponentParent>> ComponentGroup;
        IComponentGroup IParent.ComponentGroup => ComponentGroup;

        internal override IComponent[] GetComponents()
        {
            List<IComponent> components = new();
            
            components.AddRange(ComponentGroup.GetComponents());
            components.Add(this);

            return components.ToArray();
        }

        protected override ECTSystemData CreateSystemData(TRoot root, TParent parent, ISystem system) => new ComponentParentSystemData(root, parent, this, system, ComponentGroup);

        public abstract class Component : ECTComponent<TRoot, TComponentParent> { }

        public class ComponentParentSystemData : ECTSystemData
        {
            public ECTSystemDataGroup DataGroup;

            public override TSystem GetSystem<TSystem>()
            {
                if (System is TSystem system) return system;

                return DataGroup.GetSystem<TSystem>();
            }

            public ComponentParentSystemData(IRoot root, IParent parent, IComponent component, ISystem system, ECTComponentGroup<ECTComponent<TRoot, TComponentParent>> componentGroup) : base(root, parent, component, system)
            {
                DataGroup = new(componentGroup.Components);
            }
        }

        public new abstract class System<TComponent> : ECTSystem<TRoot, TParent, TComponent, ComponentParentSystemData> where TComponent : class, IComponent, IParent
        {
            protected override void OnUpdate() => Data.DataGroup.Update(Root, Component);
        }
    }
}