using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECT
{
    public abstract class ECTComponent<TRoot, TParent> : ScriptableObject, IComponent
    where TRoot : class, IRoot
    where TParent : class, IParent
    {
        protected ECTComponent()
        {
            Type type = GetType();

            SystemConstructor = new(ComponentSystemAttribute.Find(type));
            SceneReferenceConstructor = new(SceneReferenceAttribute.Find(type));
        }

        ECTConstructor<ISystem> SystemConstructor { get; }

        ECTConstructor<ISceneReference> SceneReferenceConstructor { get; }

        public IDynamicConstructor GetSystemConstructor() => SystemConstructor;

        public IDynamicConstructor GetSceneReferenceConstructor() => SceneReferenceConstructor;

        public ECTSystemData CreateSystemData(IRoot root, IParent parent)
        {
            ISystem system = CreateSystem();
            ECTSystemData data = CreateSystemData(new((TRoot)root, (TParent)parent, system));
            system.SetData(data);

            return data;
        }

        protected virtual ISystem CreateSystem() => SystemConstructor.Create();

        protected virtual ECTSystemData CreateSystemData(ECTSystemData.SystemInfo info) => new(info, this);

        public virtual IEnumerable<IComponent> GetComponentsRecursively()
        {
            yield return this;
        }
        
        protected static IValidation ValidateReferences(params UnityEngine.Object[] references) => ECTValidation.ValidateReferences(references);
        protected static IValidation ValidateReference<T>(T input, out T output) where T : UnityEngine.Object => ECTValidation.ValidateReference(input, out output);

        public abstract class System<TComponent> : ECTSystem<TRoot, TParent, TComponent, ECTSystemData> where TComponent : class, IComponent { }

        public abstract class Parent : ECTComponentParent<TRoot, TParent, Parent> { }
    }

    public interface IComponent : IReferenceComponent
    {
        IDynamicConstructor GetSystemConstructor();

        IDynamicConstructor GetSceneReferenceConstructor();

        ECTSystemData CreateSystemData(IRoot root, IParent parent);
    }
}