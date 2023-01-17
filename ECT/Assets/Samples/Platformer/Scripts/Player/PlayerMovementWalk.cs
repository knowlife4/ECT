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
            public ECTAction GoingUp = new();

            protected override void OnUpdate()
            {
                float y = Mathf.Sin(Time.time * Component.Speed) * Component.Height;

                Root.transform.position = new(Root.transform.position.x, y, Root.transform.position.z);

                if(y > 0) GoingUp.Execute();
            }
        }
    }
}