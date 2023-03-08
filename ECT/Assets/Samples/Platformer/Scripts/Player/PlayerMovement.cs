using UnityEngine;

namespace ECT.Samples.Platformer
{
    [CreateAssetMenu]
    public class PlayerMovement : PlayerEntity.Component.Parent
    {
        [ComponentSystem]
        public class System : System<PlayerMovement>
        {
            
        }
    }
}