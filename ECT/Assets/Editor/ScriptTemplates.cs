using UnityEditor;
using UnityEngine;

namespace ECT.UnityEditor
{
    public class ScriptTemplates
    {
        [MenuItem("Assets/ECT/C# Script/Entity", false, -10)]
        static void EntityTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Assets/ScriptTemplates/ECTEntityTemplate.txt", "MyEntity.cs");
        }
        
        [MenuItem("Assets/ECT/C# Script/Component", false, -10)]
        static void ComponentTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Assets/ScriptTemplates/ECTComponentTemplate.txt", "MyComponent.cs");
        }
    }
}