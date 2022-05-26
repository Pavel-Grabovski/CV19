using System;
using System.Globalization;
using System.Windows.Data;

namespace CV19.Infrastructure.Converters
{
    internal class ToArray : MultiConverter
    {
        public override object Convert(object[] v, Type t, object p, CultureInfo c)
        {
            var collection = new CompositeCollection();
            foreach(var value in v)
            {
                collection.Add(value);
            }
            return collection;
        }
    }
}
