using EasyCache.Storage;
using EasyCache.Tests.Shared;
using Moq;

namespace EasyCache.Tests
{
    public class MemoryCacheTests : BaseCacheTests
    {
        public MemoryCacheTests()
        {
            var storage = new Mock<MemoryCacheStorage>()
            {
                CallBase = true
            };

            _caching = new Caching(storage.Object);
        }
    }
}
