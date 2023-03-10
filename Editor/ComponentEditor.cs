using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace ECT.UnityEditor
{
    [CustomPropertyDrawer(typeof(ECTComponent<,>), true)]
    public class ComponentEditor : PropertyDrawer
    {
        Editor editor;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, new(), true);
    
            // Make child fields be indented
            EditorGUI.indentLevel++;

            // background
            GUILayout.BeginVertical("box");

            if (!editor)
                Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);

            // Draw object properties
            EditorGUI.BeginChangeCheck();
            if (editor) // catch empty property
            {
                editor.OnInspectorGUI ();
            }
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            GUILayout.EndVertical ();

            // Set indent back to what it was
            EditorGUI.indentLevel--;
        }
    }
}