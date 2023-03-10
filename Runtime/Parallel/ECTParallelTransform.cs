using Unity.Mathematics;
using UnityEngine;

namespace ECT.Parallel
{
    public struct ECTParallelTransform
    {
        public static implicit operator ECTParallelTransform(Transform transform) => new()
        {
            position = transform.position,
            rotation = transform.rotation,
            localScale = transform.localScale
        };

        public void Apply (ref Transform transform)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = localScale;
        }

        public float3 position;
        public quaternion rotation;
        public float3 localScale;
    }
}