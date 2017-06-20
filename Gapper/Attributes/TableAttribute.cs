using System;

namespace Gapper.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }
        public string Schema { get; set; }
    }
}
