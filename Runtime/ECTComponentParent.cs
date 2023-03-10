using System.Collections.Generic;
using UnityEngine;

namespace ECT
{
    public abstract class ECTComponentParent<TRoot, TParent, TComponentParent> : ECTComponent<TRoot, TParent>, IParent
    where TRoot : class, IRoot
    where TParent : class, IParent
    where TComponentParent : class, IComponent, IParent
    {
        [SerializeField]
        ECTComponentGroup<ECTComponent<TRoot, TComponentParent>> componentGroup;

        public IComponentGroup ComponentGroup => componentGroup;

        public override IEnumerable<IComponent> GetComponentsRecursively()
        {
            yield return this;

            foreach (IComponent component in ComponentGroup.GetComponentsRecursively()) yield return component;
        }

        protected override ECTSystemData CreateSystemData(ECTSystemData.SystemInfo info) => new ComponentParentSystemData(info, this, componentGroup);


        public class ComponentParentSystemData : ECTSystemData
        {
            public ComponentParentSystemData(SystemInfo info, IComponent component, ECTComponentGroup<ECTComponent<TRoot, TComponentParent>> componentGroup) : base(info, component)
            {
                DataGroup = new(componentGroup.Components);
            }

            public ECTSystemDataGroup DataGroup { get; }

            public override TSystem GetSystem<TSystem>()
            {
                if (Info.System is TSystem system) return system;

                return DataGroup.GetSystem<TSystem>();
            }
        }

        public new abstract class System<TComponent> : ECTSystem<TRoot, TParent, TComponent, ComponentParentSystemData> where TComponent : class, IComponent, IParent
        {
            public void UpdateSystems() => Data.DataGroup.Update(Root, Component);
            protected override void OnUpdate() => UpdateSystems();
        }

        public abstract class Component : ECTComponent<TRoot, TComponentParent> { }
    }
}