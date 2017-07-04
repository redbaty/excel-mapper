using System;
using System.Reflection;

namespace ExcelMapper
{
    public class ExcelPropertyMap
    {
        public Pipeline.Pipeline Pipeline { get; protected internal set; }

        public Type Type { get; }
        public MemberInfo Member { get; }

        internal ExcelPropertyMap(Type type, MemberInfo member)
        {
            Type = type;
            Member = member;
        }

        internal void Execute(object value, ExcelSheet sheet, ExcelRow row)
        {
            object propertyValue = Pipeline.Execute(sheet, row);
            if (Member is FieldInfo field)
            {
                field.SetValue(value, propertyValue);
            }
            else if (Member is PropertyInfo property)
            {
                property.SetValue(value, propertyValue);
            }
            else
            {
                throw new ExcelMappingException("Unknown member.");
            }
        }
    }
}
