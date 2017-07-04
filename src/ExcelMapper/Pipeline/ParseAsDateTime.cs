using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExcelMapper.Pipeline
{
    public class ParseAsDateTime : PipelineItem<DateTime>
    {
        public string[] Formats { get; private set; }
        public IFormatProvider Provider { get; private set; }
        public DateTimeStyles Style { get; private set; }

        public ParseAsDateTime(string format)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            Formats = new string[] { format };
        }

        public ParseAsDateTime WithAdditionalFormats(params string[] formats) => WithAdditionalFormats((IEnumerable<string>)formats);

        public ParseAsDateTime WithAdditionalFormats(IEnumerable<string> formats)
        {
            if (formats == null)
            {
                throw new ArgumentNullException(nameof(formats));
            }

            Formats = Formats.Concat(formats).ToArray();
            return this;
        }

        public ParseAsDateTime WithProvider(IFormatProvider provider)
        {
            Provider = provider;
            return this;
        }

        public ParseAsDateTime WithStyle(DateTimeStyles style)
        {
            Style = style;
            return this;
        }

        public override PipelineResult<DateTime> TryMap(PipelineResult<DateTime> item)
        {
            if (string.IsNullOrEmpty(item.StringValue))
            {
                return item.MakeEmpty();
            }

            if (!DateTime.TryParseExact(item.StringValue, Formats, Provider, Style, out DateTime result))
            {
                return item.MakeInvalid();
            }

            return item.MakeCompleted(result);
        }
    }
}
