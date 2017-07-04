using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExcelMapper.PropertyMapping
{
    public class BoolPropertyMapping<T> : ExcelPropertyMapping<T, bool>
    {
        public Dictionary<string, bool> Mappings { get; }
        public bool? EmptyFallback { get; private set; }
        public bool? InvalidFallback { get; private set; }
        public bool Trim { get; private set; } = true;

        internal BoolPropertyMapping(MemberInfo member, Dictionary<string, bool> mappings) : base(member)
        {
            Mappings = mappings;
        }

        public BoolPropertyMapping<T> WithEmptyFallback(bool? emptyFallback)
        {
            EmptyFallback = emptyFallback;
            return this;
        }

        public BoolPropertyMapping<T> WithInvalidFallback(bool? invalidFallback)
        {
            InvalidFallback = invalidFallback;
            return this;
        }

        public BoolPropertyMapping<T> WithTrim(bool trim)
        {
            Trim = trim;
            return this;
        }

        internal override object GetValueFromRow(ExcelSheet sheet, ExcelHeading heading, ExcelRow row, Type type)
        {
            int index = GetIndexInRow(heading, row);
            string stringValue = row.GetString(index);
            if (Trim)
            {
                stringValue = stringValue?.Trim();
            }

            if (string.IsNullOrEmpty(stringValue))
            {
                if (EmptyFallback.HasValue)
                {
                    return EmptyFallback.Value;
                }

                throw new ExcelMappingException("Empty value not allowed", GetErrorMessageLocation(), sheet, row);
            }

            bool success = true;
            bool value;
            if (Mappings == null)
            {
                success = bool.TryParse(stringValue, out value);
            }
            else
            {
                success = Mappings.TryGetValue(stringValue, out value);
            }

            if (!success)
            {
                if (InvalidFallback.HasValue)
                {
                    return InvalidFallback.Value;
                }

                string mappings = string.Join(", ", Mappings.Keys.Select(key => $"\"{key}\""));
                throw new ExcelMappingException($"Invalid value \"{stringValue}\" for enum \"{type}\" with mapping", GetErrorMessageLocation(), sheet, row);
            }

            return value;
        }
    }
}
