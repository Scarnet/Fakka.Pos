using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Fakka.Core.Managers;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using System.Diagnostics;
using System.Drawing;
using Image = System.Drawing.Image;
using System.Threading;

namespace Fakka.Core.Utilities
{
    public static class ImageHandler
    {

        public static ImageSource Base64ToImageSource(string base64Image, string defaultImagePlaceholder)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                return defaultImagePlaceholder;
            }
            else
            {
                return ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(base64Image)));
            }
        }

        public static async Task<string> MediaFileToBase64(MediaFile mediaFile)
        {
            var stream = mediaFile.GetStream();
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            return Convert.ToBase64String(bytes);
        }


        public static string GetImageUri(string imageId, string imageContext)
        {
            var url = $"{ApplicationManager.Instance.GetServerInfo().BaseImageUrl}/{imageContext}/{imageId}.png{ApplicationManager.Instance.GetServerInfo().FileAccessToken}{DateTime.Now.Ticks}";
            return url;
        }



        public static async Task<string> ImageUrlToBase64(string url)
        {
            Uri uri;
            bool validUri = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri);

            if (!validUri)
                return string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync(url);
                    return Convert.ToBase64String(bytes);
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);
                return string.Empty;
            }
        }

        public static async Task<byte[]> ImageToBytesArray(string resourceId, System.Reflection.Assembly assembly)
        {
            try
            {
                var stream = assembly.GetManifestResourceStream(resourceId);
                byte[] imageBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }
                return imageBytes;

            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }

    }
}
