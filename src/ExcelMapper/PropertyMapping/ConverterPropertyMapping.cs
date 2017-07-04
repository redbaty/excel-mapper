using System;
using System.Reflection;

namespace ExcelMapper.PropertyMapping
{
    public class ConverterPropertyMapping<T, TProperty> : ExcelPropertyMapping<T, TProperty>
    {
        private Func<string, TProperty> Converter { get; }

        internal ConverterPropertyMapping(MemberInfo member, Func<string, TProperty> converter) : base(member)
        {
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        internal override object GetValueFromRow(ExcelSheet sheet, ExcelHeading heading, ExcelRow row, Type type)
        {
            int index = GetIndexInRow(heading, row);
            string value = row.GetString(index);

            return Converter(value);
        }
    }
}