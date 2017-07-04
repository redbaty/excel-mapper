using System;

namespace ExcelMapper.Pipeline
{
    public sealed class IndexPipeline<T> : Pipeline<T>
    {
        public int Index { get; }

        public IndexPipeline(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            Index = index;
        }

        internal override object Execute(ExcelSheet sheet, ExcelRow row)
        {
            string stringValue = row.GetString(row.Index);
            return CompletePipeline(stringValue);
        }
    }
}
