using System.Collections.Generic;
using Fakka.Core.Utilities;

namespace Fakka.Core.Managers
{
    public class StateManager : Singleton<StateManager>
    {
        private Dictionary<string, object> _stateProvider = new Dictionary<string, object>();

        public object GetItem(string key)
        {
            _stateProvider.TryGetValue(key, out object data);
            return data;
        }

        public void SetItem(string key, object data)
        {
            if (_stateProvider.ContainsKey(key))
            {
                _stateProvider.Remove(key);
                _stateProvider.Add(key, data);

            }
            else
            {
                _stateProvider.Add(key, data);
            }
        }

        public void DeleteItem(string key)
        {
            _stateProvider.Remove(key);
        }

        public void DeleteAll()
        {
            _stateProvider.Clear();
        }
    }
}