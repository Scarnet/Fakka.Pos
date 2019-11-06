using System.Collections.Generic;
using System.Linq;
using Fakka.Core.Interfaces;

namespace Fakka.Core.Validations
{
    public class ValidatableObject<T> : ExtendedBindableObject, IValidity
    {
        private List<string> _errors;
        private T _value;
        private bool _isValid;

        public List<IValidationRule<T>> Validations { get; }

        public List<string> Errors
		{
			get => _errors;
		    set
			{
				_errors = value;
				RaisePropertyChanged(() => Errors);
			}
		}

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public ValidatableObject()
        {
            _isValid = true;
            _errors = new List<string>();
            Validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))                             
                .Select(v => v.ValidationMessage);

			Errors = errors.ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
    }
}
