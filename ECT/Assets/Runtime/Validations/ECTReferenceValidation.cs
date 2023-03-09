namespace ECT
{
    public struct ECTReferenceValidation<T> : IValidation where T : class
    {
        public ECTReferenceValidation(T reference) => Reference = reference;
        T Reference { get; }

        public bool Successful => Reference != null;
    }
}