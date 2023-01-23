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
        public bool UseMultithreading;
        public bool UseBurst;
        public float Speed;

        protected override ISystem System => UseMultithreading ? new RotateSystemParallel() : new RotateSystem();

        public class RotateSystem : ComponentSystem<PlayerMovementRotate>
        {
            protected override void OnUpdate()
            {
                float3 direction = Root.Target.position - Root.transform.position;

                float rotationSpeed = Component.Speed * Time.deltaTime;
                float3 up = new(0f, 1f, 0f);

                Root.transform.rotation = math.slerp(Root.transform.rotation, quaternion.LookRotationSafe(direction, up), rotationSpeed);

                for (int i = 0; i < 10000; i++)
                {
                    math.sqrt(math.log10(i));
                }
            }
        }

        public class RotateSystemParallel : ComponentParallelSystem<PlayerMovementRotate, ParallelData>
        {
            Transform transform;
            Transform target;

            protected override ECTValidation[] Validations => new[]
            {
                Validate(Root.transform, out transform),
                Validate(Root.Target, out target)
            };

            public override void UpdateData(ref ParallelData data)
            {
                data.Transform = transform;
                data.Target = target;
                data.Speed = Component.Speed;
                data.DeltaTime = Time.deltaTime;
            }

            public override void OnComplete(ParallelData data) => transform.rotation = data.Transform.rotation;

            public override void Schedule(NativeArray<ParallelData> dataArray) => API.ParallelJobExecute(dataArray).Run(Component.UseBurst);
        }

        public struct ParallelData : IParallelData<ParallelData>
        {
            public ECTParallelTransform Transform;
            public ECTParallelTransform Target;

            public float Speed;
            public float DeltaTime;

            public ParallelData Execute(NativeArray<ParallelData> DataArray)
            {
                float3 direction = Target.position - Transform.position;

                float rotationSpeed = Speed * DeltaTime;
                float3 up = new(0f, 1f, 0f);

                Transform.rotation = math.slerp(Transform.rotation, quaternion.LookRotationSafe(direction, up), rotationSpeed);

                for (int i = 0; i < 10000; i++)
                {
                    math.sqrt(math.log10(i));
                }

                return this;
            }
        }
    }
}