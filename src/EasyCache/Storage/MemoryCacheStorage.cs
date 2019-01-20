using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class MemoryCacheStorage : ICacheStorage
{
    private readonly IDictionary<string, (object, DateTime)> _dictionary;

    public MemoryCacheStorage()
    {
        _dictionary = new ConcurrentDictionary<string, (object, DateTime)>();
    }

    public virtual T GetValue<T>(string key)
    {
        if (ContainsValidKey(key))
        {
            return (T)_dictionary[key].Item1;
        }

        return default(T);
    }

    public virtual void SetValue<T>(string key, T value, TimeSpan expiration)
    {
        var expDate = DateTime.Now.Add(expiration);

        if (_dictionary.ContainsKey(key))
        {
            _dictionary[key] = (value, expDate);
        }
        else 
        {
            _dictionary.Add(key, (value, expDate));
        }
    }

    public virtual bool ContainsValidKey(string key)
    {
        if (_dictionary.ContainsKey(key))
        {
            if (_dictionary[key].Item2 >= DateTime.Now)
            {
                return true;
            }
        }

        return false;
    }
}