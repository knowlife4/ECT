using UnityEngine;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovementWalk : PlayerMovement.Component
    {
        public float Speed;
        public float Height;

        [ComponentSystem]
        public class System : System<PlayerMovementWalk>
        {
            protected override void OnUpdate()
            {
                float y = Mathf.Sin(Time.time * Component.Speed) * Component.Height;

                Transform transform = Root.transform;
                Vector3 position = transform.position;
                position = new(position.x, y, position.z);
                transform.position = position;
            }
        }
    }
}