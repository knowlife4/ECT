using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ECT.UnityEditor
{
    public class ComponentWizard : EditorWindow
    {
        [MenuItem("Assets/ECT/Component Wizard", false, -10)]
        static void Open()
        {
            ComponentWizard window = CreateInstance<ComponentWizard>();
            
            Vector2 size = new Vector2(250, 150);
            Vector2 position = new Vector2((Screen.currentResolution.width - size.x) / 2, (Screen.currentResolution.height - size.y) / 2);
            window.position = new Rect(position, size);

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

        protected void OnLostFocus() => Close();

        protected void OnEnable()
        {
            (AllComponents, ComponentEntries) = GetType(typeof(IComponent));
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal("box");
            GUIStyle style = new()
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = 18,
                normal =
                {
                    textColor = Color.white
                },
                fontStyle = FontStyle.Bold
            };

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

            if (GUILayout.Button("Create") && !string.IsNullOrWhiteSpace(Name)) CreateComponent();

            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
        }

        void CreateComponent()
        {
            Type selected = AllComponents[selectedComponent];

            ScriptableObject scriptableObject = CreateInstance(selected);

            string filePath = Selection.assetGUIDs.Length == 0 ? "Assets" : AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            
            AssetDatabase.CreateAsset(scriptableObject, $"{filePath}/{Name}.asset");
            AssetDatabase.SaveAssets();

            Close();
        }
    }
}