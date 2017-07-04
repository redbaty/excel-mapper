using System;

namespace ExcelMapper.Pipeline
{
    public class ThrowIfStatus<T> : PipelineItem<T>
    {
        public PipelineStatus Status { get; }

        public ThrowIfStatus(PipelineStatus status)
        {
            if (!Enum.IsDefined(typeof(PipelineStatus), status))
            {
                throw new ArgumentException("Invalid status type.", nameof(status));
            }

            Status = status;
        }

        public override PipelineResult<T> TryMap(PipelineResult<T> item)
        {
            if ((item.Status & Status) != 0)
            {
                throw new ExcelMappingException($"Invalid parameter {item.StringValue}.");
            }

            return item;
        }
    }
}
