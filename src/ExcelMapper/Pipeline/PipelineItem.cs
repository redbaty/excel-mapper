namespace ExcelMapper.Pipeline
{
    public abstract class PipelineItem<T>
    {
        public abstract PipelineResult<T> TryMap(PipelineResult<T> item);
    }
}
