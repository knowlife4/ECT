namespace ECT
{
    public interface IReferenceSystem
    {
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem;
    }
}