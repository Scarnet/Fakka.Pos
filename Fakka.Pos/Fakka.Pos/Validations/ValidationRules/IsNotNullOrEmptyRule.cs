using Fakka.Core.Interfaces;

namespace Fakka.Pos.Validations.ValidationRules
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
