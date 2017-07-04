using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelMapper.Pipeline
{
    public abstract class Pipeline
    {
        internal abstract object Execute(ExcelSheet sheet, ExcelRow row);
    }

    public abstract class Pipeline<T> : Pipeline
    {
        public List<PipelineItem<T>> Items { get; private set; } = new List<PipelineItem<T>>();

        public Pipeline WithAdditionalItems(IEnumerable<PipelineItem<T>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Items = Items.Concat(items).ToList();
            return this;
        }

        protected T CompletePipeline(string stringValue)
        {
            PipelineResult<T> result = new PipelineResult<T>(PipelineStatus.Began, stringValue, default(T));
            for (int i = 0; i < Items.Count; i++)
            {
                PipelineItem<T> item = Items[0];
                result = item.TryMap(result);
            }

            return result.Result;
        }
    }
}
