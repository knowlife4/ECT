using UnityEngine;

namespace ECT.Samples.Platformer
{
    public class PlayerEntity : ECTEntity<PlayerEntity>
    {
        public Transform Target;

        public void Update() => UpdateChildren();
    }
}