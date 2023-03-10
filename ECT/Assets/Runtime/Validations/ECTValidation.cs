using System.Linq;

namespace ECT
{
    public static class ECTValidation
    {
        public static ECTBoolValidation ValidateMany(params IValidation[] validations) => new(validations.All(validation => validation.Successful == true));

        
        public static IValidation ValidateReferences (params UnityEngine.Object[] inputs)
        {
            return new ECTBoolValidation(inputs.All(reference => ValidateReference(reference).Successful));
        }
        
        public static ECTReferenceValidation<T> ValidateReference<T> (T input) where T : UnityEngine.Object
        {
            return ValidateReference<T>(input, out _);
        }

        public static ECTReferenceValidation<T> ValidateReference<T> (T input, out T output) where T : UnityEngine.Object
        {
            output = input;

            return new ECTReferenceValidation<T>(input);
        }
    }
}