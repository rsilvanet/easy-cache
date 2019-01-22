using System;
using System.Collections.Generic;
using System.Linq;
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

        public MemoryCacheTests()
        {
            _storage = new Mock<MemoryCacheStorage>()
            {
                CallBase = true
            };

            _caching = new Caching(_storage.Object);
        }
        
        
        [Fact]
        public void CanSetCache()
        {
            Assert.False(_caching.ContainsKey("test"));

            _caching.SetValue(
                "test",
                "Some value",
                TimeSpan.FromDays(1)
            );

            _storage.Verify(x => x.SetValue(
                "test",
                "Some value",
                TimeSpan.FromDays(1)
            ), Times.Once);

            Assert.True(_caching.ContainsKey("test"));
        }

        [Fact]
        public void CanGenerateCacheFromCachelessFunc()
        {
            Assert.False(_caching.ContainsKey("test"));

            _caching.GetValue(
                "test",
                () => "Some value",
                TimeSpan.FromDays(1)
            );

            _storage.Verify(x => x.SetValue(
                "test",
                "Some value",
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
                "Some value",
                TimeSpan.FromDays(1)
            );

            Assert.Equal("Some value", _caching.GetValue<string>("test"));
        }

        [Fact]
        public void CanCacheSeveralDataTypes()
        {
            var date = new DateTime(2019, 1, 21, 22, 15, 0).ToUniversalTime();

            var customObject = new TestObject
            {
                StringField = "Some value",
                IntField = 10,
                LongField = 10L,
                DoubleField = 10.99,
                DecimalField = 10.99m,
                DateField = date,
                BoolField = true,
                EnumField = TestEnum.Item2,
                ObjectField = new AnotherTestObject
                {
                    StringField = "Some value"
                },
                ListField = new List<AnotherTestObject>
                {
                    new AnotherTestObject()
                    {
                        StringField = "Some value 1"
                    },
                    new AnotherTestObject()
                    {
                         StringField = "Some value 2"
                    }
                }
            };

            _caching.SetValue("char", 'A', TimeSpan.FromDays(1));
            _caching.SetValue("string", "Some value", TimeSpan.FromDays(1));
            _caching.SetValue("int-pos", 10, TimeSpan.FromDays(1));
            _caching.SetValue("int-neg", -10, TimeSpan.FromDays(1));
            _caching.SetValue("long-pos", 10L, TimeSpan.FromDays(1));
            _caching.SetValue("long-neg", -10L, TimeSpan.FromDays(1));
            _caching.SetValue("double-pos", 10.99, TimeSpan.FromDays(1));
            _caching.SetValue("double-neg", -10.99, TimeSpan.FromDays(1));
            _caching.SetValue("decimal-pos", 10.99m, TimeSpan.FromDays(1));
            _caching.SetValue("decimal-neg", -10.99m, TimeSpan.FromDays(1));
            _caching.SetValue("datetime", date, TimeSpan.FromDays(1));
            _caching.SetValue("bool-true", true, TimeSpan.FromDays(1));
            _caching.SetValue("bool-false", false, TimeSpan.FromDays(1));
            _caching.SetValue("custom-obj", customObject, TimeSpan.FromDays(1));

            Assert.Equal('A', _caching.GetValue<char>("char"));
            Assert.Equal("Some value", _caching.GetValue<string>("string"));
            Assert.Equal(10, _caching.GetValue<int>("int-pos"));
            Assert.Equal(-10, _caching.GetValue<int>("int-neg"));
            Assert.Equal(10L, _caching.GetValue<long>("long-pos"));
            Assert.Equal(-10L, _caching.GetValue<long>("long-neg"));
            Assert.Equal(10.99, _caching.GetValue<double>("double-pos"));
            Assert.Equal(-10.99, _caching.GetValue<double>("double-neg"));
            Assert.Equal(10.99m, _caching.GetValue<decimal>("decimal-pos"));
            Assert.Equal(-10.99m, _caching.GetValue<decimal>("decimal-neg"));
            Assert.Equal(date, _caching.GetValue<DateTime>("datetime").ToUniversalTime());
            Assert.True(_caching.GetValue<bool>("bool-true"));
            Assert.False(_caching.GetValue<bool>("bool-false"));

            var cachedObject = _caching.GetValue<TestObject>("custom-obj");

            Assert.NotNull(cachedObject);
            Assert.NotNull(cachedObject.ObjectField);
            Assert.NotNull(cachedObject.ListField);
            Assert.NotEmpty(cachedObject.ListField);
            Assert.Equal(customObject.StringField, cachedObject.StringField);
            Assert.Equal(customObject.IntField, cachedObject.IntField);
            Assert.Equal(customObject.LongField, cachedObject.LongField);
            Assert.Equal(customObject.DoubleField, cachedObject.DoubleField);
            Assert.Equal(customObject.DecimalField, cachedObject.DecimalField);
            Assert.Equal(customObject.DateField, cachedObject.DateField);
            Assert.Equal(customObject.BoolField, cachedObject.BoolField);
            Assert.Equal(customObject.EnumField, cachedObject.EnumField);
            Assert.Equal(customObject.ObjectField.StringField, cachedObject.ObjectField.StringField);
            Assert.Equal(customObject.ListField.Count, cachedObject.ListField.Count);
            Assert.Equal(customObject.ListField.First().StringField, cachedObject.ListField.First().StringField);
            Assert.Equal(customObject.ListField.Last().StringField, cachedObject.ListField.Last().StringField);
        }

        [Fact]
        public void CacheExpiresCorrectly()
        {
            Assert.Null(_caching.GetValue<string>("test"));

            _caching.SetValue(
                "test",
                "Some value",
                TimeSpan.FromSeconds(1)
            );

            Assert.Equal("Some value", _caching.GetValue<string>("test"));
            Thread.Sleep(2000);
            Assert.Null(_caching.GetValue<string>("test"));
        }
    }
}
