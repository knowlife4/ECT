namespace ECT
{
    public interface IRoot : IParent, IReferenceParent
    {
        public IReferenceBranch Children { get; }
    }
}