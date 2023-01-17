namespace ECT
{
    public struct ECTValidation
    {
        public ECTValidation(bool successful) => Successful = successful;
        public ECTValidation(object objectToValidate) => Successful = objectToValidate != null;

        public bool Successful { get; }
    }
}