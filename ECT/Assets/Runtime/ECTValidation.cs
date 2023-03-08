namespace ECT
{
    public struct ECTValidation
    {
        public ECTValidation(bool successful) => Successful = successful;

        public static ECTValidation Validate<T>(T input, out T output)
        {
            output = input;
            return new ECTValidation(input != null);
        }

        public bool Successful { get; }
    }
}