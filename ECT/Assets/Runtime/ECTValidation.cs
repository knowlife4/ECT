namespace ECT
{
    public struct ECTValidation
    {
        public ECTValidation(bool successful)
        {
            Successful = successful;
        }

        public bool Successful { get; }
    }
}