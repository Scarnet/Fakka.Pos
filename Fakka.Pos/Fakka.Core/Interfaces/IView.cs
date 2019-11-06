using System.Threading.Tasks;

namespace Fakka.Core.Interfaces
{
    public interface IView
    {
        Task Alert(string title, string details);
        Task Alert(string title, string detail, string buttonText);
        Task<bool> Alert(string title, string detail, string buttonText, string cancelText);
        Task<string> DisplayActionSheet(string title, string okTitle, string cancelTitle, params string[] otherButtons);

    }
}