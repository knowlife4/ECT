namespace ECT.Parallel
{
    public interface IParallelData<MyData> where MyData : unmanaged, IParallelData<MyData>
    {
        public MyData Execute();
    }
}