using System;
using System.Reflection;

namespace ExcelMapper.PropertyMapping
{
    /// <summary>
    /// A type erased property mapping.
    /// </summary>
    public abstract class ExcelPropertyMapping
    {
        public Type Type { get; }
        public MemberInfo Member { get; }

        public string ColumnName { get; internal set; }
        public int Index { get; internal set; }
        public bool Required { get; internal set; } = true;

        internal ExcelPropertyMapping(Type type, MemberInfo member)
        {
            Type = type;
            Member = member;
            ColumnName = member.Name;
        }

        internal void Resolve(ExcelSheet sheet, ExcelHeading heading, ExcelRow row, object parent)
        {
            if (Member is FieldInfo field)
            {
                object value = GetValueFromRow(sheet, heading, row, field.FieldType);
                field.SetValue(parent, value);
            }
            else if (Member is PropertyInfo property)
            {
                object value = GetValueFromRow(sheet, heading, row, property.PropertyType);
                property.SetValue(parent, value);
            }
        }

        internal abstract object GetValueFromRow(ExcelSheet sheet, ExcelHeading heading, ExcelRow row, Type type);

        protected int GetIndexInRow(ExcelHeading heading, ExcelRow row)
        {
            if (heading == null && ColumnName != null)
            {
                throw new InvalidOperationException("No heading");
            }

            int index = ColumnName == null ? Index : heading.GetColumnIndex(ColumnName);
            return index;
        }

        protected string GetErrorMessageLocation() => $"\"{ColumnName}\"" ?? $"index {Index.ToString()}";
    }
}
