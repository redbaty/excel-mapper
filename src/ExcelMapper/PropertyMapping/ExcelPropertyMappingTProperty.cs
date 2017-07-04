using System;
using System.Reflection;

namespace ExcelMapper.PropertyMapping
{
    public class ExcelPropertyMapping<TDeclaringType, TProperty> : ExcelPropertyMapping
    {
        internal ExcelPropertyMapping(MemberInfo member) : base(typeof(TDeclaringType), member) { }

        internal override object GetValueFromRow(ExcelSheet sheet, ExcelHeading heading, ExcelRow row, Type type)
        {
            int index = GetIndexInRow(heading, row);
            string stringValue = row.GetString(index);
            if (string.IsNullOrEmpty(stringValue))
            {
                return stringValue;
            }

            try
            {
                return Convert.ChangeType(stringValue, type);
            }
            catch
            {
                throw new ExcelMappingException($"Could not convert {stringValue} to type {type}", GetErrorMessageLocation(), sheet, row);
            }
        }
    }
}
