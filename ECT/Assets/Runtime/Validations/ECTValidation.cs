using System.Linq;

namespace ECT
{
    public static class ECTValidation
    {
        public static ECTBoolValidation ValidateMany(params IValidation[] validations) => new(validations.All(validation => validation.Successful == true));

        public static ECTReferenceValidation<T> ValidateReference<T> (T input, out T output) where T : class
        {
            output = input;
            return new ECTReferenceValidation<T>(input);
        }
    }
}