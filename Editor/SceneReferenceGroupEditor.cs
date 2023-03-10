using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace ECT.UnityEditor
{
    [CustomPropertyDrawer(typeof(ECTSceneReferenceGroup))]
    public class SceneReferenceGroupEditor : PropertyDrawer
    {
        ECTEditorList sceneReferenceList = new("Scene References", false, false);
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            SerializedProperty references = property.FindPropertyRelative("References");
            
            for (int i = 0; i < references.arraySize; i++)
            {
                DrawElement(i, references);
            }

            EditorGUI.EndProperty();
        }

        public void DrawElement(int index, SerializedProperty listProperty)
        {
            SerializedProperty baseProp = listProperty.GetArrayElementAtIndex(index);
            string name = baseProp.managedReferenceFullTypename.Split(' ').Last().Split('.').Last();

            IEnumerator enumerator = baseProp.GetEnumerator();
            
            EditorGUI.indentLevel++;
            
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal("textArea");
            GUILayout.Label(name);
            GUILayout.EndHorizontal();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current is not SerializedProperty prop) continue;
                EditorGUILayout.PropertyField(prop);
            }
            
            GUILayout.EndVertical();
            
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return sceneReferenceList.GetHeight() ?? base.GetPropertyHeight(property, label);
        }
    }
}