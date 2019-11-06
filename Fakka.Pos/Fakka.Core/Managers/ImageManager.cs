using System;
using Fakka.Core.Models;

namespace Fakka.Core.Managers
{
    public class ImageManager
    {
        public static Uri GetImageUri(Image image)
        {
            return new Uri( ApplicationManager.Instance.GetServerInfo().BaseServerUrl + image.Url, UriKind.Absolute);
        }
        public static Uri GetImageUri(string imageUrl)
        {
            return new Uri(ApplicationManager.Instance.GetServerInfo().BaseServerUrl + imageUrl, UriKind.Absolute);
        }
    }
}


