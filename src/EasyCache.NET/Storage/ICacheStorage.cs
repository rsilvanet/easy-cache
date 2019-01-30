using System;

namespace EasyCache.NET.Storage
{
    public interface ICacheStorage
    {
        T GetValue<T>(string key);
        void SetValue<T>(string key, T value, TimeSpan expiration);
        bool ContainsValidKey(string key);
        void RemoveKey(string key);
        void Reset();
        int Count();
    }
}