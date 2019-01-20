using System.Collections.Concurrent;
using System.Collections.Generic;

public class MemoryCacheStorage : ICacheStorage
{
    private readonly IDictionary<string, object> _dictionary;

    public MemoryCacheStorage()
    {
        _dictionary = new ConcurrentDictionary<string, object>();
    }

    public virtual T GetValue<T>(string key)
    {
        if (_dictionary.ContainsKey(key))
        {
            return (T)_dictionary[key];
        }

        return default(T);
    }

    public virtual void SetValue<T>(string key, T value)
    {
        if (_dictionary.ContainsKey(key))
        {
            _dictionary[key] = value;
        }
        else 
        {
            _dictionary.Add(key, value);
        }
    }

    public virtual bool ContainsKey(string key)
    {
        return _dictionary.ContainsKey(key);
    }
}