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

        public void SetValue<T>(string key, T value)
        {
            _storage.SetValue(key, value);
        }

        public T GetValue<T>(string key, Func<T> cachelessFunc)
        {
            var value = GetValue<T>(key);

            if (value == null)
            {
                value = cachelessFunc();
                _storage.SetValue(key, value);
            }

            return value;
        }
    }
}
