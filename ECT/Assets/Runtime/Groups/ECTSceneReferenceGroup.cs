using UnityEngine;

namespace ECT
{
    [System.Serializable]
    public class ECTSceneReferenceGroup
    {
        [SerializeReference] public ISceneReference[] References;

        public T? Get<T>() where T : struct, ISceneReference
        {
            foreach (ISceneReference reference in References)
            {
                if (reference is T sceneReference) return sceneReference;
            }

            return null;
        }
    }
}