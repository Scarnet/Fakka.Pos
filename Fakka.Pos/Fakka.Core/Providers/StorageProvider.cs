using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Fakka.Core.Interfaces;
using Fakka.Core.Managers;


namespace Fakka.Core.Providers
{
    /* public class StorageProvider : IStorage
     {
         //private SQLitePersistentBlobCache _cache;
         /// <summary>
         /// Creates new StorageProvider object to store data in UserAccount
         /// Make sure you set the application name before doing any insert or get operations
         /// </summary>
         public StorageProvider()
         {
             BlobCache.ApplicationName = ApplicationManager.Instance.GetApplicationInfo().Code;
             BlobCache.EnsureInitialized();

             // _cache = new SQLitePersistentBlobCache("test.db");
         }


         public async Task<T> GetItemAsync<T>(string key)
         {
             try
             {
                 //return await _cache.GetObject<T>(key);
                 return await BlobCache.UserAccount.GetObject<T>(key);

             }
             catch (KeyNotFoundException)
             {
                 return await Task.FromResult(default(T));
             }
         }


         public async Task SetItemAsync(string key, object data)
         {
             //await _cache.InsertObject(key, data);

             await BlobCache.UserAccount.InsertObject(key, data);
         }

         public async Task<bool> DeleteItemAsync(string key)
         {
             //await _cache.Invalidate(key);
             try
             {
                 await BlobCache.UserAccount.Invalidate(key);
                 return true;
             }
             catch (Exception)
             {
                 return false;
             }
         }

         public async void DeleteAll(string key)
         {
             //await _cache.InvalidateAll();
             await BlobCache.UserAccount.InvalidateAll();
         }
     }*/
    public class StorageProvider : IStorage
    {
        private readonly Dictionary<string, object> _storage = new Dictionary<string, object>();

        public void DeleteAll()
        {
            _storage.Clear();
        }

        public void DeleteAll<T>()
        {
            throw new NotImplementedException("Generic methods are only implemented in SQLlite providers");
        }

        public async Task<bool> DeleteItemAsync(string key)
        {
            try
            {
                _storage.Remove(key);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return false;
            }

        }

        public Task<bool> DeleteItemAsync(object key)
        {
            throw new NotImplementedException("Generic methods are only implemented in SQLlite providers");
        }

        public Task<bool> DeleteItemAsync<T>(string key)
        {
            throw new NotImplementedException("Generic methods are only implemented in SQLlite providers");
        }

        public async Task<T> GetItemAsync<T>(string key) where T : new()
        {
            try
            {
                _storage.TryGetValue(key, out var data);

                return await Task.FromResult((T)data);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public async Task SetItemAsync(string key, object data)
        {
            try
            {
                if (_storage.ContainsKey(key))
                {
                    _storage[key] = data;
                }
                else
                {
                    await Task.Run(() => { _storage.Add(key, data); });
                }
            }
            catch (Exception)
            {
                // return Task.FromResult(0);
            }

        }

        public Task<bool> SetItemAsync<T>(T data)
        {
            throw new NotImplementedException("Generic methods are only implemented in SQLlite providers");
        }
    }
}