using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ExcelMapper.PropertyMapping
{
    public class DatePropertyMapping<TDeclaringType> : ExcelPropertyMapping<TDeclaringType, DateTime>
    {
        public string[] Formats { get; private set; }
        public IFormatProvider FormatProvider { get; }
        public DateTime? EmptyFallback { get; private set; } = DateTime.MinValue;
        public DateTime? InvalidFallback { get; private set; }

        internal DatePropertyMapping(MemberInfo member, string format, IFormatProvider formatProvider) : base(member)
        {
            Formats = new string[] { format };
            FormatProvider = formatProvider;
        }

        public DatePropertyMapping<TDeclaringType> WithAdditionalFallbackFormats(params string[] formats)
        {
            if (formats == null)
            {
                throw new ArgumentNullException(nameof(formats));
            }
            
            foreach (string format in formats)
            {
                if (format == null)
                {
                    throw new ArgumentNullException(nameof(format));
                }
            }

            Formats = Formats.Concat(formats).ToArray();
            return this;
        }

        public DatePropertyMapping<TDeclaringType> WithEmptyFallback(DateTime? emptyFallback)
        {
            EmptyFallback = emptyFallback;
            return this;
        }

        public DatePropertyMapping<TDeclaringType> WithInvalidFallback(DateTime? invalidFallback)
        {
            InvalidFallback = invalidFallback;
            return this;
        }

        internal override object GetValueFromRow(ExcelSheet sheet, ExcelHeading heading, ExcelRow row, Type type)
        {
            int index = GetIndexInRow(heading, row);
            string dateTimeString = row.GetString(index);

            if (string.IsNullOrEmpty(dateTimeString))
            {
                if (EmptyFallback.HasValue)
                {
                    return EmptyFallback.Value;
                }

                throw new ExcelMappingException("Empty value not allowed", GetErrorMessageLocation(), sheet, row);
            }

            if (!DateTime.TryParseExact(dateTimeString, Formats, FormatProvider, DateTimeStyles.AllowWhiteSpaces, out DateTime date))
            {
                if (InvalidFallback.HasValue)
                {
                    return InvalidFallback.Value;
                }

                string formats = string.Join(", ", Formats.Select(format => $"\"{format}\""));
                throw new ExcelMappingException($"String {dateTimeString} was not a valid string for formats [{formats}]", GetErrorMessageLocation(), sheet, row);
            }

            return date;
        }
    }
}
