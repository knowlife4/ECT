using System.Collections.Generic;
using UnityEngine;

namespace ECT
{
    [System.Serializable]
    public class ECTSceneReferenceGroup
    {
        [SerializeReference]
        public ISceneReference[] References;

        Dictionary<int, ISceneReference> referenceCache = new();

        public T? Get<T>() where T : struct, ISceneReference
        {
            int refId = typeof(T).GetHashCode();

            if (referenceCache.TryGetValue(refId, out ISceneReference cachedRef))
            {
                if (cachedRef != null) return (T)cachedRef;

                referenceCache.Remove(refId);
            }

            foreach (ISceneReference reference in References)
            {
                if (reference is not T sceneReference) continue;
                
                referenceCache.Add(sceneReference.GetType().GetHashCode(), sceneReference);
                return sceneReference;
            }

            return null;
        }
    }
}