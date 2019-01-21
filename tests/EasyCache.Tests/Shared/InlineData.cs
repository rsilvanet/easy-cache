using System;
using System.Collections.Generic;

namespace EasyCache.Tests.Shared
{
    public class InlineData
    {
        public static IEnumerable<object[]> DataToCache = new List<object[]>
        {
            new object[] { "Test value" },
            new object[] { 10 },
            new object[] { -10 },
            new object[] { 10.99m },
            new object[] { -10.99m },
            new object[] { true },
            new object[] { false },
            new object[] { DateTime.Now },
            // new object[] { new TestObject
            //     {
            //         StringField = "Test value",
            //         IntField = 10,
            //         LongField = 10L,
            //         DoubleField = 10.99,
            //         DecimalField = 10.99m,
            //         BoolField = true
            //     }
            // }
        };
    }
}