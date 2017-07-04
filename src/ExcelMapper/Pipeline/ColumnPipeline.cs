using System;

namespace ExcelMapper.Pipeline
{
    public sealed class ColumnPipeline<T> : Pipeline<T>
    {
        public string ColumnName { get; }

        public ColumnPipeline(string columnName)
        {
            if (columnName == null)
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            if (columnName.Length == 0)
            {
                throw new ArgumentException(nameof(columnName));
            }

            ColumnName = columnName;
        }

        internal override object Execute(ExcelSheet sheet, ExcelRow row)
        {
            int index = sheet.Heading.GetColumnIndex(ColumnName);
            string stringValue = row.GetString(index);

            return CompletePipeline(stringValue);
        }
    }
}
