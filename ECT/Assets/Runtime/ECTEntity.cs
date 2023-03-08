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
        
        public void UpdateSystems() => DataGroup.Update(this, this);

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem => dataGroup.GetSystem<TSystem>();
        public ECTValidation QuerySystem<TSystem>(out TSystem system) where TSystem : class, ISystem
        {
            system = GetSystem<TSystem>();

            return new(system != null);
        }

        public TReference? GetReference<TReference>() where TReference : struct, ISceneReference => referenceGroup.Get<TReference>();
        public ECTValidation QueryReference<TReference>(out TReference reference) where TReference : struct, ISceneReference
        {
            TReference? found = GetReference<TReference>();
            reference = found ?? default;

            return found != null ? ECTValidation.Validate(reference.Validations) : new(false);
        }

        void OnEnable() => Group.Tag(this as TEntity);
        void OnDisable() => Group.UnTag(this as TEntity);
        
        public abstract class Component : ECTComponent<TEntity, TEntity> { }

        public IComponent[] GetComponents() => componentGroup.GetComponents();
    }

    public interface IEntity : IRoot, IReferenceComponent
    {
        
    }
}