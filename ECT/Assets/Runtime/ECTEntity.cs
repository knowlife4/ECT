using UnityEngine;

namespace ECT
{
    public abstract class ECTEntity<MyEntity> : MonoBehaviour, IEntity where MyEntity : class, IEntity
    {
        public ECTBranch<ECTComponent<MyEntity, MyEntity, EntityComponent>> ComponentBranch;

        public ECTReferenceBranch ReferenceBranch { get; set; }

        public abstract class EntityComponent : ECTComponent<MyEntity, MyEntity, EntityComponent> {}

        void Awake ()
        {
            ReferenceBranch = new(ComponentBranch.Components);
        }
    }

    public interface IEntity : IParent
    {

    }
}