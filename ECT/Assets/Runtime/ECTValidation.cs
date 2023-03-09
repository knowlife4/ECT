using System.Linq;

namespace ECT
{
    public struct ECTValidation
    {
        public ECTValidation(bool successful) => Successful = successful;

        public static ECTValidation ValidateReference<T>(T input, out T output)
        {
            output = input;
            return new ECTValidation(input != null);
        }
        
        public static ECTValidation Validate(params ECTValidation[] validations) => new(validations.All(validation => validation.Successful == true));

        public bool Successful { get; }
    }
}