using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelMapper.Pipeline
{
    public class ParseWithDelimiter<TElement> : PipelineItem<IEnumerable<TElement>>
    {
        public char[] Delimiters { get; private set; }
        public StringSplitOptions Options { get; private set; }

        public ParseWithDelimiter(char delimiter) => Delimiters = new char[] { delimiter };

        public ParseWithDelimiter<TElement> WithAdditionalDelimiters(params char[] delimiters) => WithAdditionalDelimiters((IEnumerable<char>)delimiters);

        public ParseWithDelimiter<TElement> WithAdditionalDelimiters(IEnumerable<char> delimiters)
        {
            if (delimiters == null)
            {
                throw new ArgumentNullException(nameof(delimiters));
            }

            Delimiters = Delimiters.Concat(delimiters).ToArray();
            return this;
        }

        public ParseWithDelimiter<TElement> WithOptions(StringSplitOptions options)
        {
            Options = options;
            return this;
        }

        public override PipelineResult<IEnumerable<TElement>> TryMap(PipelineResult<IEnumerable<TElement>> item)
        {
            if (string.IsNullOrEmpty(item.StringValue))
            {
                return item.MakeEmpty();
            }

            string[] results = item.StringValue.Split(Delimiters, Options);
            TElement[] elements = new TElement[results.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = (TElement)Convert.ChangeType(results, typeof(TElement));
            }

            return item.MakeCompleted(elements);
        }
    }
}
