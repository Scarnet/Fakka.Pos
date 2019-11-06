using System.Threading.Tasks;

namespace Fakka.Core.Interfaces
{
    public interface IPopupDialog
    {
        Task ShowSuccess(string title, string details);
        Task ShowWarning(string title, string details);
        Task ShowError(string title, string details);
        Task Dismiss( );
        Task ShowCustom(Rg.Plugins.Popup.Pages.PopupPage customDialogPage);
    }

    public interface IPopupDialogPage
    {
        void SetDialogInfo(string title, string details);
    }
}