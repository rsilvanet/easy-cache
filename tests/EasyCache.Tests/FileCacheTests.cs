using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Moq;
using Xunit;

namespace EasyCache.Tests
{
    public class FileCacheTests
    {
        private readonly Mock<FileCacheStorage> _storage;
        private readonly Caching _caching;
        
        public static IEnumerable<object[]> DataToCache = InlineData.DataToCache;

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

            _storage = new Mock<FileCacheStorage>(tempPath)
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

            if (value is DateTime)
                Assert.Equal(((DateTime)value).ToString("u"), _caching.GetValue<DateTime>("test").ToString("u"));
            else
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

            if (value is DateTime)
                Assert.Equal(((DateTime)value).ToString("u"), _caching.GetValue<DateTime>("test").ToString("u"));
            else
                Assert.Equal(value, _caching.GetValue<object>("test"));

            Thread.Sleep(2000);

            Assert.Null(_caching.GetValue<object>("test"));
        }
    }
}
