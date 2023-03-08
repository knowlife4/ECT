using System;
using System.Linq.Expressions;
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

        ECTDynamicConstructor<ISystem> SystemConstructor { get; }

        IDynamicConstructor IComponent.SceneReferenceConstructor => SceneReferenceConstructor;
        ECTDynamicConstructor<ISceneReference> SceneReferenceConstructor { get; }
        
        protected virtual ISystem CreateSystem() => SystemConstructor.Create();
        
        public virtual ECTSystemData CreateSystemData(IRoot root, IParent parent)
        {
            ISystem system = CreateSystem();
            ECTSystemData data = CreateSystemData((TRoot)root, (TParent)parent, system);
            system.SetData(data);

            return data;
        }

        protected virtual ECTSystemData CreateSystemData(TRoot root, TParent parent, ISystem system) => new ECTSystemData(root, parent, this, system);

        public abstract class System<TComponent> : ECTSystem<TRoot, TParent, TComponent, ECTSystemData> where TComponent : class, IComponent { }

        public abstract class Parent : ECTComponentParent<TRoot, TParent, Parent> { }
        
        internal virtual IComponent[] GetComponents() => new[] { this };
        IComponent[] IReferenceComponent.GetComponents() => GetComponents();
    }

    public interface IComponent : IReferenceComponent
    {
        public IDynamicConstructor SceneReferenceConstructor { get; }

        public ECTSystemData CreateSystemData(IRoot root, IParent parent);
    }
}