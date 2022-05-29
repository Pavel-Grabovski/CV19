using System;
using System.Globalization;
using System.Windows.Markup;

namespace CV19.Infrastructure.Converters
{
    [MarkupExtensionReturnType(typeof(Add))]
    internal class Add : Converter
    {
        public Add() { }

        public Add(double b) => B = b;

        public double B { get; set; } = 1;
        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is null) return null;

            var x = System.Convert.ToDouble(value, c);

            return x + B;
        }

        public override object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            if (value is null) return null;

            var x = System.Convert.ToDouble(value, c);

            return x - B;
        }
    }
}
