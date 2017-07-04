namespace ExcelMapper.Pipeline
{
    public class ParseAsBool : PipelineItem<bool>
    {
        public override PipelineResult<bool> TryMap(PipelineResult<bool> item)
        {
            if (string.IsNullOrEmpty(item.StringValue))
            {
                return item.MakeEmpty();
            }

            if (!bool.TryParse(item.StringValue, out bool result))
            {
                return item.MakeInvalid();
            }

            return item.MakeCompleted(result);
        }
    }
}
