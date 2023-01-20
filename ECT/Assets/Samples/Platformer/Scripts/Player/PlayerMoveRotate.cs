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

        public class RotateSystemParallel : ComponentParallelSystem<PlayerMovementRotate, JobData>
        {
            public override JobData CreateData() => new()
            {
                Transform = Root.transform,
                Target = Root.Target,
                Speed = Component.Speed,
                DeltaTime = Time.deltaTime
            };

            ECTParallelJob<JobData> job;

            public override IJobParallelFor CreateJob(NativeArray<JobData> dataArray)
            {
                job = new(dataArray);
                return job;
            }

            public override void OnComplete(JobData data) => Root.transform.rotation = data.Transform.rotation;
        }

        public struct JobData : IParallelData
        {
            public ECTParallelTransform Transform;
            public ECTParallelTransform Target;

            public float Speed;
            public float DeltaTime;
            
            [BurstCompile]
            public void Execute ()
            {
                float3 direction = Target.position - Transform.position;

                float rotationSpeed = Speed * DeltaTime;
                float3 up = new(0f, 1f, 0f);

                Transform.rotation = math.slerp(Transform.rotation, quaternion.LookRotationSafe(direction, up), rotationSpeed);
            }
        }
    }
}