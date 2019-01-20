using System;

namespace EasyCache
{
    public class Caching
    {
        private readonly ICacheStorage _storage;

        public Caching(ICacheStorage storage)
        {
            _storage = storage;
        }

        public T GetValue<T>(string key)
        {
            return _storage.GetValue<T>(key);
        }

        public void SetValue<T>(string key, T value, TimeSpan expiration)
        {
            _storage.SetValue(key, value, expiration);
        }

        public T GetValue<T>(string key, Func<T> cachelessFunc, TimeSpan expiration)
        {
            var value = GetValue<T>(key);

            if (value == null)
            {
                value = cachelessFunc();
                _storage.SetValue(key, value, expiration);
            }

            return value;
        }

        public bool ContainsKey(string key)
        {
            return _storage.ContainsValidKey(key);
        }
    }
}
