using Fakka.Core.Models;
using System.Threading.Tasks;

namespace Fakka.Core.Interfaces
{
    public interface IStorageManager
    {
        Task<T> GetItemAsync<T>(string key, bool isMultiLang = true) where T : BaseOfflineModel, new();
        Task<T> GetItemAsync<T>(string key) where T : BaseOfflineModel, new();
        Task<bool> SetItemAsync<T>(T data, bool isMultiLang = true) where T : BaseOfflineModel, new();
        Task<bool> DeleteItemAsync<T>(string key, bool isMultiLang = true) where T : BaseOfflineModel;
        void DeleteAll<T>(bool isMultiLang = true) where T : BaseOfflineModel;
        Task<bool> DeleteAll(); 
    }

    public interface IStorage
    {
        Task<T> GetItemAsync<T>(string key) where T : BaseOfflineModel, new();
        Task<bool> SetItemAsync<T>(T data) where T : BaseOfflineModel, new();
        Task<bool> DeleteItemAsync<T>(string key) where T : BaseOfflineModel;
        void DeleteAll<T>() where T : BaseOfflineModel;
        Task<bool> DeleteAll();
    }
}