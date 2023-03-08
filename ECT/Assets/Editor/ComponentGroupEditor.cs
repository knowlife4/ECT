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
        ReorderableList componentList;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if(componentList == null) PrepareList(property);

            componentList.DoList(position);

            EditorGUI.EndProperty();
        }

        public void PrepareList (SerializedProperty property)
        {
            var components = property.FindPropertyRelative("Components");
            componentList = new(property.serializedObject, components, false, true, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.PropertyField(rect, components.GetArrayElementAtIndex(index));
                },

                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Components");
                }
            };
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return componentList?.GetHeight() ?? base.GetPropertyHeight(property, label);
        }
    }
}