using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace ECT.UnityEditor
{
    [CustomPropertyDrawer(typeof(ECTComponentGroup<>), true)]
    public class ComponentGroupEditor : PropertyDrawer
    {
        ECTEditorList componentList = new("Components", true, true);
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            SerializedProperty components = property.FindPropertyRelative("Components");
            componentList.DrawList(position, property, components);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return componentList.GetHeight() ?? base.GetPropertyHeight(property, label);
        }
    }
}