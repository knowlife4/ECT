namespace ECT.Samples.Platformer
{
    public class PlayerEntity : ECTEntity<PlayerEntity>
    {
        public void Update() => ReferenceBranch.Update(this, this);
    }
}