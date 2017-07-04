using System;

namespace ExcelMapper.PropertyMapping
{
    public class ExcelValuePropertyFallback<TProperty> : ExcelPropertyFallback<TProperty>
    {
        public TProperty Value { get; }

        public ExcelValuePropertyFallback(TProperty value) => Value = value;

        public override TProperty Resolve()
        {
            throw new NotImplementedException();
        }
    }
}
