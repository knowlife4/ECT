using UnityEngine;

namespace ECT.Samples.Platformer
{
    public class PlayerEntity : ECTEntity<PlayerEntity>
    {
        public void Update() => UpdateSystems();
    }
}