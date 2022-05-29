using System;
using System.Windows.Markup;
using System.Linq;

namespace CV19.Infrastructure.Common
{
    [MarkupExtensionReturnType(typeof(int[]))]
    internal class StringToIntArray : MarkupExtension
    {
        [ConstructorArgument("Str")]
        public string Str { get; set; }

        public char Separator { get; set; } = ';';

        public StringToIntArray() { }

        public StringToIntArray(string str) => Str = str;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Str.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries)
                .DefaultIfEmpty()
                .Select(int.Parse)
                .ToArray();
        }
    }
}
