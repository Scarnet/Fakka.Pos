using Fakka.Core.Interfaces;
using Fakka.Core.Resources;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fakka.Core.Enums;
using Fakka.Core.Managers;

namespace Fakka.Core.Utilities
{
    public class CameraHandler : ICameraHandler 
    {
        public async Task<MediaFile> MediaPickerAsync(IView view, int? width = 0, int? height = 0)
        {
            var selectedMedia = await view.DisplayActionSheet(CoreResources.SelectMedia, CoreResources.Cancel, null, CoreResources.Gallery, CoreResources.Camera);
            
            if (selectedMedia == CoreResources.Gallery)
            {
                return await PickPhotoAsync(width, height);
            }
            else if (selectedMedia == CoreResources.Camera)
            {
                return await TakePhotoAsync(width, height);
            }
            else
            {
                return null;
            }
        }


        public async Task<MediaFile> PickPhotoAsync(int? width = 0, int? height = 0)
        {
            GC.Collect();
            var photoSize = 25;
            var photosPermission = await PermissionHandler.CheckPermissions(Permission.Photos);
            var storagePermission = await PermissionHandler.CheckPermissions(Permission.Storage);
            if (ApplicationManager.Instance.GetTerminalInfo().TerminalCode == TerminalCode.Ios)
            {
                photoSize = 10;
            }

            if (!photosPermission||!storagePermission) return null;
            var photo = await CrossMedia.Current.PickPhotoAsync(
                new PickMediaOptions { PhotoSize = PhotoSize.Custom, CustomPhotoSize = photoSize,  CompressionQuality = 92, });
            return photo;
        }

       
        public async Task<MediaFile> TakePhotoAsync(int? width = 0, int? height = 0)
        {
            GC.Collect();
            var photoSize = 25;
            var cameraPermission = await PermissionHandler.CheckPermissions(Permission.Camera);
            var storagePermission = await PermissionHandler.CheckPermissions(Permission.Storage);

            if (ApplicationManager.Instance.GetTerminalInfo().TerminalCode == TerminalCode.Ios)
            {
                photoSize = 10;
            }

            if (!cameraPermission || !storagePermission) return null;

            var photo = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions { PhotoSize = PhotoSize.Custom, CustomPhotoSize = photoSize, CompressionQuality = 92 });

            return photo;
        }

    }
}
