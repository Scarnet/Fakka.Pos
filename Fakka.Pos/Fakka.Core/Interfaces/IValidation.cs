namespace Fakka.Core.Interfaces
{
    public interface IValidity
    {
        bool IsValid { get; set; }
    }
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }

        bool Check(T value);
    }
}