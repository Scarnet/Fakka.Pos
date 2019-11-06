using Prism.Commands;

namespace Fakka.Core.Validations
{
    public abstract class BaseValidationsHandler<T> : DelegateCommand
    {
        public BaseValidationsHandler(ValidatableObject<T> validatableObject) : base(() =>
        {
            validatableObject.Validate();
        })
        {
            

        }
    }
}
