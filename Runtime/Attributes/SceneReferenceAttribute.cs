using System;
using System.Linq;

namespace ECT
{
    public class SceneReferenceAttribute : Attribute
    {
        public static Type Find(Type componentType)
        {
            Type[] nested = componentType.GetNestedTypes();

            Type firstOrDefault = nested.FirstOrDefault(type => type.IsDefined(typeof(SceneReferenceAttribute), false));
            
            return firstOrDefault;
        }
    }
}