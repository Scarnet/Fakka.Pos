using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Interfaces
{
    public interface ICameraHandler
    {
        Task<MediaFile> TakePhotoAsync(int? width = default(int), int? height = default(int));
        Task<MediaFile> PickPhotoAsync(int? width = default(int), int? height = default(int));
        Task<MediaFile> MediaPickerAsync(IView view, int? width = default(int), int? height = default(int));
    }
}
