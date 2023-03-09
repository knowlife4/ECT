using System.Collections.Generic;
using UnityEngine;

namespace ECT
{
    public abstract class ECTEntity<TEntity> : MonoBehaviour, IEntity where TEntity : class, IEntity
    {
        public static TEntity[] All => Group.Entities;

        static ECTEntityGroup<TEntity> group;

        static ECTEntityGroup<TEntity> Group => group ??= new();

        [SerializeField]
        ECTComponentGroup<ECTComponent<TEntity, TEntity>> componentGroup;

        IComponentGroup IParent.ComponentGroup => componentGroup;

        [SerializeField]
        ECTSceneReferenceGroup referenceGroup;

        ECTSceneReferenceGroup IRoot.ReferenceGroup => referenceGroup;

        ECTSystemDataGroup dataGroup;

        public ECTSystemDataGroup DataGroup => dataGroup ??= new ECTSystemDataGroup(componentGroup.Components);

        public void UpdateSystems()
        {
            DataGroup.Update(this, this);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return dataGroup.GetSystem<TSystem>();
        }

        public IValidation QuerySystem<TSystem>(out TSystem system) where TSystem : class, ISystem
        {
            system = GetSystem<TSystem>();

            return new ECTBoolValidation(system != null);
        }

        public TReference? GetReference<TReference>() where TReference : struct, ISceneReference
        {
            return referenceGroup.Get<TReference>();
        }

        public IValidation QueryReference<TReference>(out TReference reference) where TReference : struct, ISceneReference
        {
            TReference? found = GetReference<TReference>();
            reference = found ?? default;

            return found != null ? ECTValidation.ValidateMany(reference.Validations) : new(false);
        }

        public IEnumerable<IComponent> GetComponentsRecursively()
        {
            return componentGroup.GetComponentsRecursively();
        }

        void OnEnable()
        {
            Group.Tag(this as TEntity);
        }

        void OnDisable()
        {
            Group.UnTag(this as TEntity);
        }

        public abstract class Component : ECTComponent<TEntity, TEntity> { }
    }

    public interface IEntity : IRoot, IReferenceComponent { }
}