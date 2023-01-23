using UnityEngine;

namespace ECT.Samples.Platformer
{
    public class PlayerEntity : ECTEntity<PlayerEntity>
    {
        public Transform Target;

        public void Update()
        {
            if(Vector3.Distance(transform.position, Target.position) > 10) return;
            UpdateChildren();
        }
    }
}