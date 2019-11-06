
using Fakka.Core.Constants;
using Fakka.Core.Interfaces;
using Fakka.Core.Models.Cache;
using Prism.Ioc;
using System;
using System.Threading.Tasks;

namespace Fakka.Core.Services
{
    public abstract class BaseService
    {
        protected readonly IDataService DataService;
        //protected readonly IStorageManager StorageManager;
        //protected readonly IDataContext DataContext;
        protected readonly ISessionManager SessionManager;
        private IDataService sqliteDataService;
        private IStorageManager sqliteStorageManager;

        public BaseService(IContainerProvider container, string baseServiceName)
        {
            //StorageManager = container.Resolve<IStorageManager>();
            ////DataContext = container.Resolve<IDataContext>();
            SessionManager = container.Resolve<ISessionManager>();
            DataService = container.Resolve<IDataService>();
            this.sqliteDataService = container.Resolve<IDataService>();
            this.sqliteStorageManager = container.Resolve<IStorageManager>();

            BaseServiceName = baseServiceName;
        }

        /// <summary>
        /// A generic method to download media bytes to local storage to improve performance
        /// </summary>
        /// <param name="url">Download URL</param>
        /// <param name="mediaId">The id you wish to use for the local  storage, make sure it's contextually unique</param>
        /// <returns></returns>
        protected async virtual Task<byte[]> CacheMedia(string url, string mediaId)
        {
            var image = await this.sqliteStorageManager.GetItemAsync<CachedData>(mediaId);

            if (image == null || image.ExpirationDate <= DateTime.Now)
            {
                byte[] data = (await this.sqliteDataService.Download(url)).Data;
                var cachedMedia = new CachedData
                {
                    Data = data,
                    ExpirationDate = DateTime.Now.AddDays(7),
                    OfflineId = mediaId
                };
                await this.sqliteStorageManager.SetItemAsync(cachedMedia);
                return data;
            }

            return image.Data;
        }
        protected string BaseServiceName { set; get; }
    }

}
