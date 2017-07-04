using System;
using System.Linq.Expressions;

namespace ExcelMapper
{
    public class ExcelClassMap<T> : ExcelClassMap where T : new()
    {
        public ExcelClassMap() : base(typeof(T)) { }

        public ExcelPropertyMap<T, TProperty> Map<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            MemberExpression memberExpression = ValidateExpression(expression);

            var propertyMap = new ExcelPropertyMap<T, TProperty>(memberExpression.Member);
            AddMapping(propertyMap);
            return propertyMap;
        }
    }
}
