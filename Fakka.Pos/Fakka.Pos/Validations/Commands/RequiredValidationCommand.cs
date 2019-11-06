using Fakka.Core.Validations;
using Fakka.Pos.Resources;
using Fakka.Pos.Validations.ValidationRules;
namespace Fakka.Pos.Validations.Commands
{
    public class RequiredValidationCommand<T> : BaseValidationsHandler<T>
    {
        public RequiredValidationCommand(ValidatableObject<T> validatableObject, string fieldName, string customMessage = null) : base(validatableObject)
        {
            validatableObject.Validations.Clear();
            if (customMessage == null)
            {
                validatableObject.Validations.Add(new IsNotNullOrEmptyRule<T> { ValidationMessage = string.Format(UiResources.IsRequired, fieldName) });
            }
            else
            {
                validatableObject.Validations.Add(new IsNotNullOrEmptyRule<T> { ValidationMessage = customMessage });
            }



        }

    }
}