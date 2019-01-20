public interface ICacheStorage
{
    T GetValue<T>(string key);
    void SetValue<T>(string key, T value);
    bool ContainsKey(string key);
}