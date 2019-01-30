using EasyCache.NET.Storage;
using EasyCache.NET.Tests.Shared;
using Moq;

namespace EasyCache.NET.Tests
{
    public class MemoryCacheTests : BaseCacheTests
    {
        public MemoryCacheTests()
        {
            var storage = new Mock<MemoryCacheStorage>()
            {
                CallBase = true
            };

            _storage = storage;
            _caching = new Caching(storage.Object);
        }
    }
}
