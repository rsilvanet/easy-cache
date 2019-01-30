using EasyCache.NET.Storage;
using EasyCache.NET.Tests.Shared;
using Moq;
using System.IO;
using System.Linq;

namespace EasyCache.NET.Tests
{
    public class FileCacheTests : BaseCacheTests
    {
        public FileCacheTests()
        {
            var tempPath = Path.GetTempPath();

            var tempFiles = Directory.EnumerateFiles(tempPath)
                .Select(x => new FileInfo(x))
                .Where(x => x.Name.StartsWith("easy-cache"))
                .Select(x => x.FullName);

            foreach (var file in tempFiles)
            {
                File.Delete(file);
            }

            var storage = new Mock<FileCacheStorage>(tempPath)
            {
                CallBase = true
            };

            _storage = storage;
            _caching = new Caching(storage.Object);
        }
    }
}
