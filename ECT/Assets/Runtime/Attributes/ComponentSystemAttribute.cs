using System;
using System.Linq;

namespace ECT
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentSystemAttribute : Attribute
    {
        public static Type FindComponentSystemType(Type componentType)
        {
            Type[] nested = componentType.GetNestedTypes();

            return nested.FirstOrDefault(type => type.IsDefined(typeof(ComponentSystemAttribute), false));
        }
    }
}