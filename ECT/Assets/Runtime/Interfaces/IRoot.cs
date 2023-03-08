namespace ECT
{
    public interface IRoot : IParent, IReferenceSystem
    {
        public ECTSceneReferenceGroup ReferenceGroup { get; }
        public ECTSystemDataGroup DataGroup { get; }

        public ECTValidation QuerySystem<TSystem>(out TSystem found) where TSystem : class, ISystem;

        public ECTValidation QueryReference<TReference>(out TReference found) where TReference : struct, ISceneReference;
    }
}