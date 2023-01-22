using System.Collections.Generic;
using UnityEngine;

namespace ECT
{
    public abstract class ECTEntity<MyEntity> : MonoBehaviour, IEntity, IReferenceParent where MyEntity : class, IEntity, IReferenceParent
    {
        public ECTBranch<ECTComponent<MyEntity, MyEntity, EntityComponent>> ComponentBranch;
    
        private ECTReferenceBranch children;
        public ECTReferenceBranch Children => children ??= new(ComponentBranch.Components);
        IReferenceBranch IRoot.Children => Children;

        private static ECTEntityGroup<MyEntity> entityGroup;
        static ECTEntityGroup<MyEntity> EntityGroup => entityGroup ??= new();

        public void OnEnable() => EntityGroup.Tag(this as MyEntity);
        public void OnDisable() => EntityGroup.Untag(this as MyEntity);

        public abstract class EntityComponent : ECTComponent<MyEntity, MyEntity, EntityComponent> { }

        public ECTValidation QuerySystem<FindSystem>(out FindSystem find) where FindSystem : class, ISystem => Children.QuerySystem(out find);

        public void UpdateChildren() => Children.Update(this, this);

        public static MyEntity[] All => entityGroup.Entities;
        public IEntity[] GetAll() => All;
    }

    public interface IEntity : IRoot
    {
        public IEntity[] GetAll ();
    }
}