using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using ECT.Parallel;
using Unity.Jobs;
using Unity.Collections;
using NotImplementedException = System.NotImplementedException;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovementRotate : PlayerMovement.Component
    {
        public bool UseMultithreading;
        public bool UseBurst;
        public float Speed;

        protected override ISystem CreateSystem() => UseMultithreading ? new ParallelSystem() : new System();
        
        public class System : System<PlayerMovementRotate>
        {
            protected override void OnUpdate()
            {
                Transform transform = Root.transform;
                float3 direction = Root.Target.position - transform.position;

                float rotationSpeed = Component.Speed * Time.deltaTime;
                float3 up = new(0f, 1f, 0f);

                transform.rotation = math.slerp(transform.rotation, quaternion.LookRotationSafe(direction, up), rotationSpeed);

                for (int i = 0; i < 10000; i++)
                {
                    math.sqrt(math.log10(i));
                }
            }
        }

        public class ParallelSystem : System<PlayerMovementRotate>.Parallel<Data>
        {
            Transform transform;
            Transform target;

            protected override ECTValidation[] Validations => new[]
            {
                Validate(Root.transform, out transform),
                Validate(Root.Target, out target)
            };

            protected override void PopulateData(ref Data data)
            {
                data.Transform = transform;
                data.Target = target;
                data.Speed = Component.Speed;
                data.DeltaTime = Time.deltaTime;
            }

            protected override void ExtractData(Data data) => transform.rotation = data.Transform.rotation;

            protected override void Schedule(NativeArray<Data> dataArray) => ParallelConfigAPI.Create(dataArray).Run(Component.UseBurst);
        }

        public struct Data : IParallelData<Data>
        {
            public ECTParallelTransform Transform;
            public ECTParallelTransform Target;

            public float Speed;
            public float DeltaTime;

            public Data Execute(NativeArray<Data> dataArray)
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