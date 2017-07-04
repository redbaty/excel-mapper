using System;
using System.Collections.Generic;

namespace ExcelMapper.Pipeline
{
    public sealed class ColumnsPipeline<T, TElement> : Pipeline<T> where T : IEnumerable<TElement>, new() where TElement : new()
    {
        public string[] ColumnNames { get; }

        public ColumnsPipeline(string[] columnNames)
        {
            if (columnNames == null)
            {
                throw new ArgumentNullException(nameof(columnNames));
            }

            if (columnNames.Length == 0)
            {
                throw new ArgumentException("Column names cannot be empty", nameof(columnNames));
            }

            ColumnNames = columnNames;
        }

        internal override object Execute(ExcelSheet sheet, ExcelRow row)
        {/*
            var elements = new List<TElement>();
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                int index = sheet.Heading.GetColumnIndex(ColumnNames[i]);
                string stringValue = row.GetString(index);

                TElement value = CompletePipeline(stringValue);
                elements.Add(value);
            }

            return elements;*/

            return null;
        }
    }
}
