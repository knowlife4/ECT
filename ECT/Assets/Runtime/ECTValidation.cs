namespace ECT
{
    public struct ECTValidation
    {
        public ECTValidation(bool successful) => Successful = successful;
        public static ECTValidation Validate<T>(T input, out T output) where T : class
        {
            output = input;
            return new(input == null);
        }

        public bool Successful { get; }
    }
}