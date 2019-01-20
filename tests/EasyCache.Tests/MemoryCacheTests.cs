using System;
using System.Collections.Generic;
using System.Threading;
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
            
            _caching.SetValue(
                "test", 
                "Some value.",
                TimeSpan.FromDays(1)
            );
            
            _storage.Verify(x => x.SetValue(
                "test", 
                "Some value.", 
                TimeSpan.FromDays(1)
            ), Times.Once);
            
            Assert.True(_caching.ContainsKey("test"));
        }

        [Fact]
        public void CanGenerateCacheFromGetWithCachelessFunc()
        {
            Assert.False(_caching.ContainsKey("test"));
            
            _caching.GetValue(
                "test", 
                () => "Some value.",
                TimeSpan.FromDays(1)
            );

            _storage.Verify(x => x.SetValue(
                "test", 
                "Some value.",
                TimeSpan.FromDays(1)
            ), Times.Once);
            
            Assert.True(_caching.ContainsKey("test"));
        }

        [Fact]
        public void CanGetValueFromCache()
        {
            Assert.Null(_caching.GetValue<string>("test"));

            _caching.SetValue(
                "test", 
                "Some value.", 
                TimeSpan.FromDays(1)
            );

            Assert.Equal("Some value.", _caching.GetValue<string>("test"));
        }

        [Fact]
        public void CacheExpiresCorrectly()
        {
            Assert.Null(_caching.GetValue<string>("test"));

            _caching.SetValue(
                "test", 
                "Some value.", 
                TimeSpan.FromSeconds(2)
            );

            Assert.Equal("Some value.", _caching.GetValue<string>("test"));
            Thread.Sleep(3000);
            Assert.Null(_caching.GetValue<string>("test"));
        }
    }
}
