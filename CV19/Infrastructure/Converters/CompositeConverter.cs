using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace CV19.Infrastructure.Converters
{
    [MarkupExtensionReturnType(typeof(CompositeConverter))]
    internal class CompositeConverter : Converter
    {
        public CompositeConverter() { }

        public CompositeConverter(IValueConverter first) => this.First = First;

        public CompositeConverter(IValueConverter first, IValueConverter second) : this(first) => this.Second = Second;

        [ConstructorArgument("First")]
        public IValueConverter First { get; set; }

        [ConstructorArgument("Second")]
        public IValueConverter Second { get; set; }


        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            var result1 = First?.Convert(v, t, p, c) ?? v;
            var result2 = Second?.Convert(result1, t, p, c) ?? result1;

            return result2;
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            var result2 = Second?.Convert(v, t, p, c) ?? v;
            var result1 = First?.Convert(result2, t, p, c) ?? result2;

            return result1;
        }
    }
}
