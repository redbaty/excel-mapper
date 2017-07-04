using System;

namespace ExcelMapper.PropertyMapping
{
    public static class ExcelPropertyMappingExtensions
    {
        public static T WithName<T>(this T mapping, string columnName) where T : ExcelPropertyMapping
        {
            if (columnName == null)
            {
                throw new ArgumentNullException(nameof(columnName));
            }
            if (columnName.Length == 0)
            {
                throw new ArgumentException(nameof(columnName), "Column name cannot be null or empty");
            }

            mapping.Index = 0;
            mapping.ColumnName = columnName;

            return mapping;
        }

        public static T WithIndex<T>(this T mapping, int index) where T : ExcelPropertyMapping
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            mapping.ColumnName = null;
            mapping.Index = index;

            return mapping;
        }
        
        public static T WithRequired<T>(this T mapping, bool required) where T : ExcelPropertyMapping
        {
            mapping.Required = required;

            return mapping;
        }
    }
}
