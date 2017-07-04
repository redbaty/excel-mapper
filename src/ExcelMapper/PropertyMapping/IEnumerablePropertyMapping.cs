using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExcelMapper.PropertyMapping
{
    public class IEnumerablePropertyMapping<TDeclaringType, TElement> : ExcelPropertyMapping<TDeclaringType, IEnumerable<TElement>>
    {
        public IEnumerable<string> ColumnNames { get; }

        internal IEnumerablePropertyMapping(MemberInfo member, string[] columnNames) : base(member)
        {
            ColumnNames = columnNames ?? throw new ArgumentNullException(nameof(columnNames));
        }

        internal override object GetValueFromRow(ExcelSheet sheet, ExcelHeading heading, ExcelRow row, Type type)
        {
            var elements = new List<TElement>();
            foreach (string columnName in ColumnNames)
            {
                int columnIndex = heading.GetColumnIndex(columnName);
                string value = row.GetString(columnIndex);
                if (string.IsNullOrEmpty(value))
                {
                    elements.Add(default(TElement));
                    continue;
                }

                TElement element = (TElement)Convert.ChangeType(value, typeof(TElement));
                elements.Add(element);
            }

            return elements;
        }
    }
}
