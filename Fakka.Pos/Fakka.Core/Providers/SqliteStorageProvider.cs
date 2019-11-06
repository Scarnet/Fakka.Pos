using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Providers
{
    public class SqliteStorageProvider : IStorage
    {
        private string dbName = "local.db3";
        private string databasePath;
        private SQLiteConnection con;
        public SqliteStorageProvider()
        {
            this.databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
            this.con = new SQLiteConnection(this.databasePath);
        }

        public void DeleteAll<T>() where T : BaseOfflineModel
        {
            try
            {
                con.DeleteAll<T>();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<bool> DeleteAll()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assembly in assemblies)
                {
                    var offlineModels = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(BaseOfflineModel)));
                    foreach (var model in offlineModels)
                    {
                        var map = new TableMapping(model);
                        con.DeleteAll(map);
                    }
                }

            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteItemAsync<T>(string key) where T : BaseOfflineModel
        {
            try
            {
                con.Delete<T>(key);
                return true;
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<T> GetItemAsync<T>(string key) where T : BaseOfflineModel, new()
        {
            try
            {
                string pk = key.ToString();
                var item = con.Get<T>(pk);
                return item;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default(T);
            }
        }


        public async Task<bool> SetItemAsync<T>(T data) where T : BaseOfflineModel, new()
        {
            try
            {
                con.CreateTable<T>();
                int rows = con.InsertOrReplace(data);
                return rows > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
    }
}
