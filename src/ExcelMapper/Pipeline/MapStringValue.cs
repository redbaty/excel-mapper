using System;
using System.Collections.Generic;

namespace ExcelMapper.Pipeline
{
    public class MapStringValue<T> : PipelineItem<T>
    {
        public IReadOnlyDictionary<string, T> Mapping { get; }

        public MapStringValue(IDictionary<string, T> mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException();
            }

            Mapping = new Dictionary<string, T>(mapping);
        }

        public override PipelineResult<T> TryMap(PipelineResult<T> item)
        {
            if (string.IsNullOrEmpty(item.StringValue))
            {
                return item.MakeEmpty();
            }

            if (!Mapping.TryGetValue(item.StringValue, out T result))
            {
                return item.MakeInvalid();
            }

            return item.MakeCompleted(result);
        }
    }
}
