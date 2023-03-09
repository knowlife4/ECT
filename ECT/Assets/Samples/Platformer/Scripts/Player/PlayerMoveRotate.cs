using Unity.Mathematics;
using UnityEngine;
using ECT.Parallel;
using Unity.Collections;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovementRotate : PlayerMovement.Component
    {
        public bool UseMultithreading;
        public bool UseBurst;
        public float Speed;

        [SceneReference]
        public struct SceneReference : ISceneReference
        {
            public Transform Target;

            public IValidation Validation => ECTValidation.ValidateReference(Target);
        }

        protected override ISystem CreateSystem() => UseMultithreading ? new ParallelSystem() : new System();
        
        public class System : System<PlayerMovementRotate>
        {
            SceneReference reference;
            
            protected override IValidation[] Validations => new[]
            {
                QueryReference(out reference)
            };

            protected override void OnUpdate()
            {
                Transform transform = Root.transform;
                float3 direction = reference.Target.position - transform.position;

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

            SceneReference reference;

            protected override IValidation[] Validations => new[]
            {
                QueryReference(out reference),
                ValidateReference(Root.transform, out transform),
            };

            protected override void PopulateData(ref Data data)
            {
                data.Transform = transform;
                data.Target = reference.Target;
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