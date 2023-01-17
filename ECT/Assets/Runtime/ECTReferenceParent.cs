namespace ECT
{
    public interface IReferenceParent
    {
        public ECTValidation QuerySystem<FindSystem> (out FindSystem find) where FindSystem : class, ISystem;
    }
}