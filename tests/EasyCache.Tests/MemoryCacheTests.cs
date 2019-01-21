using System;
using System.Collections.Generic;
using System.Threading;
using EasyCache.Storage;
using EasyCache.Tests.Shared;
using Moq;
using Xunit;

namespace EasyCache.Tests
{
    public class MemoryCacheTests
    {
        private readonly Mock<MemoryCacheStorage> _storage;
        private readonly Caching _caching;

        public static IEnumerable<object[]> DataToCache = InlineData.DataToCache;

        public MemoryCacheTests()
        {
            _storage = new Mock<MemoryCacheStorage>()
            {
                CallBase = true
            };

            _caching = new Caching(_storage.Object);
        }
        
        [Theory, MemberData(nameof(InlineData.DataToCache))]
        public void CanGenerateCacheFromSet(object value)
        {
            Assert.False(_caching.ContainsKey("test"));
            
            _caching.SetValue(
                "test", 
                value,
                TimeSpan.FromDays(1)
            );
            
            _storage.Verify(x => x.SetValue(
                "test", 
                value, 
                TimeSpan.FromDays(1)
            ), Times.Once);
            
            Assert.True(_caching.ContainsKey("test"));
        }

        [Theory, MemberData(nameof(InlineData.DataToCache))]
        public void CanGenerateCacheFromGetWithCachelessFunc(object value)
        {
            Assert.False(_caching.ContainsKey("test"));
            
            _caching.GetValue(
                "test", 
                () => value,
                TimeSpan.FromDays(1)
            );

            _storage.Verify(x => x.SetValue(
                "test", 
                value,
                TimeSpan.FromDays(1)
            ), Times.Once);
            
            Assert.True(_caching.ContainsKey("test"));
        }

        [Theory, MemberData(nameof(InlineData.DataToCache))]
        public void CanGetValueFromCache(object value)
        {
            Assert.Null(_caching.GetValue<object>("test"));

            _caching.SetValue(
                "test", 
                value,  
                TimeSpan.FromDays(1)
            );

            Assert.Equal(value, _caching.GetValue<object>("test"));
        }

        [Theory, MemberData(nameof(InlineData.DataToCache))]
        public void CacheExpiresCorrectly(object value)
        {
            Assert.Null(_caching.GetValue<object>("test"));

            _caching.SetValue(
                "test", 
                value, 
                TimeSpan.FromSeconds(1)
            );

            Assert.Equal(value, _caching.GetValue<object>("test"));
            Thread.Sleep(2000);
            Assert.Null(_caching.GetValue<object>("test"));
        }
    }
}
