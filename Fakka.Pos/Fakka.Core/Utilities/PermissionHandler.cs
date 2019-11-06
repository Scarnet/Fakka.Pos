using System.Threading.Tasks;
using Fakka.Core.Resources;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Fakka.Core.Utilities
{
    public static class PermissionHandler
    {
        public static async Task<bool> CheckPermissions(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {

                    var title = $"{permission} {CoreResources.Permission}";
                    var question =
                        $"{CoreResources.UsePlugin} {permission} {CoreResources.PermissionIsRequired} {CoreResources.GoToSettings} {permission} {CoreResources.ForTheApp}";
                    var positive = CoreResources.Settings;
                    var negative = CoreResources.AskLater;
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return false;
                }

                request = true;

            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (newStatus.ContainsKey(permission) && newStatus[permission] != PermissionStatus.Granted)
                {
                    var title = $"{permission} {CoreResources.Permission}";
                    var question = $"{CoreResources.UsePlugin} {permission} {CoreResources.PermissionIsRequired}";
                    var positive = CoreResources.Settings;
                    var negative = CoreResources.AskLater;
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return false;
                }
            }

            return true;
        }
    }
}