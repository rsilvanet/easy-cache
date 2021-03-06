using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EasyCache.NET.Storage
{
    public class FileCacheStorage : ICacheStorage
    {
        private readonly string _path;
        
        private const string PREFIX = "easy-cache-";

        public string BasePath => _path;

        public FileCacheStorage(string path)
        {
            _path = path;
        }

        private string BuildFilePath(string fileName)
        {
            return Path.Combine(_path, PREFIX + fileName);
        }

        public virtual T GetValue<T>(string key)
        {
            if (ContainsValidKey(key))
            {
                var filePath = BuildFilePath(key);
                var strValue = File.ReadAllLines(filePath).Last();
                var bytesValue = Encoding.Default.GetBytes(strValue);
                var serializer = new DataContractJsonSerializer(typeof(T));

                return (T)serializer.ReadObject(new MemoryStream(bytesValue));
            }

            return default(T);
        }

        public virtual void SetValue<T>(string key, T value, TimeSpan expiration)
        {
            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(stream, value);

            var filePath = BuildFilePath(key);
            var expireDate = DateTime.Now.Add(expiration);

            File.WriteAllLines(filePath, new string[]
            {
                expireDate.ToString(),
                Encoding.Default.GetString(stream.ToArray())
            });
        }

        public virtual bool ContainsValidKey(string key)
        {
            var filePath = BuildFilePath(key);

            if (File.Exists(filePath))
            {
                var expiration = DateTime.Parse(File.ReadLines(filePath).First());

                if (expiration >= DateTime.Now)
                {
                    return true;
                }
            }
            
            return false;
        }

        public void RemoveKey(string key)
        {
            var filePath = BuildFilePath(key);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void Reset()
        {
            var files = Directory.EnumerateFiles(_path)
                .Select(f => new FileInfo(f))
                .Where(f => f.Name.StartsWith(PREFIX));

            foreach (var file in files)
            {
                file.Delete();
            } 
        }

        public int Count()
        {
            return Directory.EnumerateFiles(_path)
                .Select(f => new FileInfo(f))
                .Where(f => f.Name.StartsWith(PREFIX))
                .Count();
        }
    }
}