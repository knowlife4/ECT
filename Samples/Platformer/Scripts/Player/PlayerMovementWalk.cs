using UnityEngine;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovementWalk : PlayerMovement.Component
    {
        [SerializeField] float speed;
        [SerializeField] float height;

        [ComponentSystem]
        public class System : System<PlayerMovementWalk>
        {
            protected override void OnUpdate()
            {
                float y = Mathf.Sin(Time.time * Component.speed) * Component.height;

                Transform transform = Root.transform;
                Vector3 position = transform.position;
                position = new(position.x, y, position.z);
                transform.position = position;
            }
        }
    }
}