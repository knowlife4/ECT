using UnityEngine;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovement : PlayerEntity.EntityComponent.ComponentGroup
    {
        protected override ISystem System => new MovementSystem();

        public class MovementSystem : ComponentSystem<PlayerMovement> {}
    }
}