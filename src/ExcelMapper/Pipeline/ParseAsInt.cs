using System;
using System.Globalization;

namespace ExcelMapper.Pipeline
{
    public class ParseAsInt : PipelineItem<int>
    {
        public NumberStyles Style { get; private set; }
        public IFormatProvider Provider { get; private set; }

        public ParseAsInt WithStyle(NumberStyles style)
        {
            Style = style;
            return this;
        }

        public ParseAsInt WithProvider(IFormatProvider provider)
        {
            Provider = provider;
            return this;
        }

        public override PipelineResult<int> TryMap(PipelineResult<int> item)
        {
            if (string.IsNullOrEmpty(item.StringValue))
            {
                return item.MakeEmpty();
            }

            if (!int.TryParse(item.StringValue, out int result))
            {
                return item.MakeInvalid();
            }

            return item.MakeCompleted(result);
        }
    }
}
