using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ECT
{
    public class ECTSystemDataGroup : IReferenceSystem
    {
        public ECTSystemDataGroup(IComponent[] components)
        {
            Components = components;
        }
        
        public IComponent[] Components { get; }
        
        public ECTSystemData[] SystemsData;

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem => SystemsData.Select(data => data.GetSystem<TSystem>()).FirstOrDefault(system => system != null);

        public void Update(IRoot root, IParent parent)
        {
            if (SystemsData == null || SystemsData.Length != Components.Length) SetupSystems(root, parent);
            UpdateSystems();
        }

        public void SetupSystems(IRoot root, IParent parent)
        {
            SystemsData = CreateSystems(root, parent);
            InitializeSystems();
        }

        ECTSystemData[] CreateSystems(IRoot root, IParent parent)
        {
            List<ECTSystemData> data = new();
            foreach (var component in Components)
            {
                data.Add(component.CreateSystemData(root, parent));
            }

            return data.ToArray();
        }
        
        void InitializeSystems ()
        {
            foreach (ECTSystemData data in SystemsData)
            {
                InitializeSystem(data);
            }
        }
        
        void UpdateSystems ()
        {
            foreach (ECTSystemData data in SystemsData)
            {
                UpdateSystem(data);
            }
        }
        
        void InitializeSystem (ECTSystemData data)
        {
            ISystem system = data.System;
            
            if(system.IsValid().Successful) system.Initialize();
        }
        
        void UpdateSystem (ECTSystemData data)
        {
            ISystem system = data.System;
            
            if(system.IsValid().Successful) system.Update();
        }
    }
}