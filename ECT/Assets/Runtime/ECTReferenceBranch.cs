using System.Collections.Generic;

namespace ECT
{
    public class ECTReferenceBranch : IReferenceBranch
    {
        public ECTReferenceBranch(IComponent[] components)
        {
            Components = components;
        }

        public IComponent[] Components { get; }
        
        public IReference[] References { get; private set; }

        void CreateReferences (IParent root, IParent parent)
        {
            List<IReference> references = new();
            foreach (var component in Components)
            {
                references.Add(component.CreateReference(root, parent));
            }

            References = references.ToArray();
        }

        void UpdateReferences ()
        {
            foreach (var reference in References)
            {
                ISystem system = reference.System;
                bool passed = true;

                foreach (var validation in system.Validations)
                {
                    if(validation.Successful == true) continue;
                    passed = false;
                    break;
                }

                if(passed) reference.System.Update();
            }
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
            return new ECTValidation(find != null);
        }

        public void Update (IParent root, IParent parent)
        {
            if(References == null || Components.Length != References.Length) CreateReferences(root, parent);

            UpdateReferences();
        }
    }

    public interface IReferenceBranch : IReferenceParent
    {

    }
}