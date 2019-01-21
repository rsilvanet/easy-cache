using System;
using System.Runtime.Serialization;

namespace EasyCache.Tests
{
    public class TestObject
    {
        public string StringField { get; set; }
        public int IntField { get; set; }
        public long LongField { get; set; }
        public double DoubleField { get; set; }
        public decimal DecimalField { get; set; }
        public DateTime DateField { get; set; }
        public bool BoolField { get; set; }
    }
}