using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;

namespace ECT.UnityEditor
{
    [CustomEditor(typeof(ECTEntity<>), true)]
    public class EntityEditor : Editor
    {
        IEntity entity;
        IComponent[] components;

        ECTSceneReferenceGroup referenceGroup;

        int previousComponentLength;

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            base.OnInspectorGUI();
            
            if(EditorGUI.EndChangeCheck()) UpdateInfo();
        }

        void UpdateInfo()
        {
            entity = (IEntity)target;
            components = entity.GetComponents();

            referenceGroup = entity.ReferenceGroup;

            Populate();
        }

        void Populate()
        {
            List<ISceneReference> references = new();
            
            foreach (IComponent component in components)
            {
                Type type = component.SceneReferenceConstructor.Type;

                if(type == null) continue;

                if (referenceGroup.References == null) continue;

                ISceneReference foundReference = referenceGroup.References.FirstOrDefault(x => x.GetType() == type);

                if (foundReference != default)
                {
                    references.Add(foundReference);
                    continue;
                }

                ISceneReference reference = (ISceneReference)component.SceneReferenceConstructor.Create();
                
                references.Add(reference);
            }

            referenceGroup.References = references.ToArray();
        }
    }
}