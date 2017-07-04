using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExcelMapper
{
    public abstract class ExcelClassMap
    {
        public Type Type { get; }

        private List<ExcelPropertyMap> Mappings { get; } = new List<ExcelPropertyMap>();

        internal ExcelClassMap(Type type) => Type = type;

        protected internal void AddMapping(ExcelPropertyMap property)
        {
            Mappings.Add(property);
        }

        internal object Execute(ExcelSheet sheet, ExcelRow row)
        {
            object value = Activator.CreateInstance(Type);

            foreach (ExcelPropertyMap propertyMapping in Mappings)
            {
                propertyMapping.Execute(value, sheet, row);
            }

            return value;
        }

        protected internal static MemberExpression ValidateExpression<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                throw new InvalidOperationException("Not a member expression.");
            }

            return memberExpression;
        }
    }
}
