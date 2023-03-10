using UnityEditor;
using UnityEngine;

namespace ECT.UnityEditor
{
    public class ScriptTemplates
    {
        [MenuItem("Assets/ECT/C# Script/Entity", false, -10)]
        static void EntityTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile($"{GetScriptTemplateDir()}/ECTEntityTemplate.txt", "MyEntity.cs");
        }
        
        [MenuItem("Assets/ECT/C# Script/Component", false, -10)]
        static void ComponentTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile($"{GetScriptTemplateDir()}/ECTComponentTemplate.txt", "MyComponent.cs");
        }

        static string GetScriptTemplateDir()
        {
            bool inPackage = AssetDatabase.IsValidFolder("Packages/com.slice.ect");
            return inPackage ? "Packages/com.slice.ect/ScriptTemplates" : "Assets/ScriptTemplates";
        }
    }
}