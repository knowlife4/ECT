using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ECT.UnityEditor
{
    public class CreationEditor : EditorWindow
    {
        [MenuItem("Assets/Create/ECT/Component Instance", false, -10)]
        static void Open()
        {
            CreationEditor window = CreateInstance<CreationEditor>();
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 250, 150);
            window.ShowPopup();
        }
        
        static (Type[], string[]) GetType(Type baseType)
        {
            List<Type> foundTypes = new();
            List<string> entries = new();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach (TypeInfo type in assembly.DefinedTypes)
                {
                    if (!baseType.IsAssignableFrom(type) || !type.IsClass || type.IsAbstract) continue;
                    
                    foundTypes.Add(type);
                    entries.Add(string.Concat(type.Name.Select(x => char.IsUpper(x) ? "/" + x : x.ToString()))[1..]);
                }
            }

            return (foundTypes.ToArray(), entries.ToArray());
        }
        
        Type[] AllComponents { get; set; }
        string[] ComponentEntries { get; set; }

        int selectedComponent;
        
        string Name { get; set;  }

        protected void OnEnable()
        {
            (AllComponents, ComponentEntries) = GetType(typeof(IComponent));
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal("box");
            GUIStyle style = EditorStyles.boldLabel;
            style.alignment = TextAnchor.UpperCenter;
            style.fontSize = 18;

            EditorGUILayout.LabelField("Component Wizard", style);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginVertical("textArea");
            
            EditorGUILayout.PrefixLabel("Component:");
            
            selectedComponent = EditorGUILayout.Popup(selectedComponent, ComponentEntries);

            GUILayout.EndVertical();
            
            GUILayout.FlexibleSpace();
            
            GUILayout.BeginVertical("box");
            
            EditorGUILayout.LabelField("Name:");
            Name = EditorGUILayout.TextField(Name);
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Create")) CreateComponent();
            if (GUILayout.Button("Cancel")) Close();
            
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
        }

        void CreateComponent()
        {
            Type selected = AllComponents[selectedComponent];

            ScriptableObject scriptableObject = CreateInstance(selected);

            string filePath = Selection.assetGUIDs.Length == 0 ? "Assets/New TMP Color Gradient.asset" : AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            
            AssetDatabase.CreateAsset(scriptableObject, $"{filePath}/{Name}.asset");
            AssetDatabase.SaveAssets();

            Close();
        }
    }
}