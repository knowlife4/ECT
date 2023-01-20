using UnityEngine;

namespace ECT
{
    public abstract class ECTEntity<MyEntity> : MonoBehaviour, IEntity, IReferenceParent where MyEntity : class, IEntity, IReferenceParent
    {
        public ECTBranch<ECTComponent<MyEntity, MyEntity, EntityComponent>> ComponentBranch;

        private ECTReferenceBranch referenceBranch;

        public ECTReferenceBranch ReferenceBranch
        {
            get
            {
                if(referenceBranch == null) referenceBranch = new(ComponentBranch.Components);
                return referenceBranch;
            }
        }

        public abstract class EntityComponent : ECTComponent<MyEntity, MyEntity, EntityComponent> { }

        public ECTValidation QuerySystem<FindSystem>(out FindSystem find) where FindSystem : class, ISystem => ReferenceBranch.QuerySystem(out find);

        protected void UpdateComponents () => referenceBranch.Update(this, this);
    }

    public interface IEntity : IParent
    {

    }
}