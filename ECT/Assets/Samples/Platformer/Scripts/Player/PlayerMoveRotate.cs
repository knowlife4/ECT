using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using ECT.Parallel;
using Unity.Jobs;
using Unity.Collections;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovementRotate : PlayerMovement.ChildComponent
    {
        public float Speed;
        protected override ISystem System => new RotateSystemParallel();

        public class RotateSystem : ComponentSystem<PlayerMovementRotate>
        {
            protected override void OnUpdate()
            {
                float3 direction = Root.Target.position - Root.transform.position;

                float rotationSpeed = Component.Speed * Time.deltaTime;
                float3 up = new(0f, 1f, 0f);

                Root.transform.rotation = math.slerp(Root.transform.rotation, quaternion.LookRotationSafe(direction, up), rotationSpeed);
            }
        }

        public class RotateSystemParallel : ComponentParallelSystem<PlayerMovementRotate, ParallelData>
        {
            public override void UpdateData(ref ParallelData data)
            {
                data.Transform = Root.transform;
                data.Target = Root.Target;
                data.Speed = Component.Speed;
                data.DeltaTime = Time.deltaTime;
            }

            public override void OnComplete(ParallelData data) => Root.transform.rotation = data.Transform.rotation;

            public override void Schedule(NativeArray<ParallelData> dataArray) => API.ParallelJobExecute(dataArray).Run();
        }

        public struct ParallelData : IParallelData<ParallelData>
        {
            public ECTParallelTransform Transform;
            public ECTParallelTransform Target;

            public float Speed;
            public float DeltaTime;

            public ParallelData Execute()
            {
                float3 direction = Target.position - Transform.position;

                float rotationSpeed = Speed * DeltaTime;
                float3 up = new(0f, 1f, 0f);

                Transform.rotation = math.slerp(Transform.rotation, quaternion.LookRotationSafe(direction, up), rotationSpeed);

                return this;
            }
        }
    }
}