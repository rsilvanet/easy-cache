using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace EasyCache.Tests
{
    public class MemoryCacheTests
    {
        private readonly Mock<MemoryCacheStorage> _storage;
        private readonly Caching _caching;

        public MemoryCacheTests()
        {
            _storage = new Mock<MemoryCacheStorage>()
            {
                CallBase = true
            };

            _caching = new Caching(_storage.Object);
        }
        
        [Fact]
        public void CanGenerateCacheFromSet()
        {
            Assert.False(_caching.ContainsKey("test"));
            
            _caching.SetValue("test", "Some value.");
            _storage.Verify(x => x.SetValue("test", "Some value."), Times.Once);
            
            Assert.True(_caching.ContainsKey("test"));
        }

        [Fact]
        public void CanGenerateCacheFromGetWithCachelessFunc()
        {
            Assert.False(_caching.ContainsKey("test"));
            
            _caching.GetValue("test", () => "Some value.");
            _storage.Verify(x => x.SetValue("test", "Some value."), Times.Once);
            
            Assert.True(_caching.ContainsKey("test"));
        }

        [Fact]
        public void CanGetValueFromCache()
        {
            Assert.Null(_caching.GetValue<string>("test"));

            _caching.SetValue("test", "Some value.");

            Assert.Equal("Some value.", _caching.GetValue<string>("test"));
        }
    }
}
