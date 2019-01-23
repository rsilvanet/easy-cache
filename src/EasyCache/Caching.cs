using System;
using EasyCache.Storage;

namespace EasyCache
{
    public class Caching
    {
        private readonly ICacheStorage _storage;

        public Caching(ICacheStorage storage)
        {
            _storage = storage;
        }

        public void SetValue<T>(string key, T value, TimeSpan expiration)
        {
            _storage.SetValue(key, value, expiration);
        }

        public T GetValue<T>(string key)
        {
            return _storage.GetValue<T>(key);
        }

        public T GetValue<T>(string key, Func<T> cachelessFunc, TimeSpan expiration)
        {
            if (ContainsKey(key))
            {
                return GetValue<T>(key);;
            }
            
            var value = cachelessFunc();
            
            _storage.SetValue(key, value, expiration);

            return value;
        }

        public bool ContainsKey(string key)
        {
            return _storage.ContainsValidKey(key);
        }

        public void RemoveKey(string key)
        {
            _storage.RemoveKey(key);
        }

        public void Reset()
        {
            _storage.Reset();
        }

        public int Count()
        {
            return _storage.Count();
        }
    }
}
