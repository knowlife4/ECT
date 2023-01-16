using UnityEngine;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovementWalk : PlayerMovement.ChildComponent
    {
        
        public float Speed;
        public float Height;

        protected override ISystem System => new WalkSystem();

        public class WalkSystem : ComponentSystem<PlayerMovementWalk>
        {
            protected override void OnUpdate()
            {
                Root.transform.position = new(Root.transform.position.x, Mathf.Sin(Time.time * Component.Speed) * Component.Height, Root.transform.position.z);
            }
        }
    }
}