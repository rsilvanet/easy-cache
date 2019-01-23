using System;
using System.Collections.Generic;
using System.Threading;

namespace EasyCache.API.Sample.Services
{
    public class SlowFakeDbService
    {
        public IEnumerable<string> GetSomeData()
        {
            Thread.Sleep(5000);

            return new string[] 
            {
                "Value 1",
                "Value 2",
                "Value 3",
                "Value 4",
                "Value 5",
                "Value 6",
                "Value 7",
                "Value 8",
                "Value 9",
            };
        }
    }
}