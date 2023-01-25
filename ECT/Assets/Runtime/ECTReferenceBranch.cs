using System.Collections.Generic;

namespace ECT
{
    public class ECTReferenceBranch : IReferenceBranch
    {
        public ECTReferenceBranch(IComponent[] components) => Components = components;

        public IComponent[] Components { get; }
        public IReference[] References { get; private set; }

        public ECTAction OnPreUpdate { get; }
        public ECTAction OnPostUpdate { get; }

        public void Update (IRoot root, IParent parent)
        {
            if(References == null || Components.Length != References.Length) SetupReferences(root, parent);

            OnPreUpdate.Execute();
            UpdateReferences();
            OnPostUpdate.Execute();
        }

        public FindSystem Get<FindSystem>() where FindSystem : class, ISystem
        {
            if(References == null) return null;

            foreach (var reference in References)
            {
                FindSystem found = reference.Get<FindSystem>();
                if(found != null) return found;
            }

            return null;
        }

        public ECTValidation QuerySystem<FindSystem> (out FindSystem find) where FindSystem : class, ISystem
        {
            find = Get<FindSystem>();
            return new ECTValidation(Get<FindSystem>() != null);
        }

        void SetupReferences (IRoot root, IParent parent)
        {
            References = CreateReferences(root, parent);

            InitializeReferences();
        }

        IReference[] CreateReferences (IRoot root, IParent parent)
        {
            List<IReference> references = new();
            foreach (var component in Components)
            {
                references.Add(component.CreateReference(root, parent));
            }
            return references.ToArray();
        }

        void InitializeReferences ()
        {
            foreach (var reference in References)
            {
                InitializeReference(reference);
            }
        }

        void UpdateReferences ()
        {
            foreach (var reference in References)
            {
                UpdateReference(reference);
            }
        }

        void InitializeReference (IReference reference)
        {
            ISystem system = reference.System;
            if(system.IsValid()) system.Initialize();
        }

        void UpdateReference (IReference reference)
        {
            ISystem system = reference.System;
            if(system.IsValid()) system.Update();
        }
    }

    public interface IReferenceBranch : IReferenceParent
    {
        public ECTAction OnPreUpdate { get; }
        public ECTAction OnPostUpdate { get; }
    }
}