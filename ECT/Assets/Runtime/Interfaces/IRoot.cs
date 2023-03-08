namespace ECT
{
    public interface IRoot : IParent, IReferenceSystem
    {
        public ECTSystemDataGroup DataGroup { get; }

        public ECTValidation QuerySystem<TSystem>(out TSystem find) where TSystem : class, ISystem;
    }
}