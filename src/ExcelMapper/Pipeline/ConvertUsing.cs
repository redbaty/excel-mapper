using System;

namespace ExcelMapper.Pipeline
{
    public delegate bool ConvertUsingSimple<T>(string stringVaule, out T result);

    public class ConvertUsing<T> : PipelineItem<T>
    {
        public Func<PipelineResult<T>, PipelineResult<T>> Converter { get; }

        public ConvertUsing(Func<PipelineResult<T>, PipelineResult<T>> converter)
        {
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public ConvertUsing(ConvertUsingSimple<T> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            Converter = item =>
            {
                if (!converter(item.StringValue, out T result))
                {
                    return item.MakeInvalid();
                }

                return item.MakeCompleted(result);
            };
        }

        public override PipelineResult<T> TryMap(PipelineResult<T> item) => Converter(item);
    }
}
