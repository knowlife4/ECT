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
                Root.transform.Rotate(new Vector3(0, Time.deltaTime * Component.Speed * 100, 0));
            }
        }
    }
}