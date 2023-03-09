using System;
using UnityEngine;

namespace ECT
{
    public static class ECTExceptions
    {
        // Thrown if Component doesn't have a nested system with attribute [ComponentSystem]
        public static Exception MissingComponentSystem(Type type)
        {
            return new($"{type} does not have a specified Component System! Did you forget to use the [ComponentSystem] Attribute?");
        }

        public static void ThrowAndContinue(Exception exception) => Debug.LogError(exception);
    }
}