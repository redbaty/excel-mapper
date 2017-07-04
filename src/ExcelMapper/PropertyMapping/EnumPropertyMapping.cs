using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExcelMapper.PropertyMapping
{
    public class EnumPropertyMapping<T, TEnum> : ExcelPropertyMapping<T, TEnum> where TEnum : struct
    {
        public Dictionary<string, TEnum> Mappings { get; }
        public TEnum? EmptyFallback { get; private set; }
        public TEnum? InvalidFallback { get; private set; }
        public bool Trim { get; private set; } = true;

        internal EnumPropertyMapping(MemberInfo member, Dictionary<string, TEnum> mappings) : base(member)
        {
            Mappings = mappings;
        }

        public EnumPropertyMapping<T, TEnum> WithEmptyFallback(TEnum? emptyFallback)
        {
            EmptyFallback = emptyFallback;
            return this;
        }

        public EnumPropertyMapping<T, TEnum> WithInvalidFallback(TEnum? invalidFallback)
        {
            InvalidFallback = invalidFallback;
            return this;
        }

        public EnumPropertyMapping<T, TEnum> WithTrim(bool trim)
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
            TEnum value = default(TEnum);
            if (Mappings == null)
            {
                try
                {
                    value = (TEnum)Enum.Parse(type, stringValue);
                }
                catch
                {
                    success = false;
                }
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
