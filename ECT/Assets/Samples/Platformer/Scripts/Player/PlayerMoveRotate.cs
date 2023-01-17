using UnityEngine;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovementRotate : PlayerMovement.ChildComponent
    {
        public float Speed;
        protected override ISystem System => new RotateSystem();

        public class RotateSystem : ComponentSystem<PlayerMovementRotate>
        {
            protected override void OnUpdate()
            {
                if(!QuerySystem(out PlayerMovementWalk.WalkSystem walkSystem).Successful) return;
                Root.transform.Rotate(new Vector3(0, Time.deltaTime * Component.Speed * 100, 0));

                walkSystem.GoingUp.Subscribe(RotateX);
            }

            public void RotateX ()
            {
                Root.transform.Rotate(new Vector3(Time.deltaTime * Component.Speed * 100, 0, 0));
            }
        }
    }
}