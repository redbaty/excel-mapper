using System;

namespace ExcelMapper.PropertyMapping
{
    public class ExcelExceptionPropertyFallback<TProperty> : ExcelPropertyFallback<TProperty>
    {
        public override TProperty Resolve()
        {
            throw new NotImplementedException();
        }
    }
}
