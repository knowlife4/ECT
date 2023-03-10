using System.Linq;

namespace ECT
{
    public struct ECTBoolValidation : IValidation
    {
        public ECTBoolValidation(bool successful) => Successful = successful;

        public bool Successful { get; }
    }
}